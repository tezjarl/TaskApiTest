using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using TaskApiTest.Models;
using TaskApiTest.Repositories.Implementations;
using TaskApiTest.Repositories.Interfaces;
using TaskApiTest.Services.Implementations;
using TaskApiTest.Services.Interfaces;

namespace TaskApiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<JobDbContext>(
                options => options.UseSqlServer(connectionString));
            services.AddScoped<IJobManager, JobManager>();
            services.AddSingleton<IRepository<Job>, JobRepository>();
            services.AddSingleton<IJobQueue, JobQueue>();
            services.AddHostedService<FakeJobWorker>();
            services.AddMvc()
                .AddJsonOptions(options =>
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Task API", Description = "This is simple task API"});
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task API");
            });
            app.UseMvc();
            
           
        }
    }
}