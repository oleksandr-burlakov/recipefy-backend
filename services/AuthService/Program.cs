using System.Diagnostics;
using System.Text;
using AuthService.Entities;
using AuthService.ExceptionHandlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedKernel.Filters;
using SharedKernel.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<AuthContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddIdentityCore<User>(options =>
    {
        options.User.RequireUniqueEmail = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<Role>()
    .AddSignInManager()
    .AddEntityFrameworkStores<AuthContext>();

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
        { jwtSecuritySchema, [] }
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

try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
    await DatabaseHelpers.EnsureDatabaseCreatedAsync(dbContext);
    await DatabaseHelpers.MigrateDatabaseAsync(dbContext);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
app.Run();