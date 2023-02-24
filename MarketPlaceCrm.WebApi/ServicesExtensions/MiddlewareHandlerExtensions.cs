using MarketPlaceCrm.WebApi.SignalrHubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MarketPlaceCrm.WebApi.ServicesExtensions
{
    public static class MiddlewareHandlerExtensions
    {
        public static IApplicationBuilder UseMiddlewareHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(Constants.CorsPolicy);
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "MarketPlaceCrm");
                o.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CustomHub>("/customHub");
                endpoints.MapHub<GroupsHub>("/groups");
                endpoints.MapControllerRoute("Test", "api/{controller}/send");
            });
            
            return app;
        }
    }
}