using ColtSmart.Data;
using System;

namespace ColtSmart.Entity
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        public string DeviceId { get; set; }

        public string DeviceType { get; set; }

        public bool IsGetway { get; set; }

        public string DeviceName { get; set; }

        public bool IsOnline{get;set;}

		public DateTime InDate { get; set; }

        public string UserOwn { get; set; }

        public string NetFlow { get; set; }

        public string GpsCoordinate { get; set; }
    }
}
