using ColtSmart.MQTT.Client.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using MQTTnet.AspNetCore;
using ColtSmart.Service;
using System.Threading.Tasks;

namespace ColtSmart.MQTT.Client
{
    public static class ColtSmartMQTTClientExtensions
    {
        public static IServiceCollection AddColtSmartMQTT(this IServiceCollection services, IConfiguration configuration)
        {
            var mqttOption = configuration.GetSection("MqttOption").Get<MqttOption>();
            var mqttClient = new MqttFactory().CreateManagedMqttClient();

            services.AddSingleton<IManagedMqttClient>(mqttClient);
            services.AddSingleton<MqttOption>(mqttOption);

            return services;
        }

        public static void UseColtSmartMQTT(this IApplicationBuilder app)
        {
            Task.Run(async () => { await StartMqttAsync(); });
            
        }

        private static async Task StartMqttAsync()
        {
            try
            {
                var mqttClient = EnjoyGlobals.ServiceProvider.GetService<IManagedMqttClient>();
                var deviceService = EnjoyGlobals.ServiceProvider.GetService<IDeviceService>();

                var options = new ManagedMqttClientOptionsBuilder().WithAutoReconnectDelay(TimeSpan.FromMinutes(1))
                                                                   .WithClientOptions(new MqttClientOptionsBuilder().WithClientId("coltsmart_cloud_admin")
                                                                                                                    .WithTcpServer("101.132.97.241", 1883)
                                                                                                                    .Build())
                                                                   .Build();

                await mqttClient.StartAsync(options);
                await mqttClient.SubscribeAsync(new List<TopicFilter>
                {
                    new TopicFilter{ Topic="device/*/com/*/info"},
                    new TopicFilter{ Topic="device/*/com/*/data"},
                });

                var mqttHandler= new MqttServerHandler(deviceService);
                mqttClient.ApplicationMessageReceivedHandler= mqttHandler;
                mqttClient.DisconnectedHandler = mqttHandler;
                mqttClient.ConnectingFailedHandler = mqttHandler;
                mqttClient.ConnectedHandler = mqttHandler;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}；{ex.StackTrace}");
            }
        }
    }
}
