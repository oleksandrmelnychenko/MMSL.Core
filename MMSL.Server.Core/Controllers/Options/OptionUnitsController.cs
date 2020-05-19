﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Options;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Options;
using MMSL.Server.Core.Helpers;
using MMSL.Services.OptionServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Options {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    public class OptionUnitsController : WebApiControllerBase {

        private IOptionUnitService _optionUnitService;

        public OptionUnitsController(IOptionUnitService optionUnitService, IResponseFactory responseFactory, IStringLocalizer<OptionUnitsController> localizer)
            : base(responseFactory, localizer) {
            _optionUnitService = optionUnitService;
        }

        [HttpGet]
        [AssignActionRoute(OptionUnitSegments.GET_OPTION_UNITS)]
        public async Task<IActionResult> GetOptionUnit([FromQuery]long optionUnitId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.GetOptionUnitByIdAsync(optionUnitId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(OptionUnitSegments.GET_GROUP_OPTION_UNITS)]
        public async Task<IActionResult> GetGroupOptionUnit([FromQuery]long optionGroupId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.GetOptionUnitsByGroupIdAsync(optionGroupId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(OptionUnitSegments.ADD_OPTION_UNIT)]
        public async Task<IActionResult> AddOptionUnit([FromQuery]OptionUnitDataContract optionUnit, [FromForm]FileFormData formData) {
            try {
                OptionUnit optionUnitEntity = optionUnit.GetEntity();

                if (formData.File != null) {
                    optionUnitEntity.ImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", formData.File);
                }

                return Ok(SuccessResponseBody(await _optionUnitService.AddOptionUnit(optionUnitEntity), Localizer["Successfully created"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(OptionUnitSegments.UPDATE_OPTION_UNIT)]
        public async Task<IActionResult> UpdateOptionUnit([FromQuery]OptionUnitUpdateDataContract updateOptionUnit, [FromForm]FileFormData formData) {
            try {
                if (updateOptionUnit == null)
                    throw new ArgumentNullException(nameof(updateOptionUnit));

                OptionUnit entity = updateOptionUnit.GetEntity();

                if (entity.IsNew())
                    throw new ArgumentException(nameof(updateOptionUnit.Id));

                string oldImage = entity.ImageUrl;

                if (formData.File != null) {
                    entity.ImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", formData.File);
                }

                if (string.IsNullOrEmpty(oldImage) || oldImage != entity.ImageUrl) {
                    OptionUnit oldEntity = await _optionUnitService.GetOptionUnitByIdAsync(entity.Id);

                    if (!string.IsNullOrEmpty(oldEntity.ImageUrl)) {
                        FileUploadingHelper.DeleteFile($"{Request.Scheme}://{Request.Host}", oldEntity.ImageUrl);
                    }
                }

                return Ok(SuccessResponseBody(await _optionUnitService.UpdateOptionUnit(entity), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(OptionUnitSegments.DELETE_OPTION_UNIT)]
        public async Task<IActionResult> DeleteOptionUnit([FromQuery]long optionUnitId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.DeleteOptionUnit(optionUnitId), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(OptionUnitSegments.UPDATE_ORDER_INDEX)]
        public async Task<IActionResult> UpdateOptionUnit([FromBody]List<UpdateOrderIndexDataContract> orderIndexes) {
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
