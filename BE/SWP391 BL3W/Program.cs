using FAMS.Api.Configurations.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWP391_BL3W.Database;
using SWP391_BL3W.Middlewares;
using SWP391_BL3W.Repository;
using SWP391_BL3W.Repository.Interface;
using SWP391_BL3W.Services;
using SWP391_BL3W.Services.Interface;
using System.Text;

namespace SWP391_BL3W
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Extensions.ServiceCollectionExtensions.Register(builder.Services);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(o => o.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            builder.Services.AddDbContext<SWPContext>(options => {
                options.UseSqlServer("data source=35.186.148.127;initial catalog=electronicDb;user id=sa;password=yourStrong1@Password;trustservercertificate=true;multipleactiveresultsets=true;");
                //options.UseSqlServer(builder.Configuration.GetConnectionString("ElectricStore"));
            });
            builder.Services.AddAutoMapper(typeof(Program));

            //Add Authencation
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidAudience = builder.Configuration["JWT:ValidAudience"],
                      ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))

                  };
              });

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Electronic Store", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
               
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            var app = builder.Build();
            
            /* app.UseMiddleware<GlobalExceptionMiddleware>();*/
            app.ConfigureCors(builder.Configuration);

            
            
                app.UseDeveloperExceptionPage();
            

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                {

                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;

                });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
/*            app.MapControllers();*/

            app.Run();
        }
    }
}