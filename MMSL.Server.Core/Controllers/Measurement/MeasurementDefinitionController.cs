using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Services.MeasurementServices.Contracts;

namespace MMSL.Server.Core.Controllers.Measurement {
    [Authorize]
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementDefinitions)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.MeasurementDefinitions)]
    public class MeasurementDefinitionController : WebApiControllerBase {
        private readonly IMeasurementDefinitionService _measurementDefinitionService;

        public MeasurementDefinitionController(
            IMeasurementDefinitionService measurementDefinitionService,
            IResponseFactory responseFactory,
            IStringLocalizer<MeasurementDefinitionController> localizer)
            : base(responseFactory, localizer) {
            _measurementDefinitionService = measurementDefinitionService;
        }
    }
}
