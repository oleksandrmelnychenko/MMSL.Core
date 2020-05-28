using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Domain.DataContracts.DeliveryTimelines;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Domain.Repositories.DeliveryTimelines.Contracts;
using MMSL.Services.DeliveryTimelines.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.DeliveryTimelines {
    public class DeliveryTimelineService : IDeliveryTimelineService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IDeliveryTimelineRepositoriesFactory _deliveryTimelineRepositoriesFactory;

        public DeliveryTimelineService(IDeliveryTimelineRepositoriesFactory deliveryTimelineRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
            _deliveryTimelineRepositoriesFactory = deliveryTimelineRepositoriesFactory;
        }

        public Task<List<DeliveryTimeline>> GetDeliveryTimelinesAsync(string searchPhrase) =>
              Task.Run(() => {
                  using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                      IDeliveryTimelineRepository deliveryTimelineRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineRepository(connection);

                      return deliveryTimelineRepository.GetAll(searchPhrase); ;
                  }
              });

        public Task<DeliveryTimeline> NewDeliveryTimelineAsync(NewDeliveryTimelineDataContract newDeliveryTimelineDataContract) =>
              Task.Run(() => {
                  using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                      IDeliveryTimelineRepository deliveryTimelineRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineRepository(connection);

                      DeliveryTimeline deliveryTimeline = deliveryTimelineRepository.New(newDeliveryTimelineDataContract);

                      return deliveryTimeline;
                  }
              });

        public Task UpdateDeliveryTimelineAsync(DeliveryTimeline deliveryTimeline) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IDeliveryTimelineRepository deliveryTimelineRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineRepository(connection);

                    DeliveryTimeline existed = deliveryTimelineRepository.GetById(deliveryTimeline.Id);

                    if (existed != null) {
                        int rowAffected = deliveryTimelineRepository.Update(deliveryTimeline);
                    } else {
                        UserExceptionCreator<NotFoundValueException>.Create(NotFoundValueException.VALUE_NOT_FOUND).Throw();
                    }
                }
            });
    }
}
