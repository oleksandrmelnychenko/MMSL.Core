﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common;
using MMSL.Common.Exceptions.DealerExceptions;
using MMSL.Common.Helpers;
using MMSL.Common.IdentityConfiguration;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.Dealers;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Entities.Identity;
using MMSL.Services.DealerServices.Contracts;
using MMSL.Services.IdentityServices.Contracts;
using Serilog;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Dealer {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Dealer)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Dealer)]
    public class DealerController : WebApiControllerBase {

        private readonly IDealerAccountService _dealerAccountService;
        private readonly IUserIdentityService _userIdentityService;

        public DealerController(IDealerAccountService dealerAccountService,
             IUserIdentityService userIdentityService,
             IResponseFactory responseFactory,
             IStringLocalizer<DealerController> localizer) : base(responseFactory, localizer) {
            _dealerAccountService = dealerAccountService;
            _userIdentityService = userIdentityService;
        }

        [HttpGet]
        [AssignActionRoute(DealerSegments.GET_ALL_DEALERS)]
        public async Task<IActionResult> GetAllDealers(
            [FromQuery] int pageNumber,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int limit,
            [FromQuery] string searchPhrase
            ) {
            try {
                return Ok(SuccessResponseBody(await _dealerAccountService.GetDealerAccounts(pageNumber, limit, searchPhrase, from, to), Localizer["All dealers"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(DealerSegments.GET_DEALERS_BY_PERMISSION)]
        public async Task<IActionResult> GetDealersByPermission([FromQuery] long productPermissionId) {
            try {
                return Ok(SuccessResponseBody(await _dealerAccountService.GetDealerAccountsByPermissionSetting(productPermissionId), Localizer["All dealers"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(DealerSegments.SEARCH_DEALER)]
        public async Task<IActionResult> SearchDealers(
            [FromQuery] string searchPhrase,
            [FromQuery] long? productId,
            [FromQuery] bool? excludeMatchPermission) {
            try {
                return Ok(SuccessResponseBody(
                    await _dealerAccountService.SearchDealerAccountsByPermissionSetting(
                        searchPhrase,
                        productId,
                        excludeMatchPermission),
                    Localizer["Dealers found"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(DealerSegments.GET_DEALER)]
        public async Task<IActionResult> GetDealer([FromQuery] long dealerAccountId) {
            try {
                return Ok(SuccessResponseBody(await _dealerAccountService.GetDealerAccount(dealerAccountId), Localizer["Dealers account"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manufacturer")]
        [AssignActionRoute(DealerSegments.ADD_DEALER)] 
        public async Task<IActionResult> AddDealerAccount([FromBody] NewDealerAccountDataContract dealerAccountDataContract) {
            try {
                if (dealerAccountDataContract == null) throw new ArgumentNullException("NewDealerAccountDataContract");

                if (string.IsNullOrEmpty(dealerAccountDataContract.Password)) throw new ArgumentNullException("Password");

                DealerAccount dealerAccount = dealerAccountDataContract.GetEntity();

                UserAccount dealerIdentity = await _userIdentityService.NewUser(
                    new NewUserDataContract {
                        Email = dealerAccount.Email,
                        Password = dealerAccountDataContract.Password,
                        PasswordExpiresAt = DateTime.Now.AddDays(7),
                        ForceChangePassword = true,
                        Roles = new System.Collections.Generic.List<RoleType> { RoleType.Dealer }
                    });

                dealerAccount.UserIdentityId = dealerIdentity.Id;

                //TODO: set default permissions to this dealer per product


                //TODO: send email to dealer with login info

                return Ok(SuccessResponseBody(await _dealerAccountService.AddDealerAccount(dealerAccount), Localizer["Dealer account successfully created"]));
            } catch (InvalidDealerModelException dealerExc) {
                return BadRequest(ErrorResponseBody(dealerExc.GetUserMessageException, HttpStatusCode.BadRequest));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(DealerSegments.DELETE_DEALER)]
        public async Task<IActionResult> DeleteDealerAccount([FromQuery] long dealerAccountId) {
            try {
                await _dealerAccountService.DeleteDealerAccount(dealerAccountId);

                return Ok(SuccessResponseBody(dealerAccountId, Localizer["Dealer account successfully deleted"]));
            } catch (DealerNotFoundException dealerExc) {
                return BadRequest(ErrorResponseBody(dealerExc.Message, HttpStatusCode.NotFound));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(DealerSegments.UPDATE_DEALER)]
        public async Task<IActionResult> UpdateDealerAccount([FromBody] DealerAccount dealerAccount) {
            try {
                if (dealerAccount == null) throw new ArgumentNullException("dealerAccount");

                await _dealerAccountService.UpdateDealerAccount(dealerAccount);

                return Ok(SuccessResponseBody(await _dealerAccountService.GetDealerAccount(dealerAccount.Id), Localizer["Dealer account successfully updated"]));
            } catch (InvalidDealerModelException dealerExc) {
                return BadRequest(ErrorResponseBody(dealerExc.GetUserMessageException, HttpStatusCode.BadRequest));
            } catch (DealerNotFoundException notFound) {
                return BadRequest(ErrorResponseBody(notFound.GetUserMessageException, HttpStatusCode.BadRequest));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
