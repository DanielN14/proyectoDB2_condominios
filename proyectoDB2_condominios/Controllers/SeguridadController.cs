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
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.Condominios = CargarCondominios();
                return View();
            }
        }
        public IActionResult Visitas_QR()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                return View();
            }
        }

        public IActionResult Vehiculos_Condominos()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.vehiculos = CargarVehiculos_Condominos();
                return View();
            }
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
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ValidarQR", new List<SqlParameter>(){
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

            if (qrList.Count() != 0)
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
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.VisitasTradicionales_Guarda = CargarVisitasTradicionales_Guarda(idProyectoHabitacional);
                ViewBag.VisitasDelivery_Guarda = CargarVisitasDelivery_Guarda(idProyectoHabitacional);
                return View();
            }
        }

        public List<VisitasTradicionales_Guarda> CargarVisitasTradicionales_Guarda(int idProyectoHabitacional)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitasTradicional_Guarda", new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional),
            });

            List<VisitasTradicionales_Guarda> listVisitasTradicionales = new List<VisitasTradicionales_Guarda>();

            foreach (DataRow row in ds.Rows)
            {
                listVisitasTradicionales.Add(new VisitasTradicionales_Guarda()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                    numeroVivienda = row["numeroVivienda"].ToString(),
                    nombreResidente = row["nombreResidente"].ToString(),
                    nombreVisitante = row["nombreVisitante"].ToString(),
                    cedulaVisitante = row["cedulaVisitante"].ToString(),
                    condominio = row["condominio"].ToString(),
                });
            }

            return listVisitasTradicionales;
        }

        public List<VisitasDelivery_Guarda> CargarVisitasDelivery_Guarda(int idProyectoHabitacional)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitasDelivery_Guarda", new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional),
            });

            List<VisitasDelivery_Guarda> listVisitasDelivery = new List<VisitasDelivery_Guarda>();

            foreach (DataRow row in ds.Rows)
            {
                listVisitasDelivery.Add(new VisitasDelivery_Guarda()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                    numeroVivienda = row["numeroVivienda"].ToString(),
                    nombreResidente = row["nombreResidente"].ToString(),
                    proveedorDelivery = row["proveedorDelivery"].ToString(),
                    condominio = row["condominio"].ToString(),
                });
            }

            return listVisitasDelivery;
        }

    }
}
