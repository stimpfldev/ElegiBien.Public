using ElegiBien.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddElegiBienServices(builder.Configuration);

var app = builder.Build();

if (await app.RunDatabaseCommandAsync(args))
{
    return;
}

app.ConfigureElegiBienPipeline();
app.Run();

public partial class Program;
