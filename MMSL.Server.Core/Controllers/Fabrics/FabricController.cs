using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.Fabrics;
using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Server.Core.Helpers;
using MMSL.Services.FabricServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Fabrics {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]

    public class FabricController : WebApiControllerBase {

        private readonly IFabricService _fabricService;

        public FabricController(IFabricService fabricService, IResponseFactory responseFactory, IStringLocalizer<FabricController> localizer)
            : base(responseFactory, localizer) {
            _fabricService = fabricService;
        }

        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_ALL)]

        public IActionResult GetAll() {
            try {
                List<Fabric> fabrics = new List<Fabric>();
                //TODO: get all with filters
                return Ok(SuccessResponseBody(null, Localizer["All fabrics"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

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


        [HttpPost]
        [AssignActionRoute(FabricSegments.ADD_FABRIC)]

        public async Task<IActionResult> Add([FromForm] NewFabricDataContract fabric, [FromForm] FileFormData fabricImage) {
            try {
                string imageUrl = string.Empty;

                if (fabricImage.File != null) {
                    imageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", fabricImage.File);
                }

                return Ok(SuccessResponseBody(await _fabricService.AddFabric(fabric, imageUrl), Localizer["Successful"]));
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
                return Ok(SuccessResponseBody(await _fabricService.UpdateFabricVisibilities(fabric), Localizer["Successful"]));
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
