using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Helpers;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.DataContracts.Customer;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Services.StoreCustomerServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Stores {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.StoreCustomerProductProfile)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.StoreCustomerProductProfile)]
    public class StoreCustomerProductProfileController : WebApiControllerBase {

        private readonly IStoreCustomerProductProfileService _storeCustomerProductProfileService;

        public StoreCustomerProductProfileController(
            IStoreCustomerProductProfileService storeCustomerProductProfileService,
            IResponseFactory responseFactory,
            IStringLocalizer<StoreCustomerProductProfileController> localizer)
            : base(responseFactory, localizer) {
            _storeCustomerProductProfileService = storeCustomerProductProfileService;
        }

        [HttpGet]
        [Authorize(Roles = "Dealer")]
        [AssignActionRoute(StoreCustomerProductProfileSegments.GET_PROFILES)]
        public async Task<IActionResult> GetAll([FromQuery] string searchPhrase, [FromQuery] long? productId) {
            try {               
                long dealerIdentityId = ClaimHelper.GetUserId(User);

                List<CustomerProductProfile> profiles = await _storeCustomerProductProfileService.GetAllAsync(dealerIdentityId, productId, searchPhrase);

                return Ok(SuccessResponseBody(profiles, Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Dealer")]
        [AssignActionRoute(StoreCustomerProductProfileSegments.ADD_PROFILE)]
        public async Task<IActionResult> Add([FromBody] NewCustomerProductProfile newProfileDataContract) {
            try {
                long dealerIdentityId = ClaimHelper.GetUserId(User);

                return Ok(SuccessResponseBody(await _storeCustomerProductProfileService.AddAsync(dealerIdentityId, newProfileDataContract), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Dealer")]
        [AssignActionRoute(StoreCustomerProductProfileSegments.UPDATE_PROFILE)]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerProductProfile profileDataContract) {
            try {
                long dealerIdentityId = ClaimHelper.GetUserId(User);

                return Ok(SuccessResponseBody(
                    await _storeCustomerProductProfileService.UpdateAsync(profileDataContract),
                    Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Dealer")]
        [AssignActionRoute(StoreCustomerProductProfileSegments.DELETE_PROFILE)]
        public async Task<IActionResult> Delete([FromQuery] long customerProductProfileId) {
            try {
                return Ok(SuccessResponseBody(
                    await _storeCustomerProductProfileService.DeleteAsync(customerProductProfileId),
                    Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
