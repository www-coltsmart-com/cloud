using System.Collections.Generic;

namespace ColtSmart.Data
{
    public interface IPartsQryGenerator
    {
        string EscapeTableName(string value);
        string EscapeTableName(TableInfo tableInfo);
        string EscapeColumnn(string value);
        string EscapeParameter(string value);
        string EscapeColumnList(IEnumerable<ColumnInfo> columns, string tableName = null);
        string EscapeColumnListWithAliases(IEnumerable<ColumnInfo> columns, string tableName = null);
        string EscapeWhereList(IEnumerable<ColumnInfo> columns);
        string EscapeWhereList(IEnumerable<string> columns);
        string EscapeParameters(IEnumerable<ColumnInfo> columns, string suffix = "");
        string EscapeAssignmentList(IEnumerable<ColumnInfo> columns);

        string InsertQuery(TableInfo tableInfo);
        string UpdateQuery(TableInfo tableInfo, IEnumerable<string> columnsToUpdate);
        string DeleteQuery(TableInfo tableInfo);
        string GetQuery(TableInfo tableInfo, IEnumerable<string> columnsToWhere);
    }
}
