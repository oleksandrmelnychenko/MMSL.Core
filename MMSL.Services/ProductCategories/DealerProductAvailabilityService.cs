using MMSL.Common.WebApi.RoutingConfiguration.ProductCategories;
using MMSL.Domain.DataContracts.Products;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using MMSL.Services.ProductCategories.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories {
    public class DealerProductAvailabilityService : IDealerProductAvailabilityService {

        private readonly IProductCategoryRepositoriesFactory _productCategoryRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public DealerProductAvailabilityService(
            IProductCategoryRepositoriesFactory productCategoryRepositoriesFactory,
            IDbConnectionFactory connectionFactory) {
            _productCategoryRepositoriesFactory = productCategoryRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task SaveProductAvailabilitySettings(IEnumerable<ProductAvailabilitiesDataContract> settings) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IDealerProductAvailabilityRepository repo = _productCategoryRepositoriesFactory.NewDealerProductAvailabilityRepository(connection);

                    foreach (ProductAvailabilitiesDataContract setting in settings) {
                        DealerProductAvailability entity = setting.GetEntity();

                        if (entity.IsNew() && entity.IsDisabled) {
                            repo.AddAvailability(entity);
                        } else if (!entity.IsDisabled) {
                            repo.DeleteAvailability(entity);
                        }
                    }
                }
            });
    }
}
