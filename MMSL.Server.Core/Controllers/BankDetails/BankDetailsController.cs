using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.BankDetails;
using MMSL.Services.BankDetailsServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.BankDetails {
    //[AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.BankDetails)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.BankDetails)]
    public class BankDetailsController : WebApiControllerBase {

        private readonly IBankDetailsService _bankDetailsService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="bankDetailsService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public BankDetailsController(
            IBankDetailsService bankDetailsService,
            IResponseFactory responseFactory,
            IStringLocalizer<BankDetailsController> localizer) : base(responseFactory, localizer) {

            _bankDetailsService = bankDetailsService;
        }

        [HttpGet]
        [AllowAnonymous]
        [AssignActionRoute(BankDetailsSegments.GET_BANK_DETAILS)]
        public async Task<IActionResult> GetAll() {
            try {
                List<BankDetail> bankDetails = await _bankDetailsService.GetAllBankDetailsAsync();

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
        [AssignActionRoute(BankDetailsSegments.NEW_BANK_DETAIL)]
        public async Task<IActionResult> NewBankDetail([FromBody] NewBankDetailDataContract newBankDetailDataContract) {
            try {
                if (newBankDetailDataContract == null) throw new ArgumentNullException("NewUserDataContract");

                BankDetail bankDetail = await _bankDetailsService.NewBankDetail(newBankDetailDataContract);

                return Ok(SuccessResponseBody(bankDetail, Localizer["New bankDetail has been created successfully"]));
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
