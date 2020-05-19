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
                "FROM [OptionGroups]" +
                "WHERE [OptionGroups].IsDeleted = 0").ToList();

        public List<OptionGroup> GetAllMapped() {
            List<OptionGroup> optionGroups = new List<OptionGroup>();

            _connection.Query<OptionGroup, OptionUnit, OptionGroup>(
                "SELECT * " +
                "FROM [OptionGroups] AS og " +
                "LEFT JOIN [OptionUnits] AS u ON u.[OptionGroupId] = og.Id " +
                "WHERE og.[IsDeleted] = 0 " +
                "ORDER BY ORDER BY og.Id, u.OrderIndex ",
                (optionGroup, optionUnit) => {
                    if (optionGroups.Any(x => x.Id == optionGroup.Id)) {
                        optionGroup = optionGroups.First(x => x.Id == optionGroup.Id);
                    } else {
                        optionGroups.Add(optionGroup);
                    }

                    if (optionUnit != null) {
                        optionGroup.OptionUnits.Add(optionUnit);
                    }

                    return optionGroup;
                });

            return optionGroups;
        }

        public OptionGroup GetById(long id) =>
            _connection.Query<OptionGroup>(
                "SELECT * " +
                "FROM OptionGroups " +
                "WHERE Id = @Id",
                new { Id = id }).SingleOrDefault();

        public OptionGroup NewOptionGroup(OptionGroup optionGroup) =>
            _connection.Query<OptionGroup>(
                "INSERT INTO[OptionGroups](IsDeleted,[Name],[IsMandatory]) " +
                "VALUES(0,@Name,@IsMandatory) " +
                "SELECT[OptionGroups].* " +
                "FROM [OptionGroups] " +
                "WHERE [OptionGroups].Id = SCOPE_IDENTITY()", optionGroup)
            .SingleOrDefault();

        public int UpdateOptionGroup(OptionGroup optionGroup) =>
            _connection.Execute(
                "UPDATE OptionGroups " +
                "SET IsDeleted = @IsDeleted, Name=@Name,IsMandatory=@IsMandatory, " +
                "LastModified = getutcdate() " +
                "WHERE [OptionGroups].Id = @Id;"
                , optionGroup);
    }
}
