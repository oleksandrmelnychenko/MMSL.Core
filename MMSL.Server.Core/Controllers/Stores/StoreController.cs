using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Stores;
using MMSL.Services.StoreServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.BankDetails {
    //[AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.BankDetails)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Stores)]
    public class StoreController : WebApiControllerBase {

        private readonly IStoreService _storeService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="bankDetailsService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public StoreController(
            IStoreService storeService,
            IResponseFactory responseFactory,
            IStringLocalizer<StoreController> localizer) : base(responseFactory, localizer) {

            _storeService = storeService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(StoreSegments.GET_STORES)]
        public async Task<IActionResult> GetAll() {
            try {
                List<Store> bankDetails = await _storeService.GetAllStoresAsync();

                return Ok(SuccessResponseBody(bankDetails, Localizer["Successfully completed"]));
            }
            catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(StoreSegments.NEW_STORE)]
        public async Task<IActionResult> NewStore([FromBody] NewStoreDataContract newStoreDataContract) {
            try {
                if (newStoreDataContract == null) throw new ArgumentNullException("NewStoreDataContract");

                Store store = await _storeService.NewStoreAsync(newStoreDataContract);

                return Ok(SuccessResponseBody(store, Localizer["New store has been created successfully"]));
            }
            catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(StoreSegments.UPDATE_STORE)]
        public async Task<IActionResult> UpdateStore([FromBody]Store store) {
            try {
                if (store == null) throw new ArgumentNullException("UpdateStore");

                await _storeService.UpdateStoreAsync(store);

                return Ok(SuccessResponseBody(store, Localizer["Store successfully updated"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
