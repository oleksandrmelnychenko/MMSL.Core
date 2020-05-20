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

        public List<OptionGroup> GetAll(string search) =>
            _connection.Query<OptionGroup>(
                "SELECT * " +
                "FROM [OptionGroups]" +
                "WHERE [OptionGroups].IsDeleted = 0 " +
                (string.IsNullOrEmpty(search) ? string.Empty : "AND PATINDEX('%' + @Search + '%', og.[Name]) > 0")).ToList();

        public List<OptionGroup> GetAllMapped(string search) {
            List<OptionGroup> optionGroups = new List<OptionGroup>();

            _connection.Query<OptionGroup, OptionUnit, OptionGroup>(
                "SELECT * " +
                "FROM [OptionGroups] AS og " +
                "LEFT JOIN [OptionUnits] AS u ON u.[OptionGroupId] = og.Id AND u.IsDeleted = 0 " +
                "WHERE og.[IsDeleted] = 0 " +
                (string.IsNullOrEmpty(search) ? string.Empty : "AND PATINDEX('%' + @Search + '%', og.[Name]) > 0") +
                "ORDER BY og.Id, u.OrderIndex ",
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
                },
                new { Search = search });

            return optionGroups;
        }

        public OptionGroup GetById(long id) {
            OptionGroup groupResult = null;

            _connection.Query<OptionGroup, OptionUnit, OptionGroup>(
                "SELECT OptionGroups.*, [OptionUnits].* " +
                "FROM OptionGroups " +
                "LEFT JOIN [OptionUnits] ON [OptionUnits].OptionGroupId = [OptionGroups].Id AND [OptionUnits].IsDeleted = 0" +
                "WHERE [OptionGroups].Id = @Id AND [OptionGroups].IsDeleted = 0 " +
                "ORDER BY [OptionUnits].OrderIndex",
                (group, unit) => {
                    if (groupResult == null) {
                        groupResult = group;
                    }

                    if (unit != null) {
                        groupResult.OptionUnits.Add(unit);
                    }

                    return group;
                },
                new { Id = id });

            return groupResult;
        }

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
