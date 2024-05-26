using DependencyInjection.Services.InfectedService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register 'Singleton' Service
//Use for services that are expensive to create or need to be shared across the entire application, such as configuration managers, logging services, or caching mechanisms.
builder.Services.AddSingleton<IInfectedService, InfectedService>();

//Register 'Scoped' Service
//Use for services that handle business logic within a single web request, such as data repositories, business services, or transaction handlers.
//builder.Services.AddScoped<IInfectedService, InfectedService>();

//Register 'Transient' Service
//Use for lightweight, stateless services like sending emails, generating reports, or performing calculations.
//builder.Services.AddTransient<IInfectedService, InfectedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
