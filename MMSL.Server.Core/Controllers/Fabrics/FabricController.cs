using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common;
using MMSL.Common.Helpers;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Fabrics;
using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.DataContracts.Filters;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.EntityHelpers;
using MMSL.Server.Core.Helpers;
using MMSL.Services.FabricServices.Contracts;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Fabrics {
    [Authorize(Roles = "Administrator,Manufacturer")]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]

    public class FabricController : WebApiControllerBase {

        private readonly IFabricService _fabricService;

        public FabricController(IFabricService fabricService, IResponseFactory responseFactory, IStringLocalizer<FabricController> localizer)
            : base(responseFactory, localizer) {
            _fabricService = fabricService;
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_ALL)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber,
            [FromQuery] int limit,
            [FromQuery] string searchPhrase,
            [FromQuery] string filterBuilder
            ) {
            try {
                FilterItem[] filters = !string.IsNullOrEmpty(filterBuilder) 
                    ? JsonConvert.DeserializeObject<FilterItem[]>(filterBuilder)
                    : null;

                PaginatingResult<Fabric> fabrics = await _fabricService.GetFabrics(pageNumber, limit, searchPhrase, filters);
                return Ok(SuccessResponseBody(fabrics, Localizer["All fabrics"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_PDF)]
        public async Task<IActionResult> GetFabricsPdf([FromQuery] string searchPhrase, [FromQuery] string filterBuilder) {
            try {
                FilterItem[] filters = !string.IsNullOrEmpty(filterBuilder) 
                    ? JsonConvert.DeserializeObject<FilterItem[]>(filterBuilder)
                    : null;

                string downloadUrl = await _fabricService.PrepareFabricsPdf(searchPhrase, filters);
                return Ok(SuccessResponseBody(downloadUrl, Localizer["Download link"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET)]
        public async Task<IActionResult> GetById([FromQuery] long fabricId) {
            try {
                return Ok(SuccessResponseBody(await _fabricService.GetByIdAsync(fabricId), Localizer["Successful"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_FILTERS)]
        public async Task<IActionResult> GetFilters() {
            try {
                return Ok(SuccessResponseBody(await _fabricService.GetFabricFilters(), Localizer["Successful"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(FabricSegments.ADD_FABRIC)]
        public async Task<IActionResult> Add([FromForm] NewFabricDataContract fabric, [FromForm] FileFormData fabricImage) {
            try {
                long identityId = ClaimHelper.GetUserId(User);

                string imageUrl = string.Empty;

                if (fabricImage.File != null) {
                    imageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", fabricImage.File);
                }

                return Ok(SuccessResponseBody(await _fabricService.AddFabric(fabric, identityId, imageUrl), Localizer["Successful"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(FabricSegments.UPDATE_FABRIC)]

        public async Task<IActionResult> Update([FromForm] UpdateFabricDataContract fabric, [FromForm] FileFormData fabricImage) {
            try {
                string oldImageUrl = fabric.ImageUrl;
                string newImageUrl = string.Empty;

                if (fabricImage.File != null) {
                    newImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", fabricImage.File);
                }

                Fabric updated = await _fabricService.UpdateFabric(fabric, newImageUrl);

                if (!string.IsNullOrEmpty(oldImageUrl)) {
                    FileUploadingHelper.DeleteFile($"{Request.Scheme}://{Request.Host}", oldImageUrl);
                }

                return Ok(SuccessResponseBody(updated, Localizer["Successful"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [AssignActionRoute(FabricSegments.UPDATE_FABRIC_VISIBILITIES)]

        public async Task<IActionResult> UpdateFabricVisibilities([FromBody] UpdateFabricVisibilitiesDataContract fabric) {
            try {
                await _fabricService.UpdateFabricVisibilities(fabric, ClaimHelper.GetUserId(User));
                return Ok(SuccessResponseBody(null, Localizer["Successful"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(FabricSegments.DELETE_FABRIC)]

        public async Task<IActionResult> Delete([FromQuery] long fabricId) {
            try {
                //TODO: delete entity
                Fabric deleted = await _fabricService.DeleteFabric(fabricId);
                return Ok(SuccessResponseBody(deleted, Localizer["Successful"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
