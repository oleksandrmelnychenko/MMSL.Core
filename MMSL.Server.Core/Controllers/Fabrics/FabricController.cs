using ClosedXML.Excel;
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
using MMSL.Domain.Entities;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.EntityHelpers;
using MMSL.Server.Core.Helpers;
using MMSL.Services.FabricServices.Contracts;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.Fabrics
{
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Fabrics)]

    public class FabricController : WebApiControllerBase
    {

        private readonly IFabricService _fabricService;

        public FabricController(IFabricService fabricService, IResponseFactory responseFactory, IStringLocalizer<FabricController> localizer)
            : base(responseFactory, localizer)
        {
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

                string downloadPath = await _fabricService.PrepareFabricsPdf(searchPhrase, filters);
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


                List<Fabric> fabricsResult = ParseExcel(tempPath);

                //TODO: import here

                if (!string.IsNullOrEmpty(tempPath) && System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }

                return Ok(SuccessResponseBody(null, Localizer["Successful"]));
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        private List<Fabric> ParseExcel(string path)
        {
            List<Fabric> fabricsResult = new List<Fabric>();

            try
            {
                FileStream originalSourceFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                MemoryStream forParsingMemoryStream = new MemoryStream();
                originalSourceFileStream.CopyTo(forParsingMemoryStream);
                originalSourceFileStream.Dispose();
                originalSourceFileStream.Close();

                XLWorkbook xlWorkbook = new XLWorkbook(forParsingMemoryStream);

                foreach (IXLWorksheet xlSheet in xlWorkbook.Worksheets)
                {


                    IXLRow xlHeaderRow = xlSheet.RowsUsed().FirstOrDefault();
                    if (xlHeaderRow != null)
                    {


                        Tuple<string, IXLAddress>[] rawHeadersWithAddresses = xlHeaderRow.CellsUsed()
                            .Select<IXLCell, Tuple<string, IXLAddress>>(cell => new Tuple<string, IXLAddress>(cell.Value.ToString(), cell.Address))
                            .ToArray<Tuple<string, IXLAddress>>();

                        IEnumerable<IXLRow> xlDataRows = xlSheet.Rows(xlHeaderRow.RowNumber() + 1, xlSheet.RowsUsed().Last().RowNumber());

                        for (int i = 0; i < xlDataRows.Count(); i++)
                        {
                            try
                            {

                                Fabric fabric = BuildEntityImportRow(xlDataRows.ElementAt(i), i, rawHeadersWithAddresses);

                                fabricsResult.Add(fabric);
                            }
                            catch (Exception)
                            {

                            }

                            
                        }
                    }


                }
            }
            catch (Exception exc)
            {
                Debugger.Break();
            }

            return fabricsResult;
        }

        protected Fabric BuildEntityImportRow(IXLRow xlRow, int rawIndex, IEnumerable<Tuple<string, IXLAddress>> rawHeadersStrings)
        {
            Fabric assetGroupRow = new Fabric();

            if (xlRow != null)
            {

                string[] relatedEntityPropertyKeys = new string[]{
                    "Fabric Code",
                    "Description",
                    "Status",
                    "Meters",
                    "Mill",
                    "Color",
                    "Composition",
                    "GSM",
                    "Count",
                    "Weave",
                    "Pattern",
                };

                for (int i = 0; i < rawHeadersStrings.Count(); i++)
                {
                    IXLCell xlCell = xlRow.Cell(rawHeadersStrings.ElementAt(i).Item2.ColumnNumber);

                    if (relatedEntityPropertyKeys.Contains<string>(rawHeadersStrings.ElementAt(i).Item1))
                    {
                        string targetPropertyName = rawHeadersStrings.ElementAt(i).Item1;


                        if (xlCell != null)
                        {
                            PropertyInfo[] props = assetGroupRow.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(HeaderAttribute))).ToArray<PropertyInfo>();
                            foreach (PropertyInfo property in props)
                            {
                                HeaderAttribute headerAttribute = property.GetCustomAttribute<HeaderAttribute>();
                                if (headerAttribute != null && headerAttribute.Header.Equals(targetPropertyName))
                                {

                                    if (targetPropertyName == "Status")
                                    {
                                        FabricStatuses result = ParseFabricStatus(TryExtractValueFromIXLCell(xlCell));
                                        property.SetValue(assetGroupRow, result);
                                    }
                                    else if (targetPropertyName == "Meters")
                                    {
                                        float result = ParseFabricNumber(TryExtractValueFromIXLCell(xlCell));
                                        property.SetValue(assetGroupRow, result);
                                    }
                                    else
                                    {
                                        property.SetValue(assetGroupRow, TryExtractValueFromIXLCell(xlCell));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return assetGroupRow;
        }

        private FabricStatuses ParseFabricStatus(string rawValue)
        {
            FabricStatuses result = default(FabricStatuses);

            string normalizedValue = new string(rawValue.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower();

            if (FabricStatuses.Discontinued.ToString().ToLower() == normalizedValue)
                result = FabricStatuses.Discontinued;
            else if (FabricStatuses.InStock.ToString().ToLower() == normalizedValue)
                result = FabricStatuses.InStock;
            else if (FabricStatuses.OutOfStock.ToString().ToLower() == normalizedValue)
                result = FabricStatuses.OutOfStock;

            //Enum.TryParse<FabricStatuses>(new string(rawValue.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray()), out result);

            return result;
        }

        private float ParseFabricNumber(string rawValue)
        {
            float result = 0;

            float.TryParse(rawValue, out result);

            return result;
        }

        protected string TryExtractValueFromIXLCell(IXLCell xlCell)
        {

            string result = null;

            if (xlCell != null)
            {
                /// Don't remove it, not sure how it works but 
                /// IXLCell cell value will be changed (issue: date-time can be evaluated as ticks/date-format)
                //string richTextValue = xlCell.RichText?.ToString();
                //string value = xlCell.Value?.ToString();
                string cachedValue = xlCell.CachedValue?.ToString();

                /// TODO: urgent fix <see cref="IXLCell.Value"/> throws exception `Unknown function...`
                /// 
                //if (string.IsNullOrEmpty(xlCell.FormulaA1)
                //    && string.IsNullOrEmpty(xlCell.FormulaR1C1)) {
                //    result = xlCell.Value?.ToString();
                //} else {
                //    result = xlCell.CachedValue?.ToString();
                //}

                result = cachedValue;
            }

            return result;
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
