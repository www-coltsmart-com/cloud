using ColtSmart.MQTT.MQTT;
using ColtSmart.MQTT.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ColtSmart.MQTT
{
    public static class ColtSmartMQTTExtensions
   {
        public static IServiceCollection AddColtSmartMQTT(this IServiceCollection services, IConfiguration configuration)
        {
            var mqttOption = configuration.GetSection("MqttOption").Get<MqttOption>();

            var optionBuilder = new MqttServerOptionsBuilder().WithDefaultEndpointBoundIPAddress(System.Net.IPAddress.Parse(mqttOption.HostIp))
                                                              .WithDefaultEndpointPort(mqttOption.HostPort)
                                                              .WithDefaultCommunicationTimeout(TimeSpan.FromMilliseconds(mqttOption.Timeout))
                                                              .WithEncryptedEndpoint()
                                                              .WithConnectionValidator(t =>
                                                              {
                                                                  if (t.Username != mqttOption.UserName || t.Password != mqttOption.Password)
                                                                  {
                                                                      t.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                                                                  }
                                                                  else
                                                                  {
                                                                      t.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
                                                                  }
                                                              });
            
            
            var options = optionBuilder.Build();

            var certificate = new X509Certificate2(@"C:\certs\test\test.cer", "", X509KeyStorageFlags.Exportable);
            options.TlsEndpointOptions.Certificate = certificate.Export(X509ContentType.Pfx);
            options.TlsEndpointOptions.IsEnabled = true;

            services.AddHostedMqttServer(options)
                    .AddMqttConnectionHandler()
                    .AddConnections();

            return services;
        }

        public static void UseColtSmartMQTT(this IApplicationBuilder app)
        {
            app.UseConnections(c => c.MapConnectionHandler<MqttConnectionHandler>("/data", options =>
            {
                options.WebSockets.SubProtocolSelector = MQTTnet.AspNetCore.ApplicationBuilderExtensions.SelectSubProtocol;
            }));

            app.UseMqttEndpoint("/data");
            app.UseMqttServer(server => 
             {
                 var mqttHandler = new MqttServerHandler();

                 server.StartedHandler = mqttHandler;
                 server.StoppedHandler = mqttHandler;
                 server.ClientConnectedHandler = mqttHandler;
                 server.ClientDisconnectedHandler = mqttHandler;
                 server.ClientSubscribedTopicHandler = mqttHandler;
                 server.ApplicationMessageReceivedHandler = mqttHandler;
             }); 
        }
   }
}
