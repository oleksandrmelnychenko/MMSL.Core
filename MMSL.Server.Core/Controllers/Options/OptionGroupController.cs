using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Options;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Entities.Stores;
using MMSL.Services.OptionServices.Contracts;
using MMSL.Services.StoreServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Options {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Options)]
    public class OptionGroupController : WebApiControllerBase {

        private readonly IOptionGroupService _optionGroupService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public OptionGroupController(
            IOptionGroupService optionGroupService,
            IResponseFactory responseFactory,
            IStringLocalizer<OptionGroupController> localizer) : base(responseFactory, localizer) {

            _optionGroupService = optionGroupService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(OptionGroupSegments.GET_OPTION_GROUPS)]
        public async Task<IActionResult> GetAll() {
            try {
                List<OptionGroup> optionGroups = await _optionGroupService.GetOptionGroupsAsync();

                return Ok(SuccessResponseBody(optionGroups, Localizer["Successfully completed"]));
            }
            catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
