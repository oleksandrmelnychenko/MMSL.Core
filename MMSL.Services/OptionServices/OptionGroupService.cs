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
    }
}
