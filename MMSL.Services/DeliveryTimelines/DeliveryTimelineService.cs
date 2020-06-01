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


        public Task<List<DeliveryTimeline>> GetDeliveryTimelinesAsync(string searchPhrase, bool isDefault) =>
              Task.Run(() => {
                  using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                      IDeliveryTimelineRepository deliveryTimelineRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineRepository(connection);

                      return deliveryTimelineRepository.GetAll(searchPhrase, isDefault); ;
                  }
              });

        public Task<DeliveryTimeline> NewDeliveryTimelineAsync(NewDeliveryTimelineDataContract newDeliveryTimelineDataContract) =>
              Task.Run(() => {
                  using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                      IDeliveryTimelineRepository deliveryTimelineRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineRepository(connection);

                      DeliveryTimeline deliveryTimeline = deliveryTimelineRepository.New(new DeliveryTimeline {
                          Name = newDeliveryTimelineDataContract.Name,
                          Ivory = newDeliveryTimelineDataContract.Ivory,
                          Silver = newDeliveryTimelineDataContract.Silver,
                          Black = newDeliveryTimelineDataContract.Black,
                          Gold = newDeliveryTimelineDataContract.Gold,
                          IsDefault = newDeliveryTimelineDataContract.IsDefault
                      });

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

        public Task DeleteDeliveryTimelineAsync(long deliveryId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IDeliveryTimelineRepository deliveryTimelineRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineRepository(connection);

                    DeliveryTimeline existed = deliveryTimelineRepository.GetById(deliveryId);

                    if (existed != null) {
                        existed.IsDeleted = true;
                        deliveryTimelineRepository.Update(existed);
                    } else {
                        UserExceptionCreator<NotFoundValueException>.Create(NotFoundValueException.VALUE_NOT_FOUND).Throw();
                    }
                }
            });

        public Task AssignDeliveryTimelineAsync(AssignDeliveryTimelineDataContract assignDeliveryTimelineDataContract) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     IDeliveryTimelineRepository deliveryTimelineRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineRepository(connection);
                     var deliveryTimelineProductMapRepository = _deliveryTimelineRepositoriesFactory.NewDeliveryTimelineProductMapRepository(connection);

                     foreach (DeliveryTimeline deliveryTimeline in assignDeliveryTimelineDataContract.DeliveryTimelines) {
                         if (deliveryTimeline.IsNew()) {
                             var created = deliveryTimelineRepository.New(deliveryTimeline);
                             if (created != null) {
                                 deliveryTimelineProductMapRepository.New(assignDeliveryTimelineDataContract.ProductCategoryId, created.Id);
                             }
                         } else {
                             if (deliveryTimeline.IsDefault) {
                                 DeliveryTimelineProductMap existedDeliveryTimelineProductMap = deliveryTimelineProductMapRepository.GetByIds(assignDeliveryTimelineDataContract.ProductCategoryId, deliveryTimeline.Id);
                                 if (existedDeliveryTimelineProductMap != null) {
                                     var created = deliveryTimelineRepository.New(new DeliveryTimeline {
                                         Name = deliveryTimeline.Name,
                                         Ivory = deliveryTimeline.Ivory,
                                         Silver = deliveryTimeline.Silver,
                                         Black = deliveryTimeline.Black,
                                         Gold = deliveryTimeline.Gold,
                                         IsDefault = false
                                     });
                                     existedDeliveryTimelineProductMap.DeliveryTimelineId = created.Id;
                                     deliveryTimelineProductMapRepository.Update(existedDeliveryTimelineProductMap);
                                 }

                             } else {                                 
                                 deliveryTimelineRepository.Update(deliveryTimeline);
                             }
                         }
                     }
                 }
             });
    }
}
