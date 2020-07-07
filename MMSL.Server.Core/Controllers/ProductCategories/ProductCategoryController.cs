using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common;
using MMSL.Common.Helpers;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.Entities.Products;
using MMSL.Server.Core.Helpers;
using MMSL.Services.ProductCategories.Contracts;
using MMSL.Services.StoreCustomerServices.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.ProductCategories {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductCategory)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.ProductCategory)]
    public class ProductCategoryController : WebApiControllerBase {

        private readonly IProductCategoryService _productCategoryService;
        private readonly IStoreCustomerProductProfileService _storeCustomerProductProfileService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="productCategoryService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public ProductCategoryController(
            IProductCategoryService productCategoryService,
            IStoreCustomerProductProfileService storeCustomerProductProfileService,
            IResponseFactory responseFactory,
            IStringLocalizer<ProductCategoryController> localizer) : base(responseFactory, localizer) {

            _productCategoryService = productCategoryService;
            _storeCustomerProductProfileService = storeCustomerProductProfileService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.GET_PRODUCT_CATEGORIES)]
        public async Task<IActionResult> GetAll([FromQuery] string searchPhrase, [FromQuery] long? dealerAccountId) {
            try {
                bool isDealer = ClaimHelper.GetUserRoles(User).Any(x => x == RoleType.Dealer.ToString());
                long identityId = ClaimHelper.GetUserId(User);

                return Ok(SuccessResponseBody(
                    await _productCategoryService.GetProductCategoriesAsync(searchPhrase, dealerAccountId, identityId, isDealer),
                    Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Dealer")]
        [AssignActionRoute(ProductCategorySegments.GET_PRODUCT_CUSTOMER_PROFILES)]
        public async Task<IActionResult> GetCustomerProfiles([FromQuery] long storeCustomerId) {
            try {
                long identityId = ClaimHelper.GetUserId(User);

                return Ok(SuccessResponseBody(
                    await _storeCustomerProductProfileService.GetProductsWithProfilesByCustomerAsync(storeCustomerId, identityId),
                    Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        ///// <summary>
        /////     UNUSED for now. 
        /////     Maybe need to delete.
        ///// </summary>
        ///// <param name="searchPhrase"></param>
        ///// <param name="dealerAccountId"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize(Roles = "Administrator,Manufacturer")]
        //[AssignActionRoute(ProductCategorySegments.GET_PRODUCT_CATEGORIES_WITH_AVAILABILITY)]
        //public async Task<IActionResult> GetWithAvailability([FromQuery] string searchPhrase, [FromQuery] long? dealerAccountId) {
        //    try {
        //        List<ProductCategory> products = await _productCategoryService.GetProductCategoriesAvailabilitiesAsync(searchPhrase, dealerAccountId);

        //        return Ok(SuccessResponseBody(products, Localizer["Successfully completed"]));
        //    } catch (Exception exc) {
        //        Log.Error(exc.Message);
        //        return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        //    }
        //}

        [HttpGet]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.GET_PRODUCT_CATEGORY)]
        public async Task<IActionResult> Get([FromQuery] long productCategoryId, [FromQuery] bool showBodyPostureOnly) {
            try {
                bool isDealer = ClaimHelper.GetUserRoles(User).Any(x => x == RoleType.Dealer.ToString());
                long identityId = ClaimHelper.GetUserId(User);

                return Ok(SuccessResponseBody(await _productCategoryService.GetProductCategoryAsync(productCategoryId, identityId, isDealer, showBodyPostureOnly), Localizer["Successfully completed"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.NEW_PRODUCT_CATEGORY)]
        public async Task<IActionResult> NewProductCategory([FromQuery] NewProductCategoryDataContract newProductCategoryDataContract, [FromForm] FileFormData formData) {
            try {
                if (newProductCategoryDataContract == null)
                    throw new ArgumentNullException("NewProductCategoryDataContract");

                if (string.IsNullOrEmpty(newProductCategoryDataContract.Name))
                    throw new ArgumentNullException("NewProductCategoryDataContract.Name");

                ProductCategory productCategory = newProductCategoryDataContract.GetEntity();

                if (formData?.File != null) {
                    productCategory.ImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", formData.File);
                }

                productCategory = await _productCategoryService.NewProductCategoryAsync(productCategory, newProductCategoryDataContract.OptionGroupIds);

                return Ok(SuccessResponseBody(productCategory, Localizer["New product has been created successfully"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.UPDATE_PRODUCT_CATEGORY)]
        public async Task<IActionResult> UpdateProductCategory([FromQuery] UpdateProductCategoryDataContract productDataContract, [FromForm] FileFormData formData) {
            try {
                if (productDataContract == null) throw new ArgumentNullException("UpdateProductCategory");

                ProductCategory product = productDataContract.GetEntity();

                if (formData?.File != null) {
                    product.ImageUrl = await FileUploadingHelper.UploadFile($"{Request.Scheme}://{Request.Host}", formData.File);
                }

                await _productCategoryService.UpdateProductCategoryAsync(product);

                if (string.IsNullOrEmpty(productDataContract.ImageUrl) || productDataContract.ImageUrl != product.ImageUrl) {
                    if (!string.IsNullOrEmpty(productDataContract.ImageUrl)) {
                        FileUploadingHelper.DeleteFile($"{Request.Scheme}://{Request.Host}", productDataContract.ImageUrl);
                    }
                }

                return Ok(SuccessResponseBody(productDataContract, Localizer["Product successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.UPDATE_PRODUCT_CATEGORY_OPTION_GROUPS)]
        public async Task<IActionResult> UpdateProductCategoryOptionGroups([FromBody] IEnumerable<ProductCategoryMapOptionGroup> maps) {
            try {
                if (maps == null) throw new ArgumentNullException("UpdateProductCategoryOptionGroups");

                await _productCategoryService.UpdateProductCategoryOptionGroupsAsync(maps);

                return Ok(SuccessResponseBody("Completed", Localizer["Product styles successfully updated"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.DELETE_PRODUCT_CATEGORY)]
        public async Task<IActionResult> DeleteProductCategory([FromQuery] long productCategoryId) {
            try {
                await _productCategoryService.DeleteProductCategoryAsync(productCategoryId);

                return Ok(SuccessResponseBody(productCategoryId, Localizer["Product successfully deleted"]));
            } catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
