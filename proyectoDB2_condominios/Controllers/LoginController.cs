using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;
using proyectoDB2_condominios.Models;

namespace proyectoDB2_condominios.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult LoginUser(string email, string password)
        {
            Modelo_Usuario usuario = spObtener_Usuario(email, password);

            if (usuario != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();

        }
        public Modelo_Usuario spObtener_Usuario(string email, string password)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@email", email),
                new SqlParameter("@password", password)
            };

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("sp_obtener_usuario", param);

            if (ds.Rows.Count == 1)
            {
                Modelo_Usuario usuario = new Modelo_Usuario()
                {
                    idUsuario = Convert.ToInt32(ds.Rows[0]["idUsuario"]),
                    email = ds.Rows[0]["email"].ToString(),
                    password = ds.Rows[0]["password"].ToString(),
                    idRolUsuario = Convert.ToInt32(ds.Rows[0]["idRolUsuario"]),
                    idPersona = Convert.ToInt32(ds.Rows[0]["idPersona"]),

                };

                return usuario;
            }

            return null;
        }
    }
}
