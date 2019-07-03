using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ColtSmart.Entity.Entities
{
    /// <summary>
    /// 产品信息表
    /// </summary>
    public class Goods
    {
        /// <summary>
        /// Id标识
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 状态（-1：已删除；0：未发布；1：已发布）
        /// </summary>
        public int Status { get; set; }
    }
}
