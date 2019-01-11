﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Restmium.ERP.BuildingBlocks.EventBus;
using Restmium.ERP.BuildingBlocks.EventBus.Abstractions;
using Restmium.ERP.BuildingBlocks.EventBusServiceBus;
using Restmium.ERP.Services.Warehouse.Application.Handlers.Integration;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using Restmium.ERP.Services.Warehouse.Integration.Events;

namespace Restmium.ERP.Services.Warehouse.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            //TODO: Add DbContextCheck
            services.AddHealthChecks();

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(this.Configuration.GetConnectionString("DefaultMSSQLConnection"), opt => opt.EnableRetryOnFailure());
            });

            #region EventBus
            bool isServiceBusEnabled = this.Configuration.GetValue("AzureServiceBusEnabled", false);

            if (isServiceBusEnabled)
            {
                services.AddSingleton<IServiceBusPersistentConnection>(sp =>
                {
                    ILogger<DefaultServiceBusPersisterConnection> logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                    string serviceBusConnectionString = this.Configuration.GetConnectionString("AzureServiceBusTopicConnection");
                    ServiceBusConnectionStringBuilder serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
                });
                this.RegisterEventBus(services);
            }
            #endregion

            services.AddSwaggerGen(swagger => {
                swagger.SwaggerDoc(
                    "2019.1.1",
                    new Swashbuckle.AspNetCore.Swagger.Info()
                    {
                        Title = "Warehouse.API",
                        Version = "2019.1.1"
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/2019.1.1/swagger.json", "Warehouse.API 2019.1.1");
                c.RoutePrefix = string.Empty; //Make Swagger as default Index
                c.DocumentTitle = "Warehouse.API Documentation";
            });

            if (this.Configuration.GetValue("AzureServiceBusEnabled", false))
            {
                this.ConfigureEventBus(app); //EventBus
            }
        }

        #region EventBus
        private void RegisterEventBus(IServiceCollection services)
        {
            string subscriptionClientName = this.Configuration["AzureSubscriptionClientName"];

            services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
            {
                IServiceBusPersistentConnection persistentConnection = sp.GetRequiredService<IServiceBusPersistentConnection>();
                ILogger<EventBusServiceBus> logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                IEventBusSubscriptionsManager subscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusServiceBus(sp, persistentConnection, logger, subscriptionManager, subscriptionClientName);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddTransient(sp => {
                DatabaseContext context = sp.GetRequiredService<DatabaseContext>();
                ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger = sp.GetRequiredService<ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>>();
                IEventBus serviceBus = sp.GetRequiredService<IEventBus>();

                return new OrderStatusChangedToAwaitingValidationIntegrationEventHandler(serviceBus, context, logger);
            }); // OrderStatusChangedToAwaitingValidationIntegrationEventHandler
            services.AddTransient(sp => {
                DatabaseContext context = sp.GetRequiredService<DatabaseContext>();
                ILogger<ProductAddedIntegrationEventHandler> logger = sp.GetRequiredService<ILogger<ProductAddedIntegrationEventHandler>>();
                return new ProductAddedIntegrationEventHandler(context, logger);
            }); // ProductAddedIntegrationEventHandler
            services.AddTransient(sp => {
                DatabaseContext context = sp.GetRequiredService<DatabaseContext>();
                ILogger<ProductRemovedIntegrationEventHandler> logger = sp.GetRequiredService<ILogger<ProductRemovedIntegrationEventHandler>>();
                return new ProductRemovedIntegrationEventHandler(context, logger);
            }); // ProductRemovedIntegrationEventHandler
            services.AddTransient(sp => {
                DatabaseContext context = sp.GetRequiredService<DatabaseContext>();
                ILogger<ProductRenamedIntegrationEventHandler> logger = sp.GetRequiredService<ILogger<ProductRenamedIntegrationEventHandler>>();
                return new ProductRenamedIntegrationEventHandler(context, logger);
            }); // ProductRenamedIntegrationEventHandler
        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            IEventBus eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<ProductAddedIntegrationEvent, ProductAddedIntegrationEventHandler>();
            eventBus.Subscribe<ProductRemovedIntegrationEvent, ProductRemovedIntegrationEventHandler>();
        }
        #endregion
    }
}
