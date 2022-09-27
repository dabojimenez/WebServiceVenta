using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WSVenta.Models;
using WSVenta.Models.Common;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Tools;

namespace WSVenta.Services
{
    public class UserService : IUserService
    {
        //Para poder obtener el secreto
        private readonly AppSettings _appSettings;

        //constructor
        public UserService(IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
        }
        public UsuarioResponse Auth(AuthRequest model)
        {
            UsuarioResponse userResponse = new UsuarioResponse();
            using (var db = new VentaRealContext())
            {
                //Encriptamos la paswword que nos envia el usuario
                string givePassword = Encrypt.GetSHA256(model.Password);
                //Buscamos en la base de datos, y si existe nos regresa el elemento o caso contrario un null
                var usuario = db.Usuario.Where(d => d.EmailUsu == model.Email && d.PasswordUsu == givePassword).FirstOrDefault();
                //si es vacio, retornamos null
                if (usuario == null) return null;
                userResponse.correo = usuario.EmailUsu;
                //Le pasamos el usuario, ya que tiene el ID que nos interesa
                userResponse.token = GetToken(usuario);
            }
            return userResponse;
        }

        //Creamos un metodo privado, para obtener el token
        private string GetToken(Usuario usuario)
        {
            //Un clein, es una parte que van amedia sdel toke, y pueden ser publicos o privados
            var tokenHandler = new JwtSecurityTokenHandler();
            //cremaos una variable llamada llave, para obtener la llave
            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsu.ToString()),
                        //Ademas podemos crear nuestros propios (ClaimTypes)
                        new Claim(ClaimTypes.Email, usuario.EmailUsu)
                    }
                    ),
                //Cuando queremos que expire nuestro token
                Expires = DateTime.UtcNow.AddDays(60),//En 60 dias
                //Esto es lo que pemrite que encripte la informacion, y usarmeos la llave que guardamos en nuestro appsettings.json
                //el segundo parametro es el algoritmo que vamos a usar para encriptar (HmacSha256Signature) => esta frimado
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Aqui vamos a escribir el token
            return tokenHandler.WriteToken(token);
        }
    }
}
