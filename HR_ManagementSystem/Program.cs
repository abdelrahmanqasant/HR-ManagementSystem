
using HR_ManagementSystem.Automappers;
using HR_ManagementSystem.Implementations;
using HR_ManagementSystem.Models;
using HR_ManagementSystem.Permission;
using HR_ManagementSystem.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace HR_ManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

         
            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddAutoMapper(typeof(EmployeeMapper).Assembly);
         
            builder.Services.AddDbContext<HRDbContext>(op =>
            {
                op.UseLazyLoadingProxies();
                op.UseSqlServer(builder.Configuration.GetConnectionString("constr"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<HRDbContext>();
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });
            builder.Services.AddScoped<IAttendence, AttendenceRepo>();
            builder.Services.AddScoped<ICommission, CommissionRepo>();
            builder.Services.AddScoped<IDeduction, DeductionRepo>();
            builder.Services.AddScoped<IDaysOffRepo, DaysOffRepo>();
            builder.Services.AddScoped<IWeeklyDaysOff, WeeklyDaysOffRepo>(); 
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

           

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Test API", Version = "v1" });

                // JWT Authentication
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT token like: Bearer {your token}"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            var app = builder.Build();
           

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR API V1");
                    c.RoutePrefix = string.Empty; 
                });

            }
            //using var scope = app.Services.CreateScope();
            //var services = scope.ServiceProvider;
            //var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //var logger = loggerFactory.CreateLogger("app");
            //try
            //{
            //    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            //    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            //    await Seeds.DefaultRoles.SeedAsync(roleManager);
            //    await Seeds.DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);
            //    await Seeds.DefaultUsers.SeedAdminUsersAsync(userManager, roleManager);
            //    await Seeds.DefaultUsers.SeedBasicUsersAsync(userManager, roleManager);
            //    logger.LogInformation("Data Seeded");
            //    logger.LogInformation("Application Started");
            //}
            //catch (System.Exception exception)
            //{
            //    logger.LogWarning(exception, "An Error Occured While Seeding Roles");

            //}
            app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200") 
    .AllowCredentials()
    .SetIsOriginAllowedToAllowWildcardSubdomains());
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
