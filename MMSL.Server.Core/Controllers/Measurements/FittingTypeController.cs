using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Measurements;
using MMSL.Domain.DataContracts.FittingTypes;
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

        [HttpGet]
        [Authorize]
        [AssignActionRoute(FittingTypeSegments.GET_FITTING_TYPE)]
        public async Task<IActionResult> GetFittingType([FromQuery]long fittingTypeId) {
            try {
                FittingType fittingType = await _fittingTypeService.GetFittingTypeByIdAsync(fittingTypeId);

                return Ok(SuccessResponseBody(fittingType, Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(FittingTypeSegments.ADD_FITTING_TYPE)]
        public async Task<IActionResult> AddFittingType([FromBody] FittingTypeDataContract fittingTypeDataContract) {
            try {
                if (fittingTypeDataContract == null || string.IsNullOrEmpty(fittingTypeDataContract.Type)) {
                    throw new ArgumentNullException("FittingTypeDataContract");
                }

                return Ok(SuccessResponseBody(await _fittingTypeService.AddFittingTypeAsync(fittingTypeDataContract), Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(FittingTypeSegments.UPDATE_FITTING_TYPE)]
        public async Task<IActionResult> UpdateSize([FromBody] FittingType fittingType) {
            try {
                if (fittingType == null) throw new ArgumentNullException("FittingType");

                return Ok(SuccessResponseBody(await _fittingTypeService.UpdateFittingTypeAsync(fittingType), Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(FittingTypeSegments.DELETE_FITTING_TYPE)]
        public async Task<IActionResult> DeleteStore([FromQuery]long fittingTypeId) {
            try {
                await _fittingTypeService.DeleteFittingTypeAsync(fittingTypeId);

                return Ok(SuccessResponseBody(fittingTypeId, Localizer["FittingType successfully deleted"]));
            }
            catch (NotFoundValueException exc) {
                Log.Error(exc.GetUserMessageException);
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
