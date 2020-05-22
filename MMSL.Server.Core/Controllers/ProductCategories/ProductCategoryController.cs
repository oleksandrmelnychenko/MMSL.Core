using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Products;
using MMSL.Services.ProductCategories.Contracts;
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

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="productCategoryService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public ProductCategoryController(
            IProductCategoryService productCategoryService,
            IResponseFactory responseFactory,
            IStringLocalizer<ProductCategoryController> localizer) : base(responseFactory, localizer) {

            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.GET_PRODUCT_CATEGORIES)]
        public async Task<IActionResult> GetAll([FromQuery]string searchPhrase) {
            try {
                List<ProductCategory> products = await _productCategoryService.GetProductCategoriesAsync(searchPhrase);

                return Ok(SuccessResponseBody(products, Localizer["Successfully completed"]));
            }           
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.NEW_PRODUCT_CATEGORY)]
        public async Task<IActionResult> NewStore([FromBody] NewProductCategoryDataContract newProductCategoryDataContract, [FromForm]FileFormData formData) {
            try {
                if (newProductCategoryDataContract == null) throw new ArgumentNullException("NewProductCategoryDataContract");

                if (string.IsNullOrEmpty(newProductCategoryDataContract.Name)) throw new ArgumentNullException("NewProductCategoryDataContract");

                ProductCategory productCategory = await _productCategoryService.NewProductCategoryAsync(newProductCategoryDataContract);

                return Ok(SuccessResponseBody(productCategory, Localizer["New ProductCategory has been created successfully"]));
            }          
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.UPDATE_PRODUCT_CATEGORY)]
        public async Task<IActionResult> UpdateProductCategory([FromBody]ProductCategory product) {
            try {
                if (product == null) throw new ArgumentNullException("UpdateProductCategory");

                await _productCategoryService.UpdateProductCategoryAsync(product);

                return Ok(SuccessResponseBody(product, Localizer["ProductCategory successfully updated"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(ProductCategorySegments.DELETE_PRODUCT_CATEGORY)]
        public async Task<IActionResult> DeleteProductCategory([FromQuery]long productCategoryId) {
            try {
                await _productCategoryService.DeleteProductCategoryAsync(productCategoryId);

                return Ok(SuccessResponseBody(productCategoryId, Localizer["ProductCategory successfully deleted"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
