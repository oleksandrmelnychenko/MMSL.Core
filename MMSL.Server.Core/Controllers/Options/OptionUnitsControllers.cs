using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Options;
using MMSL.Domain.Entities.Options;
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

        [HttpGet]
        [AssignActionRoute(OptionUnitSegments.GET_GROUP_OPTION_UNITS)]
        public async Task<IActionResult> GetGroupOptionUnit([FromQuery]long optionGroupId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.GetOptionUnitsByGroupIdAsync(optionGroupId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(OptionUnitSegments.ADD_OPTION_UNIT)]
        public async Task<IActionResult> AddOptionUnit([FromBody]OptionUnit optionUnit) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.AddOptionUnit(optionUnit), Localizer["Successfully created"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(OptionUnitSegments.UPDATE_OPTION_UNIT)]
        public async Task<IActionResult> UpdateOptionUnit([FromBody]OptionUnit optionUnit) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.UpdateOptionUnit(optionUnit), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(OptionUnitSegments.DELETE_OPTION_UNIT)]
        public async Task<IActionResult> DeleteOptionUnit([FromQuery]long optionUnitId) {
            try {
                return Ok(SuccessResponseBody(await _optionUnitService.DeleteOptionUnit(optionUnitId), Localizer["Successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
