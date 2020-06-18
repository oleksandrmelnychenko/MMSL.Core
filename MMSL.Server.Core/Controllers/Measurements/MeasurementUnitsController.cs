using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
using MMSL.Services.MeasurementServices.Contracts;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Measurements {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementUnits)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementUnits)]
    public class MeasurementUnitsController : WebApiControllerBase {

        private readonly IMeasurementUnitService _measurementUnitService;

        public MeasurementUnitsController(
            IMeasurementUnitService measurementUnitService,
            IResponseFactory responseFactory,
            IStringLocalizer<MeasurementUnitsController> localizer)
            : base(responseFactory, localizer) {
            _measurementUnitService = measurementUnitService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(MeasurementUnitSegments.GET_MEASUREMENT_UNITS)]
        public async Task<IActionResult> GetAll() {
            try {
                return Ok(SuccessResponseBody(await _measurementUnitService.GetAll(), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
