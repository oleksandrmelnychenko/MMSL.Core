using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Services.MeasurementServices.Contracts;

namespace MMSL.Server.Core.Controllers.Measurement {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Measurements)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Measurements)]
    public class MeasurementController : WebApiControllerBase {
        private readonly IMeasurementService _measurementService;

        public MeasurementController(
            IMeasurementService measurementService,
            IResponseFactory responseFactory,
            IStringLocalizer<MeasurementController> localizer)
            : base(responseFactory, localizer) {
            _measurementService = measurementService;
        }
    }
}
