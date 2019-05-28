using ColtSmart.MQTT.Entities;
using ColtSmart.Service;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ColtSmart.MQTT.MQTT
{
    public class MqttServerHandler : IMqttServerStartedHandler, IMqttServerStoppedHandler, IMqttServerClientConnectedHandler,
                                     IMqttServerClientDisconnectedHandler, IMqttServerClientSubscribedTopicHandler,
                                     IMqttApplicationMessageReceivedHandler
    {
        private IMqttServer mqttServer;
        private IDeviceService deviceService;
        private ICollection<string> topicFilter = new List<string> { "device/#" };

        public MqttServerHandler(IMqttServer mqttServer, IDeviceService deviceService)
        {
            this.mqttServer = mqttServer;
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
            var recvMsg = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            Console.WriteLine("收到来自客户端" +eventArgs.ClientId + "，主题为" + eventArgs.ApplicationMessage.Topic + "的消息：" + recvMsg);

            eventArgs.ProcessingFailed=await this.ProcessReceiveMessage(eventArgs.ApplicationMessage.Topic, eventArgs.ClientId, recvMsg);
        }


        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="clientId">设备Id</param>
        /// <param name="reciveMsg">接收消息</param>
        /// <returns></returns>
        private async Task<bool> ProcessReceiveMessage(string topic,string clientId,string reciveMsg)
        {
            var processingFailed = true;

            try
            {
                var tInFo= GetRecvMsgType(topic);

                switch (tInFo.Item1)
                {
                    case "info":
                        var dSetup =JsonHelper.DeserializeObject<DeviceSetup>(reciveMsg);

                        if (!string.IsNullOrWhiteSpace(tInFo.Item2))
                        {
                          await this.ProcessDeviceSetup(clientId, tInFo.Item2, dSetup);
                            processingFailed = false;
                        }
                        break;
                    case "data":
                        break;
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}；{ex.StackTrace}");
            }

            return processingFailed;
        }

        /// <summary>
        /// 获取执行类型
        /// </summary>
        /// <returns></returns>
        private Tuple<string,string> GetRecvMsgType(string topic)
        {
            var pType = "";
            var ownUser = "";

            if (topic.EndsWith("/info"))
            {
                pType = "info";
            }else if (topic.EndsWith("/data"))
            {
                pType = "data";
            }

            var tSpilt= topic.Split('/').ToList();
            ownUser = tSpilt.Count > 1 ? tSpilt[1] : "";

            return new Tuple<string, string>(pType,ownUser);
        }

        /// <summary>
        /// 处理机器开机
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="deviceOwn">设备所有人</param>
        /// <param name="deviceSetup">设备信息</param>
        private async Task ProcessDeviceSetup(string deviceId,string deviceOwn,DeviceSetup deviceSetup)
        {
            if (await this.deviceService.GetDevice(deviceId,deviceOwn) == null)
            {
                await this.deviceService.Insert(new Entity.Device
                {
                    #region
                    ComPortNum = deviceSetup.ComPortNum,
                    DeviceName =deviceId,
                    DeviceType =deviceSetup.DevType,
                    InDate =DateTime.Now,
                    DeviceId =deviceId,
                    IsGetway =deviceSetup.IsGateway,
                    IsOnline =true,
                    Gps =deviceSetup.GPS,
                    UserOwn =deviceOwn,
                    Version =deviceSetup.Version
                    #endregion
                });
            }
        }
        #endregion

        /// <summary>
        /// 处理客户端客户端连接
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine($"{eventArgs.ClientId}:建立MQTT连接！");

            return Task.CompletedTask;
            //await this.mqttServer.SubscribeAsync(eventArgs.ClientId, null);
        }

        /// <summary>
        /// 处理客户端端口连接
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            Console.WriteLine($"{eventArgs.ClientId}:断开MQTT连接！");
            return Task.CompletedTask;
            //await this.mqttServer.UnsubscribeAsync(eventArgs.ClientId, this.topicFilter);
        }

        /// <summary>
        /// 订阅客户端发送过来的消息
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
        {
            Console.WriteLine($"{eventArgs.ClientId}:订阅主题；{eventArgs.TopicFilter.Topic}");
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// Mqtt服务器启动
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task HandleServerStartedAsync(EventArgs eventArgs)
        {
            Console.WriteLine($"MQTT服务启动！");

            return Task.CompletedTask;
        }

        /// <summary>
        /// 处理服务器停止
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task HandleServerStoppedAsync(EventArgs eventArgs)
        {
            Console.WriteLine($"MQTT服务停止！");

            return Task.CompletedTask;
        }
    }
}
