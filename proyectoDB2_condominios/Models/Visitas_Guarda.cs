namespace proyectoDB2_condominios.Models
{
    public class Visitas_Guarda
    {
        public int idVisita { get; set; }
        public string? nombre { get; set; }
        public string? primerApellido { get; set; }
        public string? segundoApellido { get; set; }
        public DateTime FechaEntrada { get; set; }
        public  string? Placa { get; set; }
        public string? Vivienda { get; set; }
        public string? Condominio { get; set; }
    }
}
