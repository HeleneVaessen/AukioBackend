using Microsoft.AspNetCore.Builder;

namespace Shared
{
    public static class BuilderExt
    {
        public static void Swagger(this IApplicationBuilder app, string apiName)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
