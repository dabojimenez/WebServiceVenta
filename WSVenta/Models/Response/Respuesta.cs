namespace WSVenta.Models.Response
{
    public class Respuesta
    {
        //retornaremos el estaus de la consulta, indicada por 1,0, etc
        //En ocaciones el True/False no se parsean bien or eso el uso de int
        public int Exito { get; set; }
        //el mensaje, si en caso salio mal algun proceso
        public string Mensaje { get; set; }
        //Le hago de tipo objet, ya que podremos meter cualquier ifnormacion que necesitemos en la varibale Data
        public object Data { get; set; }
    }
}
