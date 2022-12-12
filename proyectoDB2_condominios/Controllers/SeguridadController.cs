using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data.SqlClient;
using System.Data;

namespace proyectoDB2_condominios.Controllers
{
    public class SeguridadController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            ViewBag.Condominios = CargarCondominios();
            return View();
        }
        public IActionResult Visitas_QR()
        {
            ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            return View();
        }
        public IActionResult Vehiculos_Condominos()
        {
            ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            ViewBag.vehiculos = CargarVehiculos_Condominos();
            return View();
        }

        public List<Vehiculo> CargarVehiculos_Condominos()
        {
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVehiculosResidentes", null);
            List<Vehiculo> VehiculosList = new List<Vehiculo>();

            foreach (DataRow row in ds.Rows)
            {
                VehiculosList.Add(new Vehiculo()
                {
                    idVehiculo = Convert.ToInt32(row["idVehiculo"]),
                    placa = row["placa"].ToString(),
                    marca = row["marca"].ToString(),
                    modelo = row["modelo"].ToString(),
                    color = row["color"].ToString(),
                });
            }

            return VehiculosList;
        }
        public ActionResult Validacion(int CodigoQR)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ValidarQR", new List<SqlParameter>()
                {
                new SqlParameter("@CodigoQR", CodigoQR),
                });
                List<QR> qrList = new List<QR>();
                foreach (DataRow row in ds.Rows)
                {
                    qrList.Add(new QR()
                    {
                        codigo = Convert.ToInt32(row["codigo"]),
                    });
                }

            if (qrList.Count()!=0)
            {
                TempData["Mensaje"] = "Codigo correcto";
                return RedirectToAction("Visitas_QR", "Seguridad");
            }
            else
            {
                TempData["Error"] = "Codigo incorrecto";
                return RedirectToAction("Visitas_QR");
            }

        }
        public ActionResult MarcarSalida(int idVisita)
        {

            
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idVisita", idVisita),
            };

            DatabaseHelper.ExecStoreProcedure("SP_UpdateFechaSalida", param);

            return RedirectToAction("Visitas_Guarda", "Seguridad");
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
        public ActionResult Visitas_Guarda(int idProyectoHabitacional)
        {
            ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            ViewBag.Visitas_Guarda = CargarVisitas_Guarda(idProyectoHabitacional);
            return View();
        }

        public List<Visitas_Guarda> CargarVisitas_Guarda(int idProyectoHabitacional)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitas_Guarda", new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional),
            });

            List<Visitas_Guarda> VisitasList = new List<Visitas_Guarda>();

            foreach (DataRow row in ds.Rows)
            {
                VisitasList.Add(new Visitas_Guarda()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    nombre = row["nombre"].ToString(),
                    primerApellido = row["primerApellido"].ToString(),
                    segundoApellido = row["segundoApellido"].ToString(),
                    FechaEntrada = Convert.ToDateTime(row["FechaEntrada"]),
                    Placa = row["Placa"].ToString(),
                    Vivienda = row["Vivienda"].ToString(),
                    Condominio = row["Condominio"].ToString(),
                });
            }

            return VisitasList;
        }
    }
}
