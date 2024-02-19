using JobOpeningsApiWebService.Data;
using JobOpeningsApiWebService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DBConnection");

builder.Services.AddDbContext<JobOpeningsDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<ILocation, LocationRepository>();
builder.Services.AddScoped<IDepartment, DepartmentRepository>();
builder.Services.AddScoped<IJobs, JobRepository>();
builder.Services.AddScoped<IUser, UserRepository>();

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Job Openings Web API", Version = "v1" });

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer {your_token}' in the text input below."
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				Array.Empty<string>()
			}
		});
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(key),
			ValidateIssuer = false,
			ValidateAudience = false
		};

		options.Events = new JwtBearerEvents
		{
			OnTokenValidated = context =>
			{
				return Task.CompletedTask;
			},
			OnAuthenticationFailed = context =>
			{
				Console.WriteLine($"Authentication failed: {context.Exception.Message}");
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminPolicy", policy =>
	{
		policy.RequireRole("Admin");
	});

var app = builder.Build();

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
