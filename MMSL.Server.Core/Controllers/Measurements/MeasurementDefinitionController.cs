using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using MMSL.Services.MeasurementServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Measurements {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementDefinitions)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementDefinitions)]
    public class MeasurementDefinitionController : WebApiControllerBase {

        private readonly IMeasurementDefinitionService _measurementDefinitionService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="measurementDefinitionService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public MeasurementDefinitionController(
            IMeasurementDefinitionService measurementDefinitionService,
            IResponseFactory responseFactory,
            IStringLocalizer<MeasurementDefinitionController> localizer)
            : base(responseFactory, localizer) {

            _measurementDefinitionService = measurementDefinitionService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(MeasurementDefinitionSegments.GET_MEASUREMENT_DEFINITIONS)]
        public async Task<IActionResult> GetAll(
            [FromQuery]string searchPhrase ,
            [FromQuery]bool? isDefault) {
            try {
                List<MeasurementDefinition> measurementDefinitions = await _measurementDefinitionService.GetMeasurementDefinitionsAsync(searchPhrase, isDefault);

                return Ok(SuccessResponseBody(measurementDefinitions, Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(MeasurementDefinitionSegments.NEW_MEASUREMENT_DEFINITION)]
        public async Task<IActionResult> NewMeasurementDefinition([FromBody] NewMeasurementDefinitionDataContract newMeasurementDefinitionDataContract) {
            try {
                if (newMeasurementDefinitionDataContract == null) throw new ArgumentNullException("MeasurementDefinitionDataContract");

                MeasurementDefinition measurementDefinition = await _measurementDefinitionService.NewMeasurementDefinitionAsync(newMeasurementDefinitionDataContract);

                return Ok(SuccessResponseBody(measurementDefinition, Localizer["New MeasurementDefinition has been created successfully"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(MeasurementDefinitionSegments.UPDATE_MEASUREMENT_DEFINITION)]
        public async Task<IActionResult> UpdateMeasurementDefinition([FromBody]MeasurementDefinition measurementDefinition) {
            try {
                if (measurementDefinition == null) throw new ArgumentNullException("UpdateMeasurementDefinition");

                await _measurementDefinitionService.UpdateMeasurementDefinitionAsync(measurementDefinition);

                return Ok(SuccessResponseBody(measurementDefinition, Localizer["MeasurementDefinition successfully updated"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(MeasurementDefinitionSegments.DELETE_MEASUREMENT_DEFINITION)]
        public async Task<IActionResult> DeleteMeasurementDefinition([FromQuery]long measurementDefinitionId) {
            try {
                if (measurementDefinitionId == default(long)) throw new ArgumentNullException("DeleteMeasurementDefinition");

                await _measurementDefinitionService.DeleteMeasurementDefinitionAsync(measurementDefinitionId);

                return Ok(SuccessResponseBody(measurementDefinitionId, Localizer["MeasurementDefinition successfully deleted"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
