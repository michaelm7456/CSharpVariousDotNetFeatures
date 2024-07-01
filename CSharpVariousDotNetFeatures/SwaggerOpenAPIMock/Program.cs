using Microsoft.OpenApi.Models;
using SwaggerOpenAPIMock.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Add services for 'Swagger'
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("My API", new OpenApiInfo { Title = "My API", Version = "v1.0.0" });
});

var app = builder.Build();

//Enable 'Swagger' and set endpoint for OpenAPI spec.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    }
    );
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi.json", "My API");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
