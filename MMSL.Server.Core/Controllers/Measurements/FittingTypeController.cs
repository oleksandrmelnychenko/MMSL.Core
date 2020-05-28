using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
using MMSL.Domain.Entities.Measurements;
using MMSL.Services.MeasurementServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Measurements {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.FittingTypes)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.FittingTypes)]
    public class FittingTypeController : WebApiControllerBase {

        private readonly IFittingTypeService _fittingTypeService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="fittingTypeService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public FittingTypeController(
           IFittingTypeService fittingTypeService,
           IResponseFactory responseFactory,
           IStringLocalizer<FittingTypeController> localizer)
           : base(responseFactory, localizer) {
            _fittingTypeService = fittingTypeService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(FittingTypeSegments.GET_FITTING_TYPES)]
        public async Task<IActionResult> GetAll([FromQuery]string searchPhrase) {
            try {
                List<FittingType> fittingTypes = await _fittingTypeService.GetFittingTypesAsync(searchPhrase);

                return Ok(SuccessResponseBody(fittingTypes, Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
