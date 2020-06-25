using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.ProductOptions;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.Options.Contracts;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using MMSL.Services.OptionServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices {
    public class OptionGroupService : IOptionGroupService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IOptionRepositoriesFactory _optionRepositoriesFactory;
        private readonly IProductCategoryRepositoriesFactory _productRepositoriesFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connectionFactory"></param>
        /// <param name="optionRepositoriesFactory"></param>
        public OptionGroupService(IDbConnectionFactory connectionFactory, IOptionRepositoriesFactory optionRepositoriesFactory, IProductCategoryRepositoriesFactory productRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _optionRepositoriesFactory = optionRepositoriesFactory;
            _productRepositoriesFactory = productRepositoriesFactory;
        }

        public Task<List<OptionGroup>> GetOptionGroupsAsync(string search, long? productCategoryId = null) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _optionRepositoriesFactory
                        .NewOptionGroupRepository(connection)
                        .GetAllMapped(search, productCategoryId);
                }
            });

        public Task<OptionGroup> GetOptionGroupAsync(long groupId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _optionRepositoriesFactory
                        .NewOptionGroupRepository(connection)
                        .GetById(groupId);
                }
            });

        public Task<OptionGroup> NewOptionGroupAsync(NewOptionGroupDataContract newOptionGroupDataContract, OptionPrice price = null) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     IOptionGroupRepository optionGroupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);
                     IProductCategoryRepository productRepository = _productRepositoriesFactory.NewProductCategoryRepository(connection);
                     IProductCategoryMapOptionGroupsRepository productMapsRepository = _productRepositoriesFactory.NewProductCategoryMapOptionGroupsRepository(connection);

                     ProductCategory prod = productRepository.GetById(newOptionGroupDataContract.ProductId);

                     if (prod == null || prod.IsDeleted)
                         throw new Exception("Product not found");

                     OptionGroup optionGroup = new OptionGroup {
                         Name = newOptionGroupDataContract.Name,
                         IsMandatory = newOptionGroupDataContract.IsMandatory
                     };

                     optionGroup = optionGroupRepository.NewOptionGroup(optionGroup);

                     if (price != null) {
                         IOptionPriceRepository priceRepository = _optionRepositoriesFactory.NewOptionPriceRepository(connection);

                         price.OptionGroupId = optionGroup.Id;
                         priceRepository.AddPrice(price);
                     }

                     productMapsRepository.NewMap(newOptionGroupDataContract.ProductId, optionGroup.Id);

                     return optionGroup;
                 }
             });

        public Task UpdateOptionGroupAsync(UpdateOptionGroupDataContract optionGroup, OptionPrice price = null) =>
             Task.Run(() => {
                 using (var connection = _connectionFactory.NewSqlConnection()) {
                     IOptionGroupRepository optionGroupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);
                     IOptionPriceRepository priceRepository = _optionRepositoriesFactory.NewOptionPriceRepository(connection);

                     OptionGroup existed = optionGroupRepository.GetById(optionGroup.Id);

                     if (existed != null) {
                         List<OptionPrice> existing = priceRepository.GetPrices(null, optionGroup.Id);
                         OptionPrice lastActive = existing.LastOrDefault();

                         if (price == null && lastActive != null) {

                             lastActive.IsDeleted = true;
                             priceRepository.UpdatePrice(lastActive);

                         } else if (price != null) {

                             if (lastActive == null) {
                                 priceRepository.AddPrice(price);
                             } else {
                                 price.Id = lastActive.Id;
                                 priceRepository.UpdatePrice(price);
                             }

                         }

                         foreach (OptionUnit unit in existed.OptionUnits) {
                             if (unit.CurrentPrice != null) {
                                 unit.CurrentPrice.IsDeleted = true;
                                 priceRepository.UpdatePrice(unit.CurrentPrice);
                             }
                         }

                         int rowAffected = optionGroupRepository.UpdateOptionGroup(optionGroup.GetEntity());
                     } else {
                         UserExceptionCreator<NotFoundValueException>.Create(NotFoundValueException.VALUE_NOT_FOUND).Throw();
                     }
                 }
             });

        public Task DeleteOptionGroupAsync(long optionGroupId) =>
              Task.Run(() => {
                  using (var connection = _connectionFactory.NewSqlConnection()) {
                      IOptionGroupRepository optionGroupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);

                      OptionGroup existed = optionGroupRepository.GetById(optionGroupId);

                      if (existed != null) {
                          existed.IsDeleted = true;
                          optionGroupRepository.UpdateOptionGroup(existed);
                      } else {
                          UserExceptionCreator<NotFoundValueException>.Create(NotFoundValueException.VALUE_NOT_FOUND).Throw();
                      }
                  }
              });

        public Task<List<OptionGroup>> GetProductOptionGroupsWithPermissionSettingsAsync(long productId, long productSettingsId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _optionRepositoriesFactory
                        .NewOptionGroupRepository(connection)
                        .GetWithPermissionsByProductId(productId, productSettingsId);
                }
            });
    }
}
