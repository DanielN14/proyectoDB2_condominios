﻿namespace proyectoDB2_condominios.Models
{
    public class Modelo_Usuario
    {
        public int idUsuario { get; set; }
        public string? email { get; set; }

        public string? password { get; set; }
        public int idRolUsuario { get; set; }
        public int idProyectoHabitacional { get; set; }
        public int idPersona { get; set; }

    }
}
