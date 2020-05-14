using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Options;
using MMSL.Services.OptionServices.Contracts;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Options {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    public class OptionUnitsControllers : WebApiControllerBase {

        private IOptionUnitService _optionUnitService;

        public OptionUnitsControllers(IOptionUnitService optionUnitService, IResponseFactory responseFactory, IStringLocalizer<OptionUnitsControllers> localizer)
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
    }
}
