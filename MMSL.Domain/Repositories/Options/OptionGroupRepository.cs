using Dapper;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Options {
    public class OptionGroupRepository : IOptionGroupRepository {

        private IDbConnection _connection;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connection"></param>
        public OptionGroupRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<OptionGroup> GetAll() =>
            _connection.Query<OptionGroup>(
                "SELECT * " +
                "FROM [OptionGroups]").ToList();
    }
}
