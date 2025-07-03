using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Recepify.API.Helpers;
using Recepify.BLL.Configuration;
using System.Diagnostics;
using Recepify.API.Filters;
using Recepify.API.ExceptionHandlers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context =>
    {
        var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["traceId"] = traceId;
    });
});


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResultFilter>();
});


builder.Services.AddBusinessLayerDependencies(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
             {
                 context.HandleResponse();

                 context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                 context.Response.ContentType = "application/problem+json";

                 var problemDetails = new ProblemDetails
                 {
                     Status = StatusCodes.Status401Unauthorized,
                     Title = "Unauthorized",
                     Detail = "Authentication failed. Valid credentials are required to access this resource.",
                 };

                 var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
                 problemDetails.Extensions["traceId"] = traceId;
                 await context.Response.WriteAsJsonAsync(problemDetails, context.HttpContext.RequestAborted); // Use cancellation token
             }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecuritySchema =
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            },
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            Type = SecuritySchemeType.Http,
            Name = "Bearer",
            In = ParameterLocation.Header,
        };
    c.AddSecurityDefinition(jwtSecuritySchema.Reference.Id, jwtSecuritySchema);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecuritySchema, Array.Empty<string>() }
    });
});


var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await MigrationHelper.ApplyMigrationsAsync(app);
app.Run();
