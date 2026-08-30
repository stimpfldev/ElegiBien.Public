using System.Security.Cryptography;

namespace ElegiBien.Web.Configuration;

public static class ApplicationBuilderExtensions
{
    public const string CspNonceItemKey = "ElegiBien.CspNonce";

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

            var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            context.Items[CspNonceItemKey] = nonce;

            var configuration = context.RequestServices
                .GetRequiredService<IConfiguration>();

            var analyticsEnabled = string.Equals(
                configuration["GoogleAnalytics:Enabled"],
                "true",
                StringComparison.OrdinalIgnoreCase);

            var adsenseEnabled = string.Equals(
                configuration["GoogleAdSense:Enabled"],
                "true",
                StringComparison.OrdinalIgnoreCase);

            var scriptSrc = $"script-src 'nonce-{nonce}' 'strict-dynamic' https: http:;";
            var styleSrc = adsenseEnabled
                ? "style-src 'self' 'unsafe-inline' https:;"
                : "style-src 'self';";
            var imgSrc = analyticsEnabled || adsenseEnabled
                ? "img-src 'self' data: https:;"
                : "img-src 'self' data:;";
            var connectSrc = analyticsEnabled || adsenseEnabled
                ? "connect-src 'self' https:;"
                : "connect-src 'self';";
            var frameSrc = adsenseEnabled
                ? "frame-src https:;"
                : "frame-src 'none';";

            var csp = string.Join(' ',
                "default-src 'self';",
                "base-uri 'self';",
                "object-src 'none';",
                "frame-ancestors 'none';",
                "form-action 'self';",
                scriptSrc,
                styleSrc,
                imgSrc,
                "font-src 'self' data:;",
                connectSrc,
                frameSrc,
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
