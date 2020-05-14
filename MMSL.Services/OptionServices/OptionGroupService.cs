using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using MMSL.Services.OptionServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices {
    public class OptionGroupService : IOptionGroupService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IOptionRepositoriesFactory _optionRepositoriesFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connectionFactory"></param>
        /// <param name="optionRepositoriesFactory"></param>
        public OptionGroupService(IDbConnectionFactory connectionFactory, IOptionRepositoriesFactory optionRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _optionRepositoriesFactory = optionRepositoriesFactory;
        }

        public Task<List<OptionGroup>> GetOptionGroupsAsync() =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    List<OptionGroup> optionGroups = null;
                    IOptionGroupRepository optionGroupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);
                    optionGroups = optionGroupRepository.GetAll();
                    return optionGroups;
                }
            });

        public Task<OptionGroup> NewOptionGroupAsync(NewOptionGroupDataContract newOptionGroupDataContract) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     IOptionGroupRepository optionGroupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);

                     OptionGroup optionGroup = new OptionGroup {
                         Name = newOptionGroupDataContract.Name
                     };

                     optionGroup = optionGroupRepository.NewOptionGroup(optionGroup);

                     return optionGroup;
                 }
             });

        public Task UpdateOptionGroupAsync(OptionGroup optionGroup) =>
             Task.Run(() => {
                 using (var connection = _connectionFactory.NewSqlConnection()) {
                     IOptionGroupRepository optionGroupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);

                     OptionGroup existed = optionGroupRepository.GetById(optionGroup.Id);

                     if (existed != null) {
                         int rowAffected = optionGroupRepository.UpdateOptionGroup(optionGroup);
                     } else {
                         UserExceptionCreator<NotFoundValueException>.Create(NotFoundValueException.VALUE_NOT_FOUND).Throw();
                     }
                 }
             });

        public Task DeleteOptionGroupAsunc(long optionGroupId) =>
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

    }
}
