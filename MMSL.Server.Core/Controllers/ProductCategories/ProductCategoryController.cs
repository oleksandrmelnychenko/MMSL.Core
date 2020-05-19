using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
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
            catch (InvalidIdentityException exc) {
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
