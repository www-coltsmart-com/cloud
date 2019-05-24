using ColtSmart.Entity;
using ColtSmart.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coltsmart.Portal.Controllers
{
    public class DeviceController : ApiController
    {
        [HttpGet]
        [Route("api/devices")]
        public PagedResult<Device> Get(int page, int size, string devicename)
        {
            var pr= new PagedResult<Device>
            {
                CurrentPage = page,
                PageSize = size,
                TotalCount = Devices.Count
            };

            pr.Result = Devices.Skip((page - 1) * size).Take(size);

            return pr;
        }

        [HttpDelete]
        [Route("api/devices/{id}")]
        public int Delete(int id)
        {
            return 0;
        }

        public static List<Device> Devices = new List<Device>
                {
                     new Device
                     {
                         Id=100,
                         DeviceId ="2019042300000001",
                         DeviceType ="MJ-4GDTU",
                         IsGetway=false,
                         DeviceName="终端001",
                         IsOnline=true,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="刘晓东",
                         NetFlow="10.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     },
                     new Device
                     {
                         Id=101,
                         DeviceId ="2019042300000002",
                         DeviceType ="MJ-LoRaDTU",
                         IsGetway=true,
                         DeviceName="终端002",
                         IsOnline=false,
                         InDate=System.DateTime.Now.Date,
                         UserOwn="马威",
                         NetFlow="30.1KB",
                         GpsCoordinate="N/A"
                     }
                };
    
    }
}
