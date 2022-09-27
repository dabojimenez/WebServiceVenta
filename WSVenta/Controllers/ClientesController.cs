using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
//Importamos LINQ, para realizar las ocnsultas ocn Entity
using System.Linq;
//Importamos nuestro using(namespace)
using WSVenta.Models;
using WSVenta.Models.Request;
using WSVenta.Models.Response;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Si ya no tenems el token generado, ya no podremos ingresar a este controlador
    [Authorize]
    public class ClientesController : ControllerBase
    {
        //Indicamos el tipo de protocolo que usaremos
        [HttpGet]
        public IActionResult Get()
        {
            //Intanciamos nuestra clase de tipo respuesta
            Respuesta oRespuesta = new Respuesta();
            //Inicializmaos en 0, la repuesta
            oRespuesta.Exito = 0;
            try
            {
                //Creamos la instancia a nuestra conexion d ela BD
                using (VentaRealContext db = new VentaRealContext())
                {
                    //Consultamos a nuestra base de datos con LINQ
                    //rdenaremos en forma desendente, par amostar el ultimo
                    var lista = db.Cliente.OrderByDescending(d=> d.IdCliente).ToList();
                    //Mnadamos en la repsuesta/atributo el valor de 1
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lista;
                }
            }
            catch (System.Exception e)
            {
                oRespuesta.Mensaje = e.Message;
            }
            //Retornamos la repsuesta, en nuestro formato
            return Ok(oRespuesta);
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest model)
        {
            Respuesta oRespuesta = new Respuesta();
            oRespuesta.Exito = 0;
            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {
                    //Intanciamos la clase que hace referencia a nuestra BD
                    Cliente oCliente = new Cliente();
                    //Al cmapo de nuestra base de datos, le enviamos el campo/atributo que fue llenado en el modelo(Request)
                    oCliente.NombreCliente = model.NombreCliente;
                    //Agregamos a la isntruccion (query)
                    db.Add(oCliente);
                    //Agregamos ahora si a la BD
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch(Exception e)
            {
                oRespuesta.Mensaje = e.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpPut]
        public IActionResult Edit(ClienteRequest model)
        {
            Respuesta oRespuesta = new Respuesta();
            oRespuesta.Exito = 0;
            try
            {
                using (VentaRealContext db = new VentaRealContext ())
                {
                    Cliente cliente = new Cliente();
                    //Creamos el query de selectc con where en el id de mi tabla en BS
                    //me retonra un registro
                    cliente = db.Cliente.Find(model.IdCliente);
                    cliente.NombreCliente = model.NombreCliente;
                    //Le indica al entity, que este registro a apsado a un etsado modificado
                    db.Entry(cliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges ();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception e)
            {
                oRespuesta.Mensaje = e.Message;
            }
            return Ok(oRespuesta);
        }

        //El id, lo pasaremos por la URL, y para eso agregamos en el protocolo, la variable
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta oRespuesta = new Respuesta();
            oRespuesta.Exito = 0;
            try
            {
                using (VentaRealContext db = new VentaRealContext())
                {
                    Cliente cliente = new Cliente();
                    //Creamos el query de selectc con where en el id de mi tabla en BS
                    //me retonra un registro
                    cliente = db.Cliente.Find(Id);

                    //Le indica al entity, que este registro sea removido en la BD
                    db.Remove(cliente);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception e)
            {
                oRespuesta.Mensaje = e.Message;
            }
            return Ok(oRespuesta);
        }

    }
}
