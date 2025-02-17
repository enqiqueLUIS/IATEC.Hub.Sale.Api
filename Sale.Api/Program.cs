using Sale.Api.EndPoints.Sale;
using Sale.Infrastructure.Ioc.Di;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services
    .RegisterDataBase(builder.Configuration)
    .RegisterServices(builder.Configuration)
    .RegisterRepositories()
    .RegisterProviders()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        b => b
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed((hosts) => true));
});
var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
}
app.UseCors("CORSPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIS CONTRACT V1.0");
    c.RoutePrefix = "swagger";
    c.EnableFilter();
});

// Middleware para evitar el almacenamiento en caché
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
    context.Response.Headers.Append("Pragma", "no-cache");
    context.Response.Headers.Append("Expires", "0");
    context.Response.Headers.Append("Surrogate-Control", "no-store");
    await next();
});

app.MapDishEndPoints();
app.MapPaymentMethodEndpoints();
app.MapSaleDetailEndpoints();
app.MapSaleEndpoints();

app.Run();