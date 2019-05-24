using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ColtSmart.Data
{

    /// <summary>
    /// 数据表信息
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tablenameMapper"></param>
        public TableInfo(Type type)
        {
            ClassType = type;
            
                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableAttr = type
                .GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;

                if (tableAttr != null)
                {
                    TableName = tableAttr.Name;
                    if ((!(tableAttr is TableAttribute)) && tableAttr.Schema != null)
                    {
                        SchemaName = tableAttr.Schema;
                    }
                }
                else
                {
                    TableName = type.Name;
                    if (type.IsInterface() && TableName.StartsWith("I"))
                        TableName = TableName.Substring(1);
                }
            

            ColumnInfos = type.GetProperties()
                .Where(t => t.GetCustomAttributes(typeof(IgnoreAttribute), false).Count() == 0)
                .Select(t =>
                {
                    var columnAtt = t.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "ColumnAttribute") as dynamic;

                    var ci = new ColumnInfo
                    {
                        Property = t,
                        ColumnName = columnAtt?.Name ?? t.Name,
                        PropertyName = t.Name,
                        IsKey = t.GetCustomAttributes(true).Any(a => a is KeyAttribute),
                        IsIdentity = t.GetCustomAttributes(true).Any(a => a is DatabaseGeneratedAttribute
                          && (a as DatabaseGeneratedAttribute).DatabaseGeneratedOption == DatabaseGeneratedOption.Identity),
                        IsGenerated = t.GetCustomAttributes(true).Any(a => a is DatabaseGeneratedAttribute
                            && (a as DatabaseGeneratedAttribute).DatabaseGeneratedOption != DatabaseGeneratedOption.None),
                        ExcludeOnSelect = t.GetCustomAttributes(true).Any(a => a is IgnoreSelectAttribute)
                    };

                    ci.ExcludeOnInsert = ci.IsGenerated
                        || t.GetCustomAttributes(true).Any(a => a is IgnoreInsertAttribute)
                        || t.GetCustomAttributes(true).Any(a => a is ReadOnlyAttribute);

                    ci.ExcludeOnUpdate = ci.IsGenerated
                        || t.GetCustomAttributes(true).Any(a => a is IgnoreUpdateAttribute)
                        || t.GetCustomAttributes(true).Any(a => a is ReadOnlyAttribute);

                    return ci;
                })
                .ToArray();

            if (!ColumnInfos.Any(k => k.IsKey))
            {
                var idProp = ColumnInfos.FirstOrDefault(p => string.Equals(p.PropertyName, "id", StringComparison.CurrentCultureIgnoreCase));

                if (idProp != null)
                {
                    idProp.IsKey = idProp.IsGenerated = idProp.IsIdentity = idProp.ExcludeOnInsert = idProp.ExcludeOnUpdate = true;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public Type ClassType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string SchemaName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerable<ColumnInfo> ColumnInfos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public ColumnInfo GetSingleKey(string method)
        {

            var keys = ColumnInfos.Where(p => p.IsKey).ToList();
            if (keys.Count() > 1)
                throw new DataException($"{method}<T> only supports an entity with a single [Key] or [ExplicitKey] property");

            return keys[0];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnInfo> InsertColumns => ColumnInfos.Where(ci => !ci.ExcludeOnInsert);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnInfo> UpdateColumns => ColumnInfos.Where(ci => !ci.ExcludeOnUpdate);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnInfo> SelectColumns => ColumnInfos.Where(ci => !ci.ExcludeOnSelect);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnInfo> KeyColumns => ColumnInfos.Where(ci => ci.IsKey);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<ColumnInfo> GeneratedColumns => ColumnInfos.Where(ci => ci.IsGenerated);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PropertyInfo> PropertyList => ColumnInfos.Select(ci => ci.Property);


        private static readonly ConcurrentDictionary<RuntimeTypeHandle, TableInfo> TableInfos = new ConcurrentDictionary<RuntimeTypeHandle, TableInfo>();

        public static TableInfo TableInfoCache(Type type)
        {
            if (TableInfos.TryGetValue(type.TypeHandle, out TableInfo ti))
            {
                return ti;
            }

            var tInfo = new TableInfo(type);
            TableInfos[type.TypeHandle] = tInfo;
            return tInfo;
        }

    }
}
