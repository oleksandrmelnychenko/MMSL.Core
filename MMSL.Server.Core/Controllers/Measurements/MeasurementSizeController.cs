using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Services.MeasurementServices.Contracts;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Measurements {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementSizes)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementSizes)]
    public class MeasurementSizeController : WebApiControllerBase {
        private readonly IMeasurementSizeService _measurementSizeService;

        public MeasurementSizeController(
            IMeasurementSizeService measurementSizeService,
            IResponseFactory responseFactory,
            IStringLocalizer<MeasurementSizeController> localizer)
            : base(responseFactory, localizer) {
            _measurementSizeService = measurementSizeService;
        }

        public async Task<IActionResult> GetSize([FromQuery] long measurementId) {
            try {
                return Ok(SuccessResponseBody(await _measurementSizeService.GetMeasurementSizes(measurementId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        public async Task<IActionResult> GetSize([FromQuery] long measurementId) {
            try {
                return Ok(SuccessResponseBody(await _measurementSizeService.GetMeasurementSizes(measurementId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
