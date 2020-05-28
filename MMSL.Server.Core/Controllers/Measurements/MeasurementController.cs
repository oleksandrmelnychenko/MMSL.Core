using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
using MMSL.Domain.DataContracts;
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
                return Ok(SuccessResponseBody(await _measurementService.GetMeasurementsAsync(searchPhrase), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(MeasurementSegments.GET_MEASUREMENT)]
        public async Task<IActionResult> GetSingle([FromQuery]long measurementId) {
            try {
                return Ok(SuccessResponseBody(await _measurementService.GetMeasurementDetailsAsync(measurementId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(MeasurementSegments.GET_MEASUREMENT_CHART)]
        public async Task<IActionResult> GetChart([FromQuery]long measurementId) {
            try {
                return Ok(SuccessResponseBody(await _measurementService.GetMeasurementChartAsync(measurementId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(MeasurementSegments.NEW_MEASUREMENT)]
        public async Task<IActionResult> NewMeasurement([FromBody] NewMeasurementDataContract newMeasurementDataContract) {
            try {
                if (newMeasurementDataContract == null) throw new ArgumentNullException(nameof(newMeasurementDataContract));

                if (string.IsNullOrEmpty(newMeasurementDataContract.Name)) throw new ArgumentNullException(nameof(newMeasurementDataContract.Name));

                return Ok(SuccessResponseBody(await _measurementService.NewMeasurementAsync(newMeasurementDataContract), Localizer["New Measurement has been created successfully"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(MeasurementSegments.UPDATE_MEASUREMENT)]
        public async Task<IActionResult> UpdateMeasurement([FromBody]UpdateMeasurementDataContract measurement) {
            try {
                if (measurement == null) throw new ArgumentNullException("UpdateMeasurement");

                var updated = await _measurementService.UpdateMeasurementAsync(measurement);

                return Ok(SuccessResponseBody(updated, Localizer["Measurement successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(MeasurementSegments.DELETE_MEASUREMENT)]
        public async Task<IActionResult> DeleteMeasurement([FromQuery]long measurementId) {
            try {
                if (measurementId == default(long)) throw new ArgumentNullException("DeleteMeasurement");

                await _measurementService.DeleteMeasurementAsync(measurementId);

                return Ok(SuccessResponseBody(measurementId, Localizer["Measurement successfully deleted"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
