﻿using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.Repositories.Fabrics.Contracts;
using MMSL.Services.FabricServices.Contracts;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MMSL.Services.FabricServices {
    public class FabricService : IFabricService {

        private readonly IFabricRepositoriesFactory _fabricRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public FabricService(IFabricRepositoriesFactory fabricRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
            _fabricRepositoriesFactory = fabricRepositoriesFactory;
        }

        public Task<Fabric> GetByIdAsync(long fabricId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     return _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetById(fabricId);
                 }
             });

        public Task<Fabric> AddFabric(NewFabricDataContract fabric, string imageUrl = null) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = fabric.GetEntity();

                    if (!string.IsNullOrEmpty(imageUrl))
                        fabricEntity.ImageUrl = imageUrl;

                    return repository.AddFabric(fabricEntity);
                }
            });

        public Task<Fabric> UpdateFabric(UpdateFabricDataContract fabric, string imageUrl = null) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = fabric.GetEntity();

                    if (!string.IsNullOrEmpty(imageUrl))
                        fabricEntity.ImageUrl = imageUrl;

                    return repository.UpdateFabric(fabricEntity);
                }
            });

        public Task<Fabric> DeleteFabric(long fabricId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = repository.GetById(fabricId);

                    if (fabricEntity == null)
                        throw new Exception("Fabric not found");

                    fabricEntity.IsDeleted = true;

                    return repository.UpdateFabric(fabricEntity);
                }
            });

        public Task<Fabric> UpdateFabricVisibilities(UpdateFabricVisibilitiesDataContract fabric) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = repository.GetById(fabric.Id);

                    fabricEntity = fabric.MapFabric(fabricEntity);

                    repository.UpdateFabric(fabricEntity);

                    return repository.GetById(fabricEntity.Id);
                }
            });
        }
}
