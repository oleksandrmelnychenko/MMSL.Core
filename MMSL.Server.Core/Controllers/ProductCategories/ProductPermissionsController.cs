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
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.ProductCategories {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductPermissions)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductPermissions)]
    public class ProductPermissionsController : WebApiControllerBase {

        private readonly IProductPermissionSettingService _productPermissionSettingService;

        public ProductPermissionsController(
            IProductPermissionSettingService productPermissionSettingService, 
            IResponseFactory responseFactory,
            IStringLocalizer<ProductPermissionsController> localizer)
            : base(responseFactory, localizer) {

            _productPermissionSettingService = productPermissionSettingService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(ProductPermissionSegments.GET_PRODUCT_PERMISSION_SETTINGS)]
        public async Task<IActionResult> GetAll([FromQuery] long productCategoryId) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.GetSettingsByProduct(productCategoryId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(ProductPermissionSegments.GET_PRODUCT_PERMISSION_SETTING)]
        public async Task<IActionResult> Get([FromQuery] long productPermissionSettingId) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.GetPermissionSettingsById(productPermissionSettingId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(ProductPermissionSegments.ADD_PRODUCT_PERMISSION_SETTING)]
        public async Task<IActionResult> Add([FromBody] NewProductPermissionSettingsDataContract productPermissionSettings) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.AddProductPermissionSetting(productPermissionSettings), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }


        [HttpPut]
        [Authorize]
        [AssignActionRoute(ProductPermissionSegments.UPDATE_PRODUCT_PERMISSION_SETTINGS)]
        public async Task<IActionResult> Update([FromBody] NewProductPermissionSettingsDataContract productPermissionSettings) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.UpdateProductPermissionSetting(productPermissionSettings), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(ProductPermissionSegments.DELETE_PRODUCT_PERMISSION_SETTINGS)]
        public async Task<IActionResult> Delete([FromQuery] long productPermissionSettingId) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.DeletePermissionSettingsById(productPermissionSettingId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
