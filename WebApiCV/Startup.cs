using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApiCV.Data;

namespace WebApiCV
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // =========================
        // CONFIGURE SERVICES
        // =========================
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Repositorios
            services.AddScoped<ConstanteRepository>();
            services.AddScoped<InterfaceRepository>();
            services.AddScoped<PersonaRepository>();
            services.AddScoped<DatosUsuarioRepository>();
            services.AddScoped<TareaModuloRepository>();
            services.AddScoped<LegDatosGeneralesRepository>();
            services.AddScoped<ReporteCapacitacionesRepository>();
            services.AddScoped<ReporteLegajosRepository>();
            services.AddScoped<RegistroConvocatoriaRepository>();
            services.AddScoped<ConvocatoriaRepository>();
            services.AddScoped<LegGrupInvSemRepository>();

            // DbContext
            services.AddDbContext<Contexts.bdLegajosContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("CadenaConexionDB"),
                    sql => sql.CommandTimeout(600)
                );
            });

            // Controllers + Newtonsoft (evita referencias cíclicas)
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // PDF
            services.AddSingleton(typeof(IConverter),
                new SynchronizedConverter(new PdfTools()));

            // JWT
            var key = Encoding.ASCII.GetBytes(
                Configuration.GetValue<string>("keysecret"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // SWAGGER (CLÁSICO .NET 5)
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebApiCV",
                    Version = "v1"
                });

                // JWT en Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese: Bearer {token}"
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
                        new string[] {}
                    }
                });
            });
        }

        // =========================
        // CONFIGURE PIPELINE
        // =========================
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase("/webapicv");
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Swagger (fuera del IF)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "WebApiCV v1");
                c.RoutePrefix = "swagger";
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}