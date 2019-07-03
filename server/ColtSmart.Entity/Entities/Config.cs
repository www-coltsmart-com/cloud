using ColtSmart.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColtSmart.Entity.Entities
{
    /// <summary>
    /// 系统配置信息
    /// </summary>
    public class Config
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        /// <summary>
        /// 系统唯一标识，用于程序调用，必须英文，规范命名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 显示名称，用于方便维护
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值，内容格式不限制
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 说明，用于解释用途和修改方式
        /// </summary>
        public string Description { get; set; }
    }
}
