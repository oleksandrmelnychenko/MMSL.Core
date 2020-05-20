using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
using MMSL.Domain.Entities.Measurements;
using MMSL.Services.MeasurementServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Measurements {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Measurements)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Measurements)]
    public class MeasurementController : WebApiControllerBase {

        private readonly IMeasurementService _measurementService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="measurementService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public MeasurementController(
            IMeasurementService measurementService,
            IResponseFactory responseFactory,
            IStringLocalizer<MeasurementController> localizer)
            : base(responseFactory, localizer) {

            _measurementService = measurementService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(MeasurementSegments.GET_MEASUREMENTS)]
        public async Task<IActionResult> GetAll([FromQuery]string searchPhrase) {
            try {
                List<Measurement> measurements = await _measurementService.GetMeasurementsAsync(searchPhrase);

                return Ok(SuccessResponseBody(measurements, Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
