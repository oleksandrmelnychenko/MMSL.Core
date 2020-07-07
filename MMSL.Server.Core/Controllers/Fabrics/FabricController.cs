using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Services.FabricServices.Contracts;

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
    }
}
