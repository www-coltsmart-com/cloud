using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ColtSmart.Data.Adapter
{
    public class PartsQryGenerator : IPartsQryGenerator
    {
        /// <summary>
        /// Cache for Get Queries
        /// </summary>
        protected static readonly ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        /// <summary>
        /// Cache for Insert Queries
        /// </summary>
        protected static readonly ConcurrentDictionary<RuntimeTypeHandle, string> InsertQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        /// <summary>
        /// Cache for Update Queries
        /// </summary>
        protected static readonly ConcurrentDictionary<RuntimeTypeHandle, string> UpdateQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        /// <summary>
        /// Cache for Delete Queries
        /// </summary>
        protected static readonly ConcurrentDictionary<RuntimeTypeHandle, string> DeleteQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        public virtual string EscapeAssignmentList(IEnumerable<ColumnInfo> columns) 
            => string.Join(", ", columns.Select(ci => $"{EscapeColumnn(ci.ColumnName)} = {EscapeParameter(ci.PropertyName)}"));

        public virtual string EscapeColumnList(IEnumerable<ColumnInfo> columns, string tableName = null)
            => string.Join(", ", columns.Select(ci => (tableName != null ? EscapeTableName(tableName) + "." : "") + EscapeColumnn(ci.ColumnName)));

        public virtual string EscapeColumnListWithAliases(IEnumerable<ColumnInfo> columns, string tableName = null)
            => string.Join(", ", columns.Select(ci => (tableName != null ? EscapeTableName(tableName) + "." : "") + EscapeColumnn(ci.ColumnName) + (ci.ColumnName != ci.PropertyName ? " AS " + EscapeColumnn(ci.PropertyName) : "")));

        public virtual string EscapeColumnn(string value)
            => $"[{value}]";

        public virtual string EscapeParameter(string value)
            => $"@{value}";

        public virtual string EscapeParameters(IEnumerable<ColumnInfo> columns, string suffix = "")
            => string.Join(", ", columns.Select(ci => EscapeParameter(ci.PropertyName + suffix)));

        public virtual string EscapeTableName(string value)
            => $"[{value}]";

        public virtual string EscapeTableName(TableInfo tableInfo)
            => EscapeTableName(tableInfo.TableName);

        public virtual string EscapeWhereList(IEnumerable<ColumnInfo> columns)
            => string.Join(" and ", columns.Select(ci => $"{EscapeColumnn(ci.ColumnName)} = {EscapeParameter(ci.PropertyName)}"));

        public virtual string EscapeWhereList(IEnumerable<string> columns)
          => string.Join(" and ", columns.Select(ci => $"{EscapeColumnn(ci)} = {EscapeParameter(ci)}"));
        
        public virtual string GetQuery(TableInfo tableInfo, IEnumerable<string> columnsToWhere)
        {
            return GetQueries.Acquire(
                    tableInfo.ClassType.TypeHandle,
                    () => true,
                    () =>
                    {
                        var wc = columnsToWhere != null && columnsToWhere.Any() ? $"where {EscapeWhereList(columnsToWhere)}" : "";
                        return $"select {EscapeColumnListWithAliases(tableInfo.SelectColumns, tableInfo.TableName)} from { EscapeTableName(tableInfo)} {wc}";
                    }
                );
        }

        public virtual string InsertQuery(TableInfo tableInfo)
        {
            return InsertQueries.Acquire(
              tableInfo.ClassType.TypeHandle,
              () => true,
              () => $"insert into { EscapeTableName(tableInfo)} ({EscapeColumnList(tableInfo.InsertColumns)}) values ({EscapeParameters(tableInfo.InsertColumns)}) "
            );
        }

        public virtual string UpdateQuery(TableInfo tableInfo, IEnumerable<string> columnsToUpdate)
        {
            return UpdateQueries.Acquire(
                tableInfo.ClassType.TypeHandle,
                () => columnsToUpdate == null || !columnsToUpdate.Any(),
                () =>
                {
                    var updates = tableInfo.UpdateColumns.Where(ci => (columnsToUpdate == null || !columnsToUpdate.Any() || columnsToUpdate.Contains(ci.PropertyName)));
                    return $"update {EscapeTableName(tableInfo)} set {EscapeAssignmentList(updates)} where {EscapeWhereList(tableInfo.KeyColumns)}";
                }
            );
        }

        public virtual string DeleteQuery(TableInfo tableInfo)
        {
            return DeleteQueries.Acquire(
                tableInfo.ClassType.TypeHandle,
                () => true,
                () =>
                {
                    return $"delete from {EscapeTableName(tableInfo)} where {EscapeWhereList(tableInfo.KeyColumns)}";
                }
            );
        }

    }
}
