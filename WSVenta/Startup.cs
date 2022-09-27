using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSVenta.Models.Common;
using WSVenta.Services;
using WSVenta.Tools;

namespace WSVenta
{
    public class Startup
    {
        readonly string MiCors = "MiCors";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configuracion
            services.AddCors(options =>
            {
                //agregamos una politica, con nuestra variable que creamos, y ademas podemos agregar varia politicas
                //le colocamos otro atributo, que se llama (builder)
                options.AddPolicy(name: "MiCors",
                                builder =>
                                {
                                    /**Aqui indicaremos el orien d elos datos
                                     * * = permite todas las conexions
                                     *  hhtps://google.com = solo permitira conexiones de google
                                     *  MUCHO OJO AQUI
                                     */
                                    builder.WithHeaders("*");//Que acepte todos los headers
                                    builder.WithOrigins("*");
                                    builder.WithMethods("*");
                                    //builder.AllowAnyOrigin()
                                    //.AllowAnyHeader()//Permitimos todos y cualquier HEADERS/encabezado
                                    //.AllowAnyMethod();//Permitir todo y cualquier METODO
                                });
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new IntToStringConverter());
                    options.JsonSerializerOptions.Converters.Add(new DecimalToStringConverter());
                });


            //Esta forma de inyectar es para que el objeto sirva para cada request, existen otras dos mas, pero usaremos esta, recibe una interface y una clase(implementa)
            services.AddScoped<IUserService, UserService>();
            //Agregamos la interface, para usar la dependencia
            services.AddScoped<IVentaService, VentaService>();//    <==== LINEA AGREGADA


            //Debemos colocar el nombre tal cual pusimos en el JSON
            var appSettingsSection = Configuration.GetSection("AppSettings");
            //Aqui agregamos el AppSettings, que creamos en la carpeta {common}
            services.Configure<AppSettings>(appSettingsSection);
            //Aqui configuaramos el JWT
            //JWT
            var appSettings = appSettingsSection.Get<AppSettings>();
            //Ahora colocamos una llave, ya que es unica y cada encriptacion sera unica, (GetBytes), nos regresa una matriz de bytes(arreglo)
            var llave = Encoding.ASCII.GetBytes(appSettings.Secreto);
            services.AddAuthentication(d => {
                //JwtBearer, es un estandar para no reinventar la rueda, y LO INSTALAMOS MANUAL o POR RECOMENDACION DE VISUAL
                //Microsoft.aspnetcore.authtentication.jwtbearer
                //System.Identitymodel.tokens.jwt
                d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(d => {
                d.RequireHttpsMetadata = false;
                d.SaveToken = true;
                d.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                //SymmetricSecurityKey, aqui va la llave que creamos
                IssuerSigningKey = new SymmetricSecurityKey(llave),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors(MiCors);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
