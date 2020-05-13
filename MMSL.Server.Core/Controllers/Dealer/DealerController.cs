﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.Entities.Dealer;
using MMSL.Services.DealerServices.Contracts;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Dealer {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Dealer)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Dealer)]
    public class DealerController : WebApiControllerBase {

        private readonly IDealerAccountService _dealerAccountService;

        public DealerController(IDealerAccountService dealerAccountService,
             IResponseFactory responseFactory,
             IStringLocalizer<DealerController> localizer) : base(responseFactory, localizer) {
            _dealerAccountService = dealerAccountService;
        }

        [HttpGet]
        [AssignActionRoute(DealerSegments.GET_ALL_DEALERS)]
        public async Task<IActionResult> GetAllDealers(
            [FromQuery]int pageNumber,
            [FromQuery]int limit,
            [FromQuery]string searchPhrase
            ) {
            try {
                return Ok(SuccessResponseBody(await _dealerAccountService.GetDealerAccounts(pageNumber, limit, searchPhrase), Localizer["All dealers"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(DealerSegments.GET_DEALER)]
        public async Task<IActionResult> GetDealer([FromQuery]long dealerAccountId) {
            try {
                return Ok(SuccessResponseBody(await _dealerAccountService.GetDealerAccount(dealerAccountId), Localizer["Dealers account"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(DealerSegments.ADD_DEALER)]
        public async Task<IActionResult> AddDealerAccount([FromBody] DealerAccount dealerAccount) {
            try {
                if (dealerAccount == null) throw new ArgumentNullException("dealerAccount");

                return Ok(SuccessResponseBody(await _dealerAccountService.AddDealerAccount(dealerAccount), Localizer["Dealer account successfully created"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(DealerSegments.DELETE_DEALER)]
        public async Task<IActionResult> DeleteDealerAccount([FromQuery]long dealerAccountId) {
            try {
                await _dealerAccountService.DeleteDealerAccount(dealerAccountId);

                return Ok(SuccessResponseBody(dealerAccountId, Localizer["Dealer account successfully deleted"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(DealerSegments.UPDATE_DEALER)]
        public async Task<IActionResult> UpdateDealerAccount([FromBody]DealerAccount dealerAccount) {
            try {
                await _dealerAccountService.UpdateDealerAccount(dealerAccount);

                return Ok(SuccessResponseBody(dealerAccount, Localizer["Dealer account successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}