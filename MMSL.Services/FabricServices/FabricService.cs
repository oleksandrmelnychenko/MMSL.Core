using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.DataContracts.Filters;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Fabrics.Contracts;
using MMSL.Services.FabricServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using MMSL.Common;

namespace MMSL.Services.FabricServices
{
    public class FabricService : IFabricService
    {

        private readonly IFabricRepositoriesFactory _fabricRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public FabricService(IFabricRepositoriesFactory fabricRepositoriesFactory, IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _fabricRepositoriesFactory = fabricRepositoriesFactory;
        }

        public Task<Fabric> GetByIdAsync(long fabricId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                 {
                     return _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetById(fabricId);
                 }
             });

        public Task<Fabric> AddFabric(NewFabricDataContract fabric, long userIdentityId, string imageUrl = null) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = fabric.GetEntity();
                    fabricEntity.UserIdentityId = userIdentityId;

                    if (!string.IsNullOrEmpty(imageUrl))
                        fabricEntity.ImageUrl = imageUrl;

                    FabricVisibilitiesDataContract visibilities = repository.GetFabricsVisibilities(userIdentityId);

                    if (visibilities != null)
                    {
                        fabricEntity.IsColorVisible = visibilities.IsColorVisible;
                        fabricEntity.IsCompositionVisible = visibilities.IsCompositionVisible;
                        fabricEntity.IsCountVisible = visibilities.IsCountVisible;
                        fabricEntity.IsGSMVisible = visibilities.IsGSMVisible;
                        fabricEntity.IsMetresVisible = visibilities.IsMetresVisible;
                        fabricEntity.IsMillVisible = visibilities.IsMillVisible;
                        fabricEntity.IsPatternVisible = visibilities.IsPatternVisible;
                        fabricEntity.IsWeaveVisible = visibilities.IsWeaveVisible;
                    }

                    return repository.AddFabric(fabricEntity);
                }
            });

        public Task<Fabric> UpdateFabric(UpdateFabricDataContract fabric, string imageUrl = null) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = fabric.GetEntity();

                    if (!string.IsNullOrEmpty(imageUrl))
                        fabricEntity.ImageUrl = imageUrl;

                    return repository.UpdateFabric(fabricEntity);
                }
            });

        public Task<Fabric> DeleteFabric(long fabricId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = repository.GetById(fabricId);

                    if (fabricEntity == null)
                        throw new Exception("Fabric not found");

                    fabricEntity.IsDeleted = true;

                    return repository.UpdateFabric(fabricEntity);
                }
            });

        public Task UpdateFabricVisibilities(FabricVisibilitiesDataContract fabric, long userIdentityId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    //Fabric defaultFabricEntity = repository.GetById(fabric.Id);

                    //defaultFabricEntity = fabric.MapFabric(defaultFabricEntity);

                    repository.UpdateFabricVisibilities(fabric, userIdentityId);

                    //return repository.GetById(defaultFabricEntity.Id);
                }
            });

        public Task<FabricVisibilitiesDataContract> GetFabricVisibilities(long userIdentityId) =>
           Task.Run(() => {
               using (IDbConnection connection = _connectionFactory.NewSqlConnection())
               {
                   IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                   return repository.GetFabricsVisibilities(userIdentityId);
               }
           });

        public Task<PaginatingResult<Fabric>> GetFabrics(int pageNumber, int limit, string searchPhrase, FilterItem[] filters, long? ownerUserIdentityId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetPagination(pageNumber, limit, searchPhrase, filters, ownerUserIdentityId);
                }
            });

        public Task<List<FilterItem>> GetFabricFilters(long? ownerUserIdenetity) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetFilters(ownerUserIdenetity);
                }
            });

        public Task<string> PrepareFabricsPdf(string searchPhrase, FilterItem[] filters) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IEnumerable<Fabric> fabrics = _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetAllFabrics(searchPhrase, filters);

                    string fullFilePath = Path.Combine(ConfigurationManager.UploadsPath, $"Fabrics_{DateTime.Now.Ticks}.pdf");

                    //TODO: create pdf
                    Document doc1 = new Document(PageSize.A4);

                    foreach (Fabric fabric in fabrics)
                    {
                        //TODO: check if path correct
                        string serverImagePath = Path.Combine(ConfigurationManager.UploadsPath, fabric.ImageUrl);
                    }

                    using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
                    {
                        PdfWriter.GetInstance(doc1, fs);
                    }

                    return fullFilePath;
                }
            });
    }
}
