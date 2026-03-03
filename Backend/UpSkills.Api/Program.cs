using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using UpSkills.Api.Interfaces;
using UpSkills.Api.Services;
using UpSkills.Business.Interfaces;
using UpSkills.Business.Libs;
using UpSkills.Business.Services;
using UpSkills.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Añadir y configurar DBContext de Entity Framework para usar la Base de Datos
builder.Services.AddDbContext<AppDbContext>(dbOptions =>
    dbOptions.UseNpgsql( //Npgsql es para PostGreSQL
        builder.Configuration.GetConnectionString("DbConnection"), //Conexión con la base de datos
        npgsqlOptions => npgsqlOptions.MigrationsAssembly("UpSkills.Api") //El proyecto donde se va a configurar la base de datos (esta misma API)
    )
);

builder.Services.AddOpenApi(); //Documentación de la API
builder.Services.AddControllers(); //Añadir todos los controladores / enpoints de la API
builder.Services.AddMemoryCache(); //Añadir el servicio de Memoria Cache

//Añadir y configurar AutoMapper para las conversiones entre DTO (Json) <-> Entidad (Tabla base de datos)
builder.Services.AddAutoMapper(config => config.AddProfile<AutoMapperConfig>());

//Añadir los servicios que vamos a utilizar
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPaisesService, PaisesService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

//Añadir politicas de origenes cruzados
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", //Crear politica para permitir al cliente Blazor
        policy => policy
            .WithOrigins("https://localhost:7086") //URL y puerto del cliente Blazor
            .AllowAnyHeader() //Permitir todas las cabezeras
            .AllowAnyMethod() //Permitir todos los metodos HTTP
        );
});

//Añadir servicio de Tokens de sesion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //ValidIssuer = "your_issuer",
                //ValidAudience = "your_audience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ApiTokenKey/issuer_Signing/"))
            };
        });

var app = builder.Build();

app.MapControllers(); //Añadir los controladores / endpoints de la API
app.UseHttpsRedirection(); //Añadir el redireccionamiento automatico si https esta disponible

//Añadir solo si estamos en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); //Documentación de la API
    app.MapScalarApiReference(); //Documentador para la API (Scalar)
}

app.UseCors("AllowBlazorClient"); //Aplicar la politica para permitir el cliente de Blazor

app.Run(); //Ejecutar la API
