using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using SME.Models;
using SME.Persistence;
using SME.Services;
using Microsoft.AspNetCore.Hosting.Internal;

namespace SME
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;

        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<MongoSettings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Container = Configuration.GetSection("MongoDb:Container").Value;
                    options.Database = Configuration.GetSection("MongoDb:Database").Value;
                    options.IsDockerized = Configuration["DOTNET_RUNNING_IN_CONTAINER"] != null;
                    Console.WriteLine(Configuration["DOTNET_RUNNING_IN_CONTAINER"]);
                    Console.WriteLine("---------------------------------------------------");
                    Console.WriteLine(options.ConnectionString);
                    Console.WriteLine("---------------------------------------------------");
                    options.IsInDevelopment = HostingEnvironment.IsDevelopment();
                }
            );
            // services.Configure<Neo4jSettings>(
            //     options =>
            //     {
            //         options.ConnectionString = Configuration.GetSection("Neo4j:ConnectionString").Value;
            //         options.UserId = Configuration.GetSection("Neo4j:UserId").Value;
            //         options.Password = Configuration.GetSection("Neo4j:Password").Value;
            //     }
            // );
            services.AddSingleton<MongoDbConnection>();
            // services.AddSingleton<GraphDbConnection>();
            services.AddSingleton<RabbitMQConnection>();
            services.AddScoped<IResourceRepository, ResourceMongo>();
            services.AddScoped<IConceptRepository, ConceptMongo>();
            services.AddScoped<ITechnologyRepository, TechnologyMongo>();
            services.AddScoped<ILearningPlanRepository,LearningPlanMongo>();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "SME API",
                    Description = "This is a documentation for Learning Resource Input (SME) Module which lets a SME to add, modify and delete resources for our learning system",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "NoInterviewCorp",
                        Email = string.Empty,
                        Url = "https://github.com/NoInterviewCorp/"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RabbitMQConnection rmq)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SME V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
