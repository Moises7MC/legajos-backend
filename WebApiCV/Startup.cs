using DinkToPdf;
using DinkToPdf.Contracts;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .WithHeaders("authorization", "accept", "content-type", "origin"));
            }); ;
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
            services.AddDbContext<Contexts.bdLegajosContext>(_ => {
                _.UseSqlServer(Configuration.GetConnectionString("CadenaConexionDB"), options =>
                {
                    options.CommandTimeout(600); // 10 minutes
                });
             });
            //SOLUCI�N PARA PROBLEMAS DE REFERENCIAS C�CLICAS
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("keysecret"));
            services.AddAuthentication(au =>
            {
                au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiCV", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase("/webapicv");
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // 1. Sacamos Swagger del bloque IF para que funcione en cualquier entorno
            // O asegúrate de que el bloque IF incluya la lógica de producción si lo prefieres
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // 2. EL CAMBIO CLAVE: Usar ruta relativa con "./"
                // Esto hace que busque el JSON dentro de /webapicv/ en lugar de la raíz del servidor
                c.SwaggerEndpoint("./v1/swagger.json", "WebApiCV v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // El orden de estos es importante
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
