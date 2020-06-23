using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
using MMSL.Domain.DataContracts.Products;
using MMSL.Services.ProductCategories.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.ProductCategories {
    [Authorize(Roles = "Administrator,Manufacturer")]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductAvailability)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductAvailability)]
    public class DealerProductAvailabilityController : WebApiControllerBase {

        private IDealerProductAvailabilityService _dealerProductAvailabilityService;

        public DealerProductAvailabilityController(
            IDealerProductAvailabilityService dealerProductAvailabilityService,
            IResponseFactory responseFactory,
            IStringLocalizer<DealerProductAvailabilityController> localizer)
            : base(responseFactory, localizer) {

            _dealerProductAvailabilityService = dealerProductAvailabilityService;
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(ProductAvailabilitySegments.SaveProductValailabilities)]
        public async Task<IActionResult> SaveAvailabilitySettings([FromBody] IEnumerable<ProductAvailabilitiesDataContract> settings) {
            try {
                await _dealerProductAvailabilityService.SaveProductAvailabilitySettings(settings);

                return Ok(SuccessResponseBody(null, Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
