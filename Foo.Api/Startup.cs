using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(o =>
                {
                    o.Authority = "https://demo.identityserver.io";
                    o.ApiName = "api";
                });
            services.AddAuthorization(o => o.AddPolicy("MyPolicy", Policies.MyPolicy()));
            services.AddTransient<IAuthorizationHandler, RegularClientAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, ShortClientAuthorizationHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
