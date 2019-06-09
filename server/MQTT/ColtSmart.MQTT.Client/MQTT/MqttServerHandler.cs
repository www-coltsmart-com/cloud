using ColtSmart.MQTT.Client.Entities;
using ColtSmart.Service;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColtSmart.MQTT.Client
{
    public class MqttServerHandler : IMqttApplicationMessageReceivedHandler, IMqttClientDisconnectedHandler, 
                                     IConnectingFailedHandler, IMqttClientConnectedHandler
    {
        private IDeviceService deviceService;

        public MqttServerHandler(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
        }

        #region[处理接收消息]
        /// <summary>
        /// 绑定消息接收事件
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            //var recvMsg = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            eventArgs.ProcessingFailed=await this.ProcessReceiveMessage(eventArgs.ApplicationMessage.Topic, eventArgs.ApplicationMessage.Payload);
        }


        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="clientId">设备Id</param>
        /// <param name="reciveMsg">接收消息</param>
        /// <returns></returns>
        private async Task<bool> ProcessReceiveMessage(string topic,byte[] payload)
        {
            var processingFailed = true;

            try
            {
                if (topic.EndsWith("/connected"))
                {
                    var deviceId = "";

                    if (!string.IsNullOrWhiteSpace(deviceId))
                    {
                        await deviceService.UpdateOnline(deviceId, true);
                    }
                }
                else if (topic.EndsWith("/disconnected"))
                {
                    var deviceId = "";

                    if (!string.IsNullOrWhiteSpace(deviceId))
                    {
                        await this.deviceService.UpdateOnline(deviceId, false);
                    }
                }
                else
                {
                    #region
                    var tInFo = GetRecvMsgType(topic);

                    switch (tInFo.Item1)
                    {
                        case "info":
                            {
                                var recvMsg = Encoding.UTF8.GetString(payload);
                                var dSetup = JsonHelper.DeserializeObject<DeviceSetup>(recvMsg);

                                if (!string.IsNullOrWhiteSpace(tInFo.Item2))
                                {
                                    await this.ProcessDeviceSetup(dSetup.DeviceId, tInFo.Item2, dSetup);
                                    processingFailed = false;
                                }
                            }
                            break;
                        case "data":
                            {
                                var netFlow= Math.Round((payload.Length * 1.0) / 1024, 2);
                                await this.deviceService.UpdateDeviceNet(tInFo.Item3, netFlow); 
                            }
                            break;
                        case "offline":
                            {
                                var recvMsg = Encoding.UTF8.GetString(payload);
                                var deviceId = JsonHelper.DeserializeObject<string>(recvMsg);
                                await this.deviceService.UpdateOnline(deviceId, false);
                            }
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}；{ex.StackTrace}");
            }

            return processingFailed;
        }

        /// <summary>
        /// 获取执行类型
        /// </summary>
        /// <returns></returns>
        private Tuple<string,string,string> GetRecvMsgType(string topic)
        {
            var pType = "";
            var ownUser = "";
            var deviceId = "";

            if (topic.EndsWith("/info"))
            {
                pType = "info";
            }else if (topic.EndsWith("/data"))
            {
                pType = "data";

            }else if (topic.EndsWith("/offline"))
            {
                pType = "offline";
            }

            var tSpilt= topic.Split('/').ToList();
            ownUser = tSpilt.Count > 1 ? tSpilt[1] : "";
            deviceId = tSpilt.Count > 3 ? tSpilt[3] : "";

            return new Tuple<string, string,string>(pType,ownUser,deviceId.Trim());
        }

        /// <summary>
        /// 处理机器开机
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="deviceOwn">设备所有人</param>
        /// <param name="deviceSetup">设备信息</param>
        private async Task ProcessDeviceSetup(string deviceId,string deviceOwn,DeviceSetup deviceSetup)
        {
            var device=  new Entity.Device
            {
                #region
                ComPortNum = deviceSetup.ComPortNum,
                DeviceName = deviceId,
                DeviceType = deviceSetup.DevType,
                InDate = DateTime.Now,
                DeviceId = deviceId,
                IsGetway = deviceSetup.IsGateway,
                IsOnline = true,
                Gps = deviceSetup.GPS,
                UserOwn = deviceOwn,
                Version = deviceSetup.Version
                #endregion
            };

            if (await this.deviceService.GetDevice(deviceId) == null)
            {
                await this.deviceService.Insert(device);
            }
            else
            {
                device.IsOnline = true;
                await this.deviceService.Update(device);
            }
        }

        #endregion


        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            Console.WriteLine("MQTT断开连接");
            return Task.CompletedTask;
        }

        public Task HandleConnectingFailedAsync(ManagedProcessFailedEventArgs eventArgs)
        {
            Console.WriteLine("MQTT连接失败" + eventArgs.Exception == null ? "" : eventArgs.Exception.Message + "；" + eventArgs.Exception.StackTrace);
            return Task.CompletedTask;
        }

        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("MQTT连接成功!");
            return Task.CompletedTask;
        }
    }
}
