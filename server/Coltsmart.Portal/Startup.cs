using Autofac;
using Autofac.Extensions.DependencyInjection;
using ColtSmart.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Coltsmart.NetCore.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ColtSmart.Core;
using ColtSmart.Service.Impl;
using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ColtSmart.MQTT;
using ColtSmart;

namespace coltsmart.server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Policy",b =>
                {
                    b.WithOrigins("*");
                    b.WithHeaders("*");
                    b.WithMethods("*");
                });
            });
            services.AddMemoryCache();
            services.AddMvc(options => options.Conventions.Add(new WebApiOverloadingApplicationModelConvention()))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(op => op.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
            services.AddAuthentication().AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtManager.Secret)),
                };
            });

            services.AddColtSmartMQTT(Configuration);

            ConfigurationVariables.Default = Configuration.BuildConfigurationVariables();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterDbExecutor(b => b.UsePostgre().UseConnectionString(ConfigurationVariables.Default.ConnectionString));
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(DeviceService).Assembly).AsImplementedInterfaces();

            this.ApplicationContainer = builder.Build();
            
            return new AutofacServiceProvider(this.ApplicationContainer);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGlobalAppServiceProvider(Configuration);
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;

                var result = JsonConvert.ToJson(exception);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseColtSmartMQTT();
            
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            });
        }
    }
}
