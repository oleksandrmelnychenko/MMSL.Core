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

        private readonly IStoreService _bankDetailsService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="bankDetailsService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public StoreController(
            IStoreService bankDetailsService,
            IResponseFactory responseFactory,
            IStringLocalizer<StoreController> localizer) : base(responseFactory, localizer) {

            _bankDetailsService = bankDetailsService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(StoreSegments.GET_STORES)]
        public async Task<IActionResult> GetAll() {
            try {
                List<Store> bankDetails = await _bankDetailsService.GetAllStoresAsync();

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
        [AllowAnonymous]
        [AssignActionRoute(StoreSegments.NEW_STORE)]
        public async Task<IActionResult> NewBankDetail([FromBody] NewStoreDataContract newStoreDataContract) {
            try {
                if (newStoreDataContract == null) throw new ArgumentNullException("NewUserDataContract");

                Store store = await _bankDetailsService.NewStore(newStoreDataContract);

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
    }
}
