using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ColtSmart.Entity.Entities
{
    /// <summary>
    /// 产品属性表
    /// </summary>
    public class GoodsAttr
    {
        /// <summary>
        /// Id标识
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        /// <summary>
        /// 产品标识
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
