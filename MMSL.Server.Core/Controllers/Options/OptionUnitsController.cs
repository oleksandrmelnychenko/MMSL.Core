using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common;
using MMSL.Common.Helpers;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Options;
using MMSL.Domain.DataContracts.ProductOptions;
using MMSL.Domain.Entities.Options;
using MMSL.Server.Core.Helpers;
using MMSL.Services.OptionServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Options {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    public class OptionUnitsController : WebApiControllerBase {

        private readonly IOptionUnitService _optionUnitService;

        public OptionUnitsController(IOptionUnitService optionUnitService, IResponseFactory responseFactory, IStringLocalizer<OptionUnitsController> localizer)
            : base(responseFactory, localizer) {
            _optionUnitService = optionUnitService;
        }

        [HttpGet]
        [AssignActionRoute(OptionUnitSegments.GET_OPTION_UNITS)]
        public async Task<IActionResult> GetOptionUnit([FromQuery] long optionUnitId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.GetOptionUnitByIdAsync(optionUnitId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(OptionUnitSegments.GET_GROUP_OPTION_UNITS)]
        public async Task<IActionResult> GetGroupOptionUnit([FromQuery] long optionGroupId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.GetOptionUnitsByGroupIdAsync(optionGroupId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(OptionUnitSegments.ADD_OPTION_UNIT)]
        public async Task<IActionResult> AddOptionUnit(
            [FromForm] OptionUnitDataContract optionUnit,
            [FromForm] FileFormData formData) {
            try {
                decimal priceValue = 0;
                bool priceAvailable = optionUnit.Price != null && NumericParsingHelper.TryParsePrice(optionUnit.Price, out priceValue);

                if (priceAvailable && !optionUnit.CurrencyTypeId.HasValue)
                    throw new ArgumentNullException(nameof(optionUnit.CurrencyTypeId));

                OptionUnit optionUnitEntity = optionUnit.GetEntity();

                List<UnitValueDataContract> values = !string.IsNullOrEmpty(optionUnit.SerializedValues)
                    ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<UnitValueDataContract>>(optionUnit.SerializedValues)
                    : null;

                if (formData.File != null) {
                    optionUnitEntity.ImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", formData.File);
                    optionUnitEntity.ImageName = formData.File.FileName;
                }

                OptionPrice price = priceAvailable
                    ? new OptionPrice {
                        Price = priceValue,
                        CurrencyTypeId = optionUnit.CurrencyTypeId.Value
                    }
                    : null;

                return Ok(SuccessResponseBody(
                    await _optionUnitService.AddOptionUnit(optionUnitEntity, values, price),
                    Localizer["Successfully created"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(OptionUnitSegments.UPDATE_OPTION_UNIT)]
        public async Task<IActionResult> UpdateOptionUnit([FromForm] OptionUnitUpdateDataContract updateOptionUnit, [FromForm] FileFormData formData) {
            try {
                decimal priceValue = 0;
                bool priceAvailable = updateOptionUnit.Price != null && NumericParsingHelper.TryParsePrice(updateOptionUnit.Price, out priceValue);

                if (priceAvailable && !updateOptionUnit.CurrencyTypeId.HasValue)
                    throw new ArgumentNullException(nameof(updateOptionUnit.CurrencyTypeId));

                if (updateOptionUnit == null)
                    throw new ArgumentNullException(nameof(updateOptionUnit));

                OptionUnit entity = updateOptionUnit.GetEntity();

                if (entity.IsNew())
                    throw new ArgumentException(nameof(updateOptionUnit.Id));

                List<UnitValueDataContract> values = !string.IsNullOrEmpty(updateOptionUnit.SerializedValues)
                    ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<UnitValueDataContract>>(updateOptionUnit.SerializedValues)
                    : null;

                string oldImage = string.Empty;
                if (!string.IsNullOrEmpty(entity.ImageUrl)) {
                    OptionUnit oldEntity = await _optionUnitService.GetOptionUnitByIdAsync(entity.Id);
                    oldImage = oldEntity.ImageUrl;
                }

                if (formData.File != null) {
                    entity.ImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", formData.File);
                    entity.ImageName = formData.File.FileName;
                }

                OptionPrice price = priceAvailable
                    ? new OptionPrice {
                        Price = priceValue,
                        CurrencyTypeId = updateOptionUnit.CurrencyTypeId.Value,
                        OptionUnitId = updateOptionUnit.Id
                    }
                    : null;

                OptionUnit result = await _optionUnitService.UpdateOptionUnit(entity, values, price);

                if ((string.IsNullOrEmpty(oldImage) || oldImage != entity.ImageUrl) && !string.IsNullOrEmpty(oldImage)) {
                    FileUploadingHelper.DeleteFile($"{Request.Scheme}://{Request.Host}", oldImage);
                }

                return Ok(SuccessResponseBody(result, Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(OptionUnitSegments.DELETE_OPTION_UNIT)]
        public async Task<IActionResult> DeleteOptionUnit([FromQuery] long optionUnitId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.DeleteOptionUnit(optionUnitId), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(OptionUnitSegments.UPDATE_ORDER_INDEX)]
        public async Task<IActionResult> UpdateOptionUnit([FromBody] List<UpdateOrderIndexDataContract> orderIndexes) {
            try {
                await _optionUnitService.UpdateOrderIndexesAsync(orderIndexes);

                return Ok(SuccessResponseBody(orderIndexes, Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
