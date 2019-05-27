using ColtSmart.Data;
using System;

namespace ColtSmart.Entity
{
    /// <summary>
    /// 设备类
    /// </summary>
    public class Device
    {
        [Key]
        public int Id { get; set; }

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
        public string Version { get; set; }
        /// <summary>
        /// 串行端口号
        /// </summary>
        public int ComPortNum { get; set; }
    }
}
