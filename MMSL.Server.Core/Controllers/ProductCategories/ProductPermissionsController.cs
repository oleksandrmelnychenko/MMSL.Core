using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
using MMSL.Domain.DataContracts.Products;
using MMSL.Services.OptionServices.Contracts;
using MMSL.Services.ProductCategories.Contracts;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.ProductCategories {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductPermissions)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductPermissions)]
    public class ProductPermissionsController : WebApiControllerBase {

        private readonly IProductPermissionSettingService _productPermissionSettingService;
        private readonly IOptionGroupService _optionGroupService;
        
        public ProductPermissionsController(
            IProductPermissionSettingService productPermissionSettingService,
            IOptionGroupService optionGroupService, 
            IResponseFactory responseFactory,
            IStringLocalizer<ProductPermissionsController> localizer)
            : base(responseFactory, localizer) {

            _productPermissionSettingService = productPermissionSettingService;
            _optionGroupService = optionGroupService;
        }

        [HttpGet]
        [AssignActionRoute(ProductPermissionSegments.GET_PRODUCT_PERMISSION_SETTINGS)]
        public async Task<IActionResult> GetAll([FromQuery] long productCategoryId, [FromQuery] string dealerSearchTerm) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.GetSettingsByProduct(productCategoryId, dealerSearchTerm), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(ProductPermissionSegments.GET_PRODUCT_PERMISSION_SETTING)]
        public async Task<IActionResult> Get([FromQuery] long productPermissionSettingId) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.GetPermissionSettingsById(productPermissionSettingId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(ProductPermissionSegments.GET_PRODUCT_OPTIONS_WITH_PERMISSION_SETTING)]
        public async Task<IActionResult> GetPrdouctOptionsWithPermissions([FromQuery] long productId, [FromQuery] long productPermissionSettingId) {
            try {
                return Ok(SuccessResponseBody(await _optionGroupService.GetProductOptionGroupsWithPermissionSettingsAsync(productId, productPermissionSettingId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
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
        [AssignActionRoute(ProductPermissionSegments.UPDATE_PRODUCT_PERMISSION_SETTINGS)]
        public async Task<IActionResult> Update([FromBody] UpdateProductPermissionSettingsDataContract productPermissionSettings) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.UpdateProductPermissionSetting(productPermissionSettings), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [AssignActionRoute(ProductPermissionSegments.DELETE_PRODUCT_PERMISSION_SETTINGS)]
        public async Task<IActionResult> Delete([FromQuery] long productPermissionSettingId) {
            try {
                return Ok(SuccessResponseBody(await _productPermissionSettingService.DeletePermissionSettingsById(productPermissionSettingId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(ProductPermissionSegments.BIND_SETTING_TO_DEALERS)]
        public async Task<IActionResult> BindDealers([FromBody] ProductPermissionToDealersBindingDataContract productPermissionBinding) {
            try {
                await _productPermissionSettingService.SetDealerToProductPermissionSetting(productPermissionBinding);
                return Ok(SuccessResponseBody(await _productPermissionSettingService.GetPermissionSettingsById(productPermissionBinding.ProductPermissionSettingId), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

    }
}
