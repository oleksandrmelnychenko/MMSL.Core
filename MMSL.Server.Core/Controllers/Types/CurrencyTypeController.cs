using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Types;
using MMSL.Domain.Entities.CurrencyTypes;
using MMSL.Services.Types.Contracts;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Types {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.CurrencyTypes)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.CurrencyTypes)]
    public class CurrencyTypeController : WebApiControllerBase {

        private readonly ICurrencyTypeService _currencyTypeService;

        public CurrencyTypeController(ICurrencyTypeService currencyTypeService, IResponseFactory responseFactory, IStringLocalizer<CurrencyTypeController> localizer)
            : base(responseFactory, localizer) {
            _currencyTypeService = currencyTypeService;
        }

        [HttpGet]
        [AssignActionRoute(CurrencyTypeSegments.GET_CURRENCY_TYPES)]
        public async Task<IActionResult> Get() {
            try {
                return Ok(SuccessResponseBody(await _currencyTypeService.GetCurrencyTypesAsync(), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(CurrencyTypeSegments.ADD_CURRENCY_TYPE)]
        public async Task<IActionResult> AddOptionUnit([FromBody]CurrencyType currencyType) {
            try {
                return Ok(SuccessResponseBody(await _currencyTypeService.AddCurrencyTypeAsync(currencyType), Localizer["Successfully created"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(CurrencyTypeSegments.UPDATE_CURRENCY_TYPE)]
        public async Task<IActionResult> UpdateOptionUnit([FromBody]CurrencyType currencyType) {
            try {
                return Ok(SuccessResponseBody(await _currencyTypeService.UpdateCurrencyTypeAsync(currencyType), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(CurrencyTypeSegments.DELETE_CURRENCY_TYPE)]
        public async Task<IActionResult> DeleteOptionUnit([FromQuery]long currencyTypeId) {
            try {
                return Ok(SuccessResponseBody(await _currencyTypeService.DeleteCurrencyTypeAsync(currencyTypeId), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
