using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace proyectoDB2_condominios.Controllers
{
    public class CondominiosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Condominios = CargarCondominios();
            return View();
        }

        public List<Condominio> CargarCondominios()
        {
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerCondominios", null);
            List<Condominio> condominioList = new List<Condominio>();

            foreach (DataRow row in ds.Rows)
            {
                condominioList.Add(new Condominio()
                {
                    idProyectoHabitacional = Convert.ToInt32(row["idProyectoHabitacional"]),
                    logo = row["logo"].ToString(),
                    codigo = row["codigo"].ToString(),
                    nombre = row["nombre"].ToString(),
                    direccion = row["direccion"].ToString(),
                    telefonoOficina = row["telefonoOficina"].ToString(),

                });
            }

            return condominioList;
        }
        public ActionResult Editar(int idProyectoHabitacional)
        {
            ViewBag.condominio = CargarCondominio(idProyectoHabitacional);
            return View();
        }
        private List<Condominio> CargarCondominio(int idProyectoHabitacional)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional)
            };
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerCondominio", param);
            List<Condominio> condominioList = new List<Condominio>();

            foreach (DataRow row in ds.Rows)
            {
                condominioList.Add(new Condominio()
                {
                    idProyectoHabitacional = Convert.ToInt32(row["idProyectoHabitacional"]),
                    logo = row["logo"].ToString(),
                    codigo = row["codigo"].ToString(),
                    nombre = row["nombre"].ToString(),
                    direccion = row["direccion"].ToString(),
                    telefonoOficina = row["telefonoOficina"].ToString(),
                });
            }

            return condominioList;
        }

        public ActionResult UpdateCondominio(int idProyectoHabitacional, string nombre, string direccion, string telefonoOficina)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@direccion", direccion),
                new SqlParameter("@telefonoOficina", telefonoOficina)
            };

            DatabaseHelper.ExecStoreProcedure("SP_UpdateCondominio", param);

            return RedirectToAction("Index", "Condominios");
        }

        public ActionResult EliminarCondominio(string idProyectoHabitacional)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional)
            };

            DatabaseHelper.ExecStoreProcedure("SP_EliminarCondominio", param);

            return RedirectToAction("Index", "Condominios");
        }
        public ActionResult Agregar()
        {
            return View();
        }
        public ActionResult AgregarCondominio(string codigo, string nombre, string direccion, string telefonoOficina)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@logo", "Logos/edificios.png"),
                new SqlParameter("@codigo", codigo),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@direccion", direccion),
                new SqlParameter("@telefonoOficina", telefonoOficina),
            };

            DatabaseHelper.ExecStoreProcedure("SP_AgregarCondominio", param);


            return RedirectToAction("Index", "Condominios");
        }
    }
}
