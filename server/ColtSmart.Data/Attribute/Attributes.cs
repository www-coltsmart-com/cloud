using System;

namespace ColtSmart.Data
{
    /// <summary>
    /// Defines the name of a table to use in commands.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Creates a table mapping to a specific name 
        /// </summary>
        /// <param name="tableName">The name of this table in the database.</param>
        public TableAttribute(string tableName)
        {
            Name = tableName;
        }

        /// <summary>
        /// The name of the table in the database
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies that this field is a primary key in the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
    }

    /// <summary>
    /// Specifies that this field is a explicitly set primary key in the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExplicitKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// Specifies whether a field is writable in the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WriteAttribute : Attribute
    {
        /// <summary>
        /// Specifies whether a field is writable in the database.
        /// </summary>
        /// <param name="write">Whether a field is writable in the database.</param>
        public WriteAttribute(bool write)
        {
            Write = write;
        }

        /// <summary>
        /// Whether a field is writable in the database.
        /// </summary>
        public bool Write { get; }
    }

    /// <summary>
    /// Specifies that this is a computed column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedAttribute : Attribute
    {
    }

    /// <summary>
    /// Specifies whether a property should be completely ignored
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// Specifies whether a property should be completely ignored
        /// </summary>
        public IgnoreAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies whether a field is insertable in the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreInsertAttribute : Attribute
    {
        /// <summary>
        /// Specifies whether a field is insertable in the database.
        /// </summary>
        public IgnoreInsertAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies whether a field is updatable in the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreUpdateAttribute : Attribute
    {
        /// <summary>
        /// Specifies whether a field is updatable in the database.
        /// </summary>
        public IgnoreUpdateAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies whether a field should be returned from the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreSelectAttribute : Attribute
    {
        /// <summary>
        /// Specifies whether a field should be returned from the database.
        /// </summary>
        public IgnoreSelectAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies whether a field is read only (same as computed).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ReadOnlyAttribute : Attribute
    {
        /// <summary>
        /// Specifies whether a field is read only (same as computed).
        /// </summary>
        public ReadOnlyAttribute()
        {
        }
    }

}
