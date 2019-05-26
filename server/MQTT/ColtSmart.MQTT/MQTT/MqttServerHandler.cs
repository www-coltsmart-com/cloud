using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ColtSmart.MQTT.MQTT
{
    public class MqttServerHandler : IMqttServerStartedHandler, IMqttServerStoppedHandler, IMqttServerClientConnectedHandler,
                                     IMqttServerClientDisconnectedHandler, IMqttServerClientSubscribedTopicHandler,
                                     IMqttApplicationMessageReceivedHandler
    {
        private IMqttServer mqttServer;
        private ICollection<string> topicFilter = new List<string> { "device/#" };

        public MqttServerHandler(IMqttServer mqttServer)
        {
            this.mqttServer = mqttServer;
        }

        /// <summary>
        /// 绑定消息接收事件
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            Console.WriteLine("收到来自客户端" +eventArgs.ClientId + "，主题为" + eventArgs.ApplicationMessage.Topic + "的消息：" + Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload));
           

            return Task.CompletedTask;
        }

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
