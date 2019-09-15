using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonREST.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackathonREST
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
            services.AddDbContext<AppointmentContext>(opt => opt.UseInMemoryDatabase("AppointmentList").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddDbContext<AppointmentResultContext>(opt => opt.UseInMemoryDatabase("AppointmentResultList"));
            services.AddDbContext<CenterContext>(opt => opt.UseInMemoryDatabase("CenterList"));
            services.AddDbContext<CenterTypeContext>(opt => opt.UseInMemoryDatabase("CenterTypeList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
