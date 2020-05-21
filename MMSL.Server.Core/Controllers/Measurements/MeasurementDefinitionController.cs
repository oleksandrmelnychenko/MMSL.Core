using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.MeasurementDefinitions;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
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

  
    }
}
