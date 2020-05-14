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

        public OptionGroup GetById(long id) =>
            _connection.Query<OptionGroup>(
                "SELECT * " +
                "FROM OptionGroups " +
                "WHERE Id = @Id",
                new { Id = id }).SingleOrDefault();

        public OptionGroup NewOptionGroup(OptionGroup optionGroup) =>
            _connection.Query<OptionGroup>(
                "INSERT INTO[OptionGroups](IsDeleted,[Name]) " +
                "VALUES(0,@Name) " +
                "SELECT[OptionGroups].* " +
                "FROM [OptionGroups] " +
                "WHERE [OptionGroups].Id = SCOPE_IDENTITY()", optionGroup)
            .SingleOrDefault();

        public int UpdateOptionGroup(OptionGroup optionGroup) =>
            _connection.Execute(
                "UPDATE OptionGroups " +
                "SET IsDeleted = @IsDeleted, Name=@Name, " +
                "LastModified = getutcdate() " +
                "WHERE [OptionGroups].Id = @Id;"
                , optionGroup);
    }
}
