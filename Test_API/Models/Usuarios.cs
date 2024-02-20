namespace Test_API.Models
{
    public class Usuarios
    {

        public int id { get; set; }

        public string nombre { get; set; }

        public string apellido { get; set; }

        public int edad { get; set; }

        public DateTime fecha_nacimiento { get; set; }

        public DateTime fecha_hora_registro { get; set; }

        public bool estado { get; set; }
    }
}
