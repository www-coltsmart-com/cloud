using System;
using System.Collections.Generic;
using System.Text;

namespace ColtSmart.Data.Adapter
{
   public class PostgrePartsQryGenerator: PartsQryGenerator
    {
        public override string EscapeTableName(TableInfo tableInfo) =>
          (!string.IsNullOrEmpty(tableInfo.SchemaName) ? EscapeTableName(tableInfo.SchemaName) + "." : null) + EscapeTableName(tableInfo.TableName);

        /// <summary>
        /// Returns the format for table name
        /// </summary>
        public override string EscapeTableName(string value) => $"\"{value.ToLower()}\"";

        /// <summary>
        /// Returns the format for column
        /// </summary>
        public override string EscapeColumnn(string value) => $"\"{value}\"";
    }
}
