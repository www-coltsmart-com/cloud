using ColtSmart.MQTT.Client.Options;
using ColtSmart.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Diagnostics;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Threading.Tasks;

namespace ColtSmart.MQTT.Client
{
    public static class ColtSmartMQTTClientExtensions
    {
        public static IServiceCollection AddColtSmartMQTT(this IServiceCollection services, IConfiguration configuration)
        {
            var mqttOption = configuration.GetSection("MqttOption").Get<MqttOption>();
            var mqttClient = new MqttFactory().CreateMqttClient();

            services.AddSingleton<IMqttClient>(mqttClient);
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
                var mqttClient = EnjoyGlobals.ServiceProvider.GetService<IMqttClient>();
                var deviceService = EnjoyGlobals.ServiceProvider.GetService<IDeviceService>();
                var mqttOption = EnjoyGlobals.ServiceProvider.GetService<MqttOption>();

                var options = new MqttClientOptionsBuilder().WithCommunicationTimeout(TimeSpan.FromMinutes(1))
                                                          .WithClientId("coltsmart_cloud_admin")
                                                          .WithTcpServer(mqttOption.HostIp, mqttOption.HostPort)
                                                          .WithCleanSession(true)
                                                          .Build();

                MqttNetGlobalLogger.LogMessagePublished += (s, e) =>
                {
                    var trace = $">> [{e.TraceMessage.Timestamp:O}] [{e.TraceMessage.ThreadId}] [{e.TraceMessage.Source}] [{e.TraceMessage.Level}]: {e.TraceMessage.Message}";

                    if (e.TraceMessage.Exception != null)
                    {
                        trace += Environment.NewLine + e.TraceMessage.Exception.ToString();
                    }

                    Console.WriteLine(trace);
                };

                await mqttClient.ConnectAsync(options);
                await mqttClient.SubscribeAsync("device/#");

                var mqttHandler= new MqttServerHandler(deviceService);
                mqttClient.ApplicationMessageReceivedHandler= mqttHandler;
                mqttClient.DisconnectedHandler = mqttHandler;
                
                mqttClient.ConnectedHandler = mqttHandler;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}；{ex.StackTrace}");
            }
        }
    }
}
