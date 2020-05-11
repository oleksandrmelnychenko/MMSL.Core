using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Identity;
using MMSL.Services.IdentityServices.Contracts;
using Serilog;

namespace MMSL.Server.Core.Controllers.Identity {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.UserIdentity)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.UserIdentity)]
    public class IdentityController : WebApiControllerBase {
        private readonly IUserIdentityService _userIdentityService;

        public IdentityController(IUserIdentityService userIdentityService,
             IResponseFactory responseFactory,
             IStringLocalizer<IdentityController> localizer) : base(responseFactory, localizer) {
            _userIdentityService = userIdentityService;
        }

        [Authorize]
        [HttpGet]
        [AssignActionRoute(IdentitySegments.VALIDATE_TOKEN)]
        public async Task<IActionResult> ValidateToken() {
            try {
                UserAccount user = await _userIdentityService.ValidateToken(User);
                return Ok(SuccessResponseBody(user, Localizer["Token validated successfully"]));
            } catch (InvalidIdentityException exc) {
                return Unauthorized(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.Unauthorized, exc.Body));
            } catch (Exception exc) {
                Log.Error(exc.Message); 
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [AssignActionRoute(IdentitySegments.NEW_ACCOUNT)]
        public async Task<IActionResult> NewUser([FromBody] NewUserDataContract newUserDataContract) {
            try {
                if (newUserDataContract == null) throw new ArgumentNullException("NewUserDataContract");

                UserAccount user = await _userIdentityService.NewUser(newUserDataContract);

                return Ok(SuccessResponseBody(user, Localizer["New user has been created successfully"]));
            } catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            } catch (Exception exc) {
                Log.Error(exc.Message); 
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [AssignActionRoute(IdentitySegments.SIGN_IN)]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationDataContract authenticateDataContract) {
            try {
                if (authenticateDataContract == null) throw new ArgumentNullException("AuthenticationDataContract");

                UserAccount user = await _userIdentityService.SignInAsync(authenticateDataContract);

                return Ok(SuccessResponseBody(user, Localizer["User logged in successfully"]));
            } catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
