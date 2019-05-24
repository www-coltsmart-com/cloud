using System.Reflection;

namespace ColtSmart.Data
{
    public class ColumnInfo
    {
        /// <summary>
        /// column name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// entity property name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// primary key
        /// </summary>
        public bool IsKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGenerated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ExcludeOnInsert { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ExcludeOnUpdate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ExcludeOnSelect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo Property { get; set; }
    }
}
