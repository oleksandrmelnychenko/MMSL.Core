using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.IdentityConfiguration;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Services.IdentityServices.Contracts;
using MMSL.Services.StoreCustomerServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Stores {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.StoreCustomers)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.StoreCustomers)]
    public class StoreCustomersController : WebApiControllerBase {

        private readonly IStoreCustomerService _storeCustomerService;

        private readonly IUserIdentityService _userIdentityService;

        public StoreCustomersController(IStoreCustomerService storeCustomerService,
            IUserIdentityService userIdentityService,
            IResponseFactory responseFactory, IStringLocalizer<StoreCustomersController> localizer) 
            : base(responseFactory, localizer) {
            _storeCustomerService = storeCustomerService;
            _userIdentityService = userIdentityService;
        }

        [HttpGet]
        [AssignActionRoute(StoreCustomerSegments.GET_ALL_STORE_CUSTOMER)]
        public async Task<IActionResult> GetAll(
            [FromQuery]long? storeId,
            [FromQuery]int pageNumber,
            [FromQuery]int limit,
            [FromQuery]string searchPhrase,
            [FromQuery]string storeName) {
            try {
                return Ok(SuccessResponseBody(await _storeCustomerService.GetCustomersByStoreAsync(pageNumber, limit, searchPhrase, storeName, storeId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(StoreCustomerSegments.GET_STORE_CUSTOMER)]
        public async Task<IActionResult> Get([FromQuery]long storeCustomerId) {
            try {
                return Ok(SuccessResponseBody(await _storeCustomerService.GetCustomerAsync(storeCustomerId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(StoreCustomerSegments.ADD_STORE_CUSTOMER)]
        public async Task<IActionResult> Add([FromBody]StoreCustomer storeCustomer) {
            try {
                if (storeCustomer == null) throw new ArgumentNullException("storeCustomer");

                UserAccount customerIdentity = await _userIdentityService.NewUser(
                    new NewUserDataContract {
                        Email = storeCustomer.Email,
                        Password = PasswordGenerationHelper.GetRandomPassword(),
                        PasswordExpiresAt = DateTime.Now.AddDays(7),
                        Roles = new List<RoleType> { RoleType.Customer }
                    });

                storeCustomer.UserIdentityId = customerIdentity.Id;

                StoreCustomer customerEntity = await _storeCustomerService.AddCustomerAsync(storeCustomer);

                return Ok(SuccessResponseBody(customerEntity, Localizer["Successfully added"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(StoreCustomerSegments.UPDATE_STORE_CUSTOMER)]
        public async Task<IActionResult> Update([FromBody]StoreCustomer storeCustomer) {
            try {
                return Ok(SuccessResponseBody(await _storeCustomerService.UpdateCustomerAsync(storeCustomer), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(StoreCustomerSegments.DELETE_STORE_CUSTOMER)]
        public async Task<IActionResult> Delete([FromQuery]long storeCustomerId) {
            try {
                return Ok(SuccessResponseBody(await _storeCustomerService.DeleteCustomerAsync(storeCustomerId), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }


    }
}
