using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Services.MeasurementServices.Contracts;

namespace MMSL.Server.Core.Controllers.Measurements {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementSizes)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementSizes)]
    public class MeasurementSizeController : WebApiControllerBase {
        private readonly IMeasurementSizeService _measurementSizeService;

        public MeasurementSizeController(
            IMeasurementSizeService measurementSizeService, 
            IResponseFactory responseFactory,
            IStringLocalizer<MeasurementSizeController> localizer)
            : base(responseFactory, localizer) {
            _measurementSizeService = measurementSizeService;
        }
    }
}
