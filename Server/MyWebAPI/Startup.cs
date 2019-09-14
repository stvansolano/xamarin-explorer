using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyWebAPI.Security;

namespace MyWebAPI
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
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = false,
						ValidateIssuerSigningKey = false,
						ValidIssuer = Configuration[JwtTokenBuilder.CONFIGURATION_AUTHENTICATION_ISSUER_KEY],
						ValidAudience = Configuration[JwtTokenBuilder.CONFIGURATION_AUTHENTICATION_AUDIENCE_KEY],
						IssuerSigningKey = JwtTokenBuilder.JwtSecurityKey.Create(Configuration[JwtTokenBuilder.CONFIGURATION_AUTHENTICATION_SHARED_SECRET_KEY])
					};

					options.Events = new JwtBearerEvents
					{
						OnAuthenticationFailed = context =>
						{
							return Task.CompletedTask;
						},
						OnTokenValidated = context =>
						{
							return Task.CompletedTask;
						}
					};
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1" });
			});

			services.AddDbContext<AdventureWorks.SqlServer.Models.AdventureworksContext>(
				options => options.UseSqlServer(Configuration.GetConnectionString("AdventureWorksDatatabase")));
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
				app.UseHttpsRedirection();
			}
			app.UseAuthentication();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "api";
            });

            app.UseMvc();
		}
	}
}
