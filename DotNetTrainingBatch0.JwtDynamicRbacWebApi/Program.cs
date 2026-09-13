using System.Text;
using DotNetTrainingBatch0.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DotNetTrainingBatch0.JwtDynamicRbacWebApi.Features.Auth;
using DotNetTrainingBatch0.JwtDynamicRbacWebApi.Features.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtWebApi", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below (without the 'Bearer ' prefix).",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();

// Register AppDbContext with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("JwtWebApiDb"));

var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        )
    };
});



var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.TblRoles.Any())
    {
        var roles = new[]
        {
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppRole { Id = 1, RoleName = "Admin" },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppRole { Id = 2, RoleName = "Staff" }
        };
        context.TblRoles.AddRange(roles);

        var permissions = new[]
        {
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppPermission { Id = 1, PermissionName = "Product.View" },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppPermission { Id = 2, PermissionName = "Product.Create" },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppPermission { Id = 3, PermissionName = "Product.Update" },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppPermission { Id = 4, PermissionName = "Product.Delete" }
        };
        context.TblPermissions.AddRange(permissions);

        var rolePermissions = new[]
        {
            // Admin permissions
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppRolePermission { Id = 1, RoleId = 1, PermissionId = 1 },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppRolePermission { Id = 2, RoleId = 1, PermissionId = 2 },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppRolePermission { Id = 3, RoleId = 1, PermissionId = 3 },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppRolePermission { Id = 4, RoleId = 1, PermissionId = 4 },
            // Staff permissions
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppRolePermission { Id = 5, RoleId = 2, PermissionId = 1 }
        };
        context.TblRolePermissions.AddRange(rolePermissions);

        context.TblUsers.AddRange(
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppUser
            {
                Id = 1,
                Username = "admin",
                Password = "123",
                RoleId = 1
            },
            new DotNetTrainingBatch0.Database.AppDbContextModels.AppUser
            {
                Id = 2,
                Username = "staff",
                Password = "123",
                RoleId = 2
            }
        );
        context.SaveChanges();
    }
}

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
