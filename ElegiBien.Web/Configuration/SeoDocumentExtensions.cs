using System.Security;
using System.Text;

namespace ElegiBien.Web.Configuration;

public static class SeoDocumentExtensions
{
    private static readonly string[] SitemapPaths =
    [
        "/",
        "/AirConditioning",
        "/Paint",
        "/Flooring",
        "/Heating",
        "/Legal/Methodology"
    ];

    public static IApplicationBuilder UseElegiBienSeoDocuments(
        this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                await next();
                return;
            }

            if (context.Request.Path.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase))
            {
                await WriteRobotsAsync(context);
                return;
            }

            if (context.Request.Path.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase))
            {
                await WriteSitemapAsync(context);
                return;
            }

            if (context.Request.Path.Equals("/ads.txt", StringComparison.OrdinalIgnoreCase))
            {
                await WriteAdsTxtAsync(context);
                return;
            }

            await next();
        });
    }

    private static async Task WriteRobotsAsync(HttpContext context)
    {
        var baseUrl = BuildBaseUrl(context.Request);
        var content = string.Join('\n',
            "User-agent: *",
            "Allow: /",
            "Disallow: /Shared/",
            string.Empty,
            $"Sitemap: {baseUrl}/sitemap.xml",
            string.Empty);

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.Headers.CacheControl = "public,max-age=3600";
        await context.Response.WriteAsync(content, Encoding.UTF8);
    }

    private static async Task WriteSitemapAsync(HttpContext context)
    {
        var baseUrl = BuildBaseUrl(context.Request);
        var builder = new StringBuilder();

        builder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var path in SitemapPaths)
        {
            var location = SecurityElement.Escape($"{baseUrl}{path}");
            builder.Append("  <url><loc>")
                .Append(location)
                .AppendLine("</loc></url>");
        }

        builder.AppendLine("</urlset>");

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/xml; charset=utf-8";
        context.Response.Headers.CacheControl = "public,max-age=3600";
        await context.Response.WriteAsync(builder.ToString(), Encoding.UTF8);
    }

    private static async Task WriteAdsTxtAsync(HttpContext context)
    {
        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
        var configuredPublisherId = configuration["GoogleAdSense:PublisherId"]?.Trim();

        if (string.IsNullOrWhiteSpace(configuredPublisherId))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        var publisherId = configuredPublisherId.StartsWith("ca-pub-", StringComparison.OrdinalIgnoreCase)
            ? configuredPublisherId[3..]
            : configuredPublisherId;

        if (!publisherId.StartsWith("pub-", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        var content = $"google.com, {publisherId}, DIRECT, f08c47fec0942fa0\n";

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.Headers.CacheControl = "public,max-age=3600";
        await context.Response.WriteAsync(content, Encoding.UTF8);
    }

    private static string BuildBaseUrl(HttpRequest request)
    {
        var pathBase = request.PathBase.HasValue
            ? request.PathBase.Value!.TrimEnd('/')
            : string.Empty;

        return $"{request.Scheme}://{request.Host}{pathBase}";
    }
}
