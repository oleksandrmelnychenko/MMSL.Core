using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.Entities.Measurements;
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

        [HttpGet]
        [AssignActionRoute(MeasurementSizeSegments.GET_MEASUREMENT_SIZES)]
        public async Task<IActionResult> GetSizes([FromQuery] long measurementId) {
            try {
                return Ok(SuccessResponseBody(await _measurementSizeService.GetMeasurementSizesAsync(measurementId), Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(MeasurementSizeSegments.ADD_MEASUREMENT_SIZE)]
        public async Task<IActionResult> AddSize([FromBody] MeasurementSizeDataContract measurementSizeDataContract) {
            try {
                if (measurementSizeDataContract == null || string.IsNullOrEmpty(measurementSizeDataContract.Name)) {
                    throw new ArgumentNullException("MeasurementSizeDataContract");
                }

                return Ok(SuccessResponseBody(await _measurementSizeService.AddMeasurementSizeAsync(measurementSizeDataContract), Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(MeasurementSizeSegments.UPDATE_MEASUREMENT_SIZE)]
        public async Task<IActionResult> UpdateSize([FromBody] MeasurementSize measurementSize) {
            try {
                if (measurementSize == null) {
                    throw new ArgumentNullException("MeasurementSize");
                }

                return Ok(SuccessResponseBody(await _measurementSizeService.UpdateMeasurementSizeAsync(measurementSize), Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
