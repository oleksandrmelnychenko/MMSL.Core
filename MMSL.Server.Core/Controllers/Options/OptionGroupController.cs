using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.Helpers;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Options;
using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.ProductOptions;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Entities.Stores;
using MMSL.Services.OptionServices.Contracts;
using MMSL.Services.StoreServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Options {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    public class OptionGroupController : WebApiControllerBase {

        private readonly IOptionGroupService _optionGroupService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public OptionGroupController(
            IOptionGroupService optionGroupService,
            IResponseFactory responseFactory,
            IStringLocalizer<OptionGroupController> localizer) : base(responseFactory, localizer) {

            _optionGroupService = optionGroupService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(OptionGroupSegments.GET_OPTION_GROUPS)]
        public async Task<IActionResult> GetAll([FromQuery]string search, [FromQuery] long productCategoryId) {
            try {
                return Ok(SuccessResponseBody(await _optionGroupService.GetOptionGroupsAsync(search, productCategoryId), Localizer["Successfully completed"]));
            }
            catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(OptionGroupSegments.GET_OPTION_GROUP_BY_ID)]
        public async Task<IActionResult> GetById([FromQuery]long groupId) {
            try {
                if (groupId == default(long))
                    throw new ArgumentNullException(nameof(groupId));

                return Ok(SuccessResponseBody(await _optionGroupService.GetOptionGroupAsync(groupId), Localizer["Successfully completed"]));
            } catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(OptionGroupSegments.NEW_OPTION_GROUP)]
        public async Task<IActionResult> NewOptionGroup([FromBody] NewOptionGroupDataContract newOptionGroupDataContract) {
            try {
                if (newOptionGroupDataContract == null)
                    throw new ArgumentNullException("NewOptionGroupDataContract");

                if (string.IsNullOrEmpty(newOptionGroupDataContract.Name))
                    throw new ArgumentNullException("Name");

                if (newOptionGroupDataContract.ProductId == default(long))
                    throw new ArgumentNullException("ProductId");

                decimal priceValue = 0;
                bool priceAvailable = newOptionGroupDataContract.Price != null && NumericParsingHelper.TryParsePrice(newOptionGroupDataContract.Price, out priceValue);

                if (priceAvailable && !newOptionGroupDataContract.CurrencyTypeId.HasValue)
                    throw new ArgumentNullException(nameof(newOptionGroupDataContract.CurrencyTypeId));

                OptionPrice price = priceAvailable
                    ? new OptionPrice {
                        Price = priceValue,
                        CurrencyTypeId = newOptionGroupDataContract.CurrencyTypeId.Value
                    }
                    : null;

                return Ok(SuccessResponseBody(await _optionGroupService.NewOptionGroupAsync(newOptionGroupDataContract, price), Localizer["Style has been created successfully"]));
            }           
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(OptionGroupSegments.UPDATE_OPTION_GROUP)]
        public async Task<IActionResult> UpdateOptionGroup([FromBody] UpdateOptionGroupDataContract optionGroup) {
            try {
                if (optionGroup == null)
                    throw new ArgumentNullException("UpdateOptionGroup");

                decimal priceValue = 0;
                bool priceAvailable = optionGroup.Price != null && NumericParsingHelper.TryParsePrice(optionGroup.Price, out priceValue);

                if (priceAvailable && !optionGroup.CurrencyTypeId.HasValue)
                    throw new ArgumentNullException(nameof(optionGroup.CurrencyTypeId));

                OptionPrice price = priceAvailable
                    ? new OptionPrice {
                        Price = priceValue,
                        CurrencyTypeId = optionGroup.CurrencyTypeId.Value,
                        OptionGroupId = optionGroup.Id
                    }
                    : null;

                await _optionGroupService.UpdateOptionGroupAsync(optionGroup, price);

                return Ok(SuccessResponseBody(optionGroup, Localizer["Style successfully updated"]));
            }
            catch (NotFoundValueException exc) {
                Log.Error(exc.GetUserMessageException);
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(OptionGroupSegments.DELETE_OPTION_GROUP)]
        public async Task<IActionResult> DeleteStore([FromQuery]long optionGroupId) {
            try {
                await _optionGroupService.DeleteOptionGroupAsync(optionGroupId);

                return Ok(SuccessResponseBody(optionGroupId, Localizer["Style successfully deleted"]));
            }
            catch (NotFoundValueException exc) {
                Log.Error(exc.GetUserMessageException);
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
