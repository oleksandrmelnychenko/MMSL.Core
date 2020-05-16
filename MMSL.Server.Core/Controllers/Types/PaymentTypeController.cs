using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Types;
using MMSL.Domain.DataContracts.Types;
using MMSL.Domain.Entities.PaymentTypes;
using MMSL.Services.Types.Contracts;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Types {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.PaymentTypes)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.PaymentTypes)]
    public class PaymentTypeController : WebApiControllerBase {

        private readonly IPaymentTypeService _paymentTypeService;

        public PaymentTypeController(IPaymentTypeService paymentTypeService, IResponseFactory responseFactory, IStringLocalizer<PaymentTypeController> localizer)
            : base(responseFactory, localizer) {
            _paymentTypeService = paymentTypeService;
        }

        [HttpGet]
        [AssignActionRoute(PaymentTypeSegments.GET_PAYMENT_TYPES)]
        public async Task<IActionResult> Get() {
            try {
                return Ok(SuccessResponseBody(await _paymentTypeService.GetPaymentTypesAsync(), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(PaymentTypeSegments.ADD_PAYMENT_TYPE)]
        public async Task<IActionResult> Add([FromBody]PaymentTypeDataContract paymentType) {
            try {
                return Ok(SuccessResponseBody(await _paymentTypeService.AddPaymentTypeAsync(paymentType), Localizer["Successfully created"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(PaymentTypeSegments.UPDATE_PAYMENT_TYPE)]
        public async Task<IActionResult> Update([FromBody]PaymentTypeDataContract paymentType) {
            try {
                return Ok(SuccessResponseBody(await _paymentTypeService.UpdatePaymentTypeAsync(paymentType), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(PaymentTypeSegments.DELETE_PAYMENT_TYPE)]
        public async Task<IActionResult> Delete([FromQuery]long paymentTypeId) {
            try {
                return Ok(SuccessResponseBody(await _paymentTypeService.DeletePaymentTypeAsync(paymentTypeId), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
