using System.Linq;
using FluentValidation.AspNetCore;
using Kasp.FormBuilder.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kasp.FormBuilder.FluentValidation.Tests {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		private IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddMvc()
				.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining(typeof(Startup)))
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddFormBuilder();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			app.UseMvc();
		}
	}
}