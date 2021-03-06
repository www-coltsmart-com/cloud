﻿using ColtSmart.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColtSmart.Entity
{
    /// <summary>
    /// 设备类
    /// </summary>
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string DeviceId { get; set; }

        public string DeviceType { get; set; }

        public bool IsGetway { get; set; }

        public string DeviceName { get; set; }

        public bool IsOnline { get; set; }

        public DateTime InDate { get; set; }

        public string UserOwn { get; set; }

        public string Gps { get; set; }
        /// <summary>
        /// 设备版本
        /// </summary>
        [Ignore]
        public string Version { get; set; }
        /// <summary>
        /// 串行端口号
        /// </summary>
        [Ignore]
        public int ComPortNum { get; set; }
    }
}
