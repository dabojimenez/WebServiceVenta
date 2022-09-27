using System;
using System.Linq;
using WSVenta.Models;
using WSVenta.Models.Request;

namespace WSVenta.Services
{
    public class VentaService : IVentaService
    {
        public void Add(VentaRequest model)
        {
            
                using (VentaRealContext db = new VentaRealContext())
                {
                    using (var transaccion = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var venta = new Venta();
                            venta.TotalVenta = model.Conceptos.Sum(d => d.CatidadConcepto * d.PrecioUnitarioConcepto);
                            venta.FechaVenta = DateTime.Now;
                            venta.IdCliente = model.IdCliente;
                            db.Venta.Add(venta);
                            db.SaveChanges();

                            foreach (var modelConcepto in model.Conceptos)
                            {
                                var concepto = new Models.Concepto();
                                concepto.CatidadConcepto = modelConcepto.CatidadConcepto;
                                concepto.IdProducto = modelConcepto.IdProducto;
                                concepto.PrecioUnitarioConcepto = modelConcepto.PrecioUnitarioConcepto;
                                concepto.ImporteConcepto = modelConcepto.ImporteConcepto;
                                concepto.IdVenta = venta.IdVenta;
                                db.Concepto.Add(concepto);
                                db.SaveChanges();
                            }
                            transaccion.Commit();
                        }
                        catch (Exception)
                        {
                            transaccion.Rollback();
                            //como cacha la repsuesta , mandarmeos tambien otro mensaje
                            throw new Exception("Ocurrio un error en la inserción");
                        }
                    }
                }
        }
    }
}
