using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Common.WebApi.RoutingConfiguration.DeliveryTimelines;
using MMSL.Common.WebApi.RoutingConfiguration.Options;
using MMSL.Domain.DataContracts.DeliveryTimelines;
using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Services.DeliveryTimelines.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Controllers.DeliveryTimeLines {
    [AssignControllerLocalizedRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.DeliveryTimelines)]
    [AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.DeliveryTimelines)]
    public class DeliveryTimeLineController : WebApiControllerBase {

        private readonly IDeliveryTimelineService _deliveryTimelineService;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="deliveryTimelineService"></param>
        /// <param name="responseFactory"></param>
        /// <param name="localizer"></param>
        public DeliveryTimeLineController(
          IDeliveryTimelineService deliveryTimelineService,
          IResponseFactory responseFactory,
          IStringLocalizer<DeliveryTimeLineController> localizer)
          : base(responseFactory, localizer) {
            _deliveryTimelineService = deliveryTimelineService;
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(DeliveryTimelineSegments.GET_DELIVERY_TIMELINES)]
        public async Task<IActionResult> GetAll([FromQuery]string searchPhrase, [FromQuery]bool isDefault) {
            try {
                List<DeliveryTimeline> deliveryTimeLines = await _deliveryTimelineService.GetDeliveryTimelinesAsync(searchPhrase, isDefault);

                return Ok(SuccessResponseBody(deliveryTimeLines, Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(DeliveryTimelineSegments.GET_DELIVERY_TIMELINES_BY_PRODUCT)]
        public async Task<IActionResult> GetAllByProduct([FromQuery]long id) {
            try {
                List<DeliveryTimeline> deliveryTimeLines = await _deliveryTimelineService.GetDeliveryTimelinesByProductAsync(id);

                return Ok(SuccessResponseBody(deliveryTimeLines, Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(DeliveryTimelineSegments.NEW_DELIVERY_TIMELINE)]
        public async Task<IActionResult> NewDeliveryTimeline([FromBody]NewDeliveryTimelineDataContract newDeliveryTimelineDataContract) {
            try {
                if (newDeliveryTimelineDataContract == null || string.IsNullOrEmpty(newDeliveryTimelineDataContract.Name))
                    throw new ArgumentNullException("NewDeliveryTimelineDataContract");                

                DeliveryTimeline deliveryTimeLine = await _deliveryTimelineService.NewDeliveryTimelineAsync(newDeliveryTimelineDataContract);

                return Ok(SuccessResponseBody(deliveryTimeLine, Localizer["Delivery timeline has been created successfully"]));
            }          
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [Authorize]
        [AssignActionRoute(DeliveryTimelineSegments.ASSIGN_DELIVERY_TIMELINE)]
        public async Task<IActionResult> AssignDeliveryTimeline([FromBody]AssignDeliveryTimelineDataContract assignDeliveryTimelineDataContract) {
            try {
                if (assignDeliveryTimelineDataContract == null || assignDeliveryTimelineDataContract.DeliveryTimelines == null) 
                    throw new ArgumentNullException("AssignDeliveryTimelineDataContract");

                var deliveryTimlines = await _deliveryTimelineService.AssignDeliveryTimelineAsync(assignDeliveryTimelineDataContract);

                return Ok(SuccessResponseBody(deliveryTimlines, Localizer["Successfully completed"]));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut]
        [Authorize]
        [AssignActionRoute(DeliveryTimelineSegments.UPDATE_DELIVERY_TIMELINE)]
        public async Task<IActionResult> UpdateDeliveryTimeline([FromBody]DeliveryTimeline deliveryTimeline) {
            try {
                if (deliveryTimeline == null) throw new ArgumentNullException("UpdateDeliveryTimeline");

                await _deliveryTimelineService.UpdateDeliveryTimelineAsync(deliveryTimeline);

                return Ok(SuccessResponseBody(deliveryTimeline, Localizer["DeliveryTimeline successfully updated"]));
            }
            catch (NotFoundValueException exc) {
                Log.Error(exc.GetUserMessageException);
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete]
        [Authorize]
        [AssignActionRoute(DeliveryTimelineSegments.DELETE_DELIVERY_TIMELINE)]
        public async Task<IActionResult> DeleteDeliveryTimeline([FromQuery]long deliveryTimelineId) {
            try {
                if (deliveryTimelineId == default(long)) throw new ArgumentNullException("DeleteDeliveryTimeline");

                await _deliveryTimelineService.DeleteDeliveryTimelineAsync(deliveryTimelineId);

                return Ok(SuccessResponseBody(deliveryTimelineId, Localizer["DeliveryTimeline successfully deleted"]));
            }
            catch (NotFoundValueException exc) {
                Log.Error(exc.GetUserMessageException);
                return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest));
            }
            catch (Exception exc) {
                Log.Error(exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
