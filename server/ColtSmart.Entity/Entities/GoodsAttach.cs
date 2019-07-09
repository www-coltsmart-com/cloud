using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ColtSmart.Entity.Entities
{
    /// <summary>
    /// 产品附件
    /// </summary>
    public class GoodsAttach
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
        /// 附件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 附件文件格式
        /// </summary>
        public string Ext { get; set; }
        /// <summary>
        /// 附件文件大小(单位：KB)
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string Path { get; set; }
    }
}
