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
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.EntityHelpers;
using MMSL.Server.Core.Helpers;
using MMSL.Server.Core.Import;
using MMSL.Services.FabricServices.Contracts;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Fabrics
{
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]

    public class FabricController : WebApiControllerBase
    {
        private readonly IFabricService _fabricService;

        private readonly ExcelParser _excelParser;

        public FabricController(IFabricService fabricService, ExcelParser excelParser, IResponseFactory responseFactory, IStringLocalizer<FabricController> localizer)
            : base(responseFactory, localizer)
        {
            _fabricService = fabricService;
            _excelParser = excelParser;
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_ALL)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber,
            [FromQuery] int limit,
            [FromQuery] string searchPhrase,
            [FromQuery] string filterBuilder
            )
        {
            try
            {
                FilterItem[] filters = !string.IsNullOrEmpty(filterBuilder)
                    ? JsonConvert.DeserializeObject<FilterItem[]>(filterBuilder)
                    : null;

                long? manufacturerUserIdentity = ClaimHelper.GetUserRoles(User).All(x => x != RoleType.Dealer.ToString())
                    ? (long?)ClaimHelper.GetUserId(User)
                    : null;

                PaginatingResult<Fabric> fabrics = await _fabricService.GetFabrics(pageNumber, limit, searchPhrase, filters, manufacturerUserIdentity);
                return Ok(SuccessResponseBody(fabrics, Localizer["All fabrics"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_PDF)]
        public async Task<IActionResult> GetFabricsPdf([FromQuery] string searchPhrase, [FromQuery] string filterBuilder)
        {
            try
            {
                FilterItem[] filters = !string.IsNullOrEmpty(filterBuilder)
                    ? JsonConvert.DeserializeObject<FilterItem[]>(filterBuilder)
                    : null;

                long? manufacturerUserIdentity = ClaimHelper.GetUserRoles(User).All(x => x != RoleType.Dealer.ToString())
                    ? (long?)ClaimHelper.GetUserId(User)
                    : null;

                string downloadPath = await _fabricService.PrepareFabricsPdf(searchPhrase, filters, manufacturerUserIdentity);
                return Ok(SuccessResponseBody(FileUploadingHelper.FullPathToWebPath($"{Request.Scheme}://{Request.Host}", downloadPath), Localizer["Download link"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET)]
        public async Task<IActionResult> GetById([FromQuery] long fabricId)
        {
            try
            {
                return Ok(SuccessResponseBody(await _fabricService.GetByIdAsync(fabricId), Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer,Dealer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_FILTERS)]
        public async Task<IActionResult> GetFilters()
        {
            try
            {
                long? manufacturerUserIdentity = ClaimHelper.GetUserRoles(User).All(x => x != RoleType.Dealer.ToString())
                    ? (long?)ClaimHelper.GetUserId(User)
                    : null;

                return Ok(SuccessResponseBody(await _fabricService.GetFabricFilters(manufacturerUserIdentity), Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer")]
        [HttpPost]
        [AssignActionRoute(FabricSegments.ADD_FABRIC)]
        public async Task<IActionResult> Add([FromForm] NewFabricDataContract fabric, [FromForm] FileFormData fabricImage)
        {
            try
            {
                long identityId = ClaimHelper.GetUserId(User);

                string imageUrl = string.Empty;

                if (fabricImage.File != null)
                {
                    imageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", fabricImage.File);
                }

                return Ok(SuccessResponseBody(await _fabricService.AddFabric(fabric, identityId, imageUrl), Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }


        [Authorize(Roles = "Administrator,Manufacturer")]
        [HttpPost]
        [AssignActionRoute(FabricSegments.IMPORT_FABRICS)]
        public async Task<IActionResult> Import([FromForm] FileFormData import)
        {
            try
            {
                long identityId = ClaimHelper.GetUserId(User);

                string tempPath = string.Empty;

                if (import.File != null)
                {
                    tempPath = await FileUploadingHelper.UploadTempFile(import.File);
                }

                if (string.IsNullOrEmpty(tempPath))
                    return BadRequest(ErrorResponseBody(Localizer["Can not upload file"], HttpStatusCode.BadRequest));

                List<Fabric> fabricsResult = _excelParser.ParseFabricExcel(tempPath);

                IEnumerable<Fabric> insertingResult = await _fabricService.AddFabrics(fabricsResult, identityId);

                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }

                return Ok(SuccessResponseBody(new ImportResult(insertingResult.Count()), Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer")]
        [HttpPut]
        [AssignActionRoute(FabricSegments.UPDATE_FABRIC)]
        public async Task<IActionResult> Update([FromForm] UpdateFabricDataContract fabric, [FromForm] FileFormData fabricImage)
        {
            try
            {
                string oldImageUrl = fabric.ImageUrl;
                string newImageUrl = string.Empty;

                if (fabricImage.File != null)
                {
                    newImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", fabricImage.File);
                }

                Fabric updated = await _fabricService.UpdateFabric(fabric, newImageUrl);

                if (!string.IsNullOrEmpty(oldImageUrl) && !string.IsNullOrEmpty(newImageUrl))
                {
                    FileUploadingHelper.DeleteFile($"{Request.Scheme}://{Request.Host}", oldImageUrl);
                }

                return Ok(SuccessResponseBody(updated, Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer")]
        [HttpPut]
        [AssignActionRoute(FabricSegments.UPDATE_FABRIC_VISIBILITIES)]
        public async Task<IActionResult> UpdateFabricVisibilities([FromBody] FabricVisibilitiesDataContract fabric)
        {
            try
            {
                await _fabricService.UpdateFabricVisibilities(fabric, ClaimHelper.GetUserId(User));
                return Ok(SuccessResponseBody(null, Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer")]
        [HttpGet]
        [AssignActionRoute(FabricSegments.GET_FABRIC_VISIBILITIES)]
        public async Task<IActionResult> GetFabricVisibilities()
        {
            try
            {
                return Ok(SuccessResponseBody(await _fabricService.GetFabricVisibilities(ClaimHelper.GetUserId(User)), Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [Authorize(Roles = "Administrator,Manufacturer")]
        [HttpDelete]
        [AssignActionRoute(FabricSegments.DELETE_FABRIC)]
        public async Task<IActionResult> Delete([FromQuery] long fabricId)
        {
            try
            {
                Fabric deleted = await _fabricService.DeleteFabric(fabricId);
                return Ok(SuccessResponseBody(deleted, Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
