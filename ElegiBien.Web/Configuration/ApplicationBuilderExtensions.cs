namespace ElegiBien.Web.Configuration;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureElegiBienPipeline(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseElegiBienSecurityHeaders();
        app.UseElegiBienSeoDocuments();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthorization();
        app.MapStaticAssets();

        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        return app;
    }

    private static IApplicationBuilder UseElegiBienSecurityHeaders(
        this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var headers = context.Response.Headers;
            headers.TryAdd("X-Content-Type-Options", "nosniff");
            headers.TryAdd("X-Frame-Options", "DENY");
            headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
            headers.TryAdd(
                "Permissions-Policy",
                "camera=(), microphone=(), geolocation=(), payment=(), usb=()");
            headers.TryAdd("Cross-Origin-Opener-Policy", "same-origin");
            headers.TryAdd("Cross-Origin-Resource-Policy", "same-origin");

            var csp = string.Join(' ',
       "default-src 'self';",
       "base-uri 'self';",
       "object-src 'none';",
       "frame-ancestors 'none';",
       "form-action 'self';",
       "script-src 'self' https://www.googletagmanager.com;",
       "style-src 'self';",
       "img-src 'self' data: https://*.google-analytics.com https://www.googletagmanager.com;",
       "font-src 'self';",
       "connect-src 'self' https://*.google-analytics.com https://*.analytics.google.com https://www.googletagmanager.com;",
       "manifest-src 'self';",
       "worker-src 'self';");

            if (!context.RequestServices
                    .GetRequiredService<IWebHostEnvironment>()
                    .IsDevelopment())
            {
                csp += " upgrade-insecure-requests;";
            }

            headers.TryAdd("Content-Security-Policy", csp);
            await next();
        });
    }
}
