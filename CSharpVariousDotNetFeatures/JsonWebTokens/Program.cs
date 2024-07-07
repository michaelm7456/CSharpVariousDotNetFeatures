using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;


// Add Authentication and specify the configuration as JWT Defaults.
builder.Services.AddAuthentication(_ =>
{
    _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    _.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(_ =>
    _.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JtwSettings:Issuer"],
        ValidAudience = config["JtwSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    }
);

// Add Authorization
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
