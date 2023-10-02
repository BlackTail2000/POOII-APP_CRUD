namespace APP_CRUD.Models
{
    public class Personal
    {
        public int IdPersonal { get; set; }
        public string NombrePerso { get; set; }
        public Departamento refDepartamento { get; set;}
        public double Sueldo { get; set; }
        public string FechaContrato { get; set; }
      
        public string Estado { get; set; }
    }
}
