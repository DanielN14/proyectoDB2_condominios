using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using proyecto_condominios.DatabaseHelper;
using QRCoder;
using System;
using System.IO;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using proyectoDB2_condominios.Models;

namespace proyectoDB2_condominios.Controllers
{
    public class VisitasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult EasyPass()
        {
            ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            return View();
        }
        public IActionResult AgregarQR(string idPersona)
        {

            ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            Random r = new Random();
            int number = r.Next(1000, 10000);

            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(number.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);

            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap bitmap = qRCode.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    ViewBag.numero = number.ToString();
                }
            }
            DatabaseHelper.ExecStoreProcedure("SP_AgregarQR",
                new List<SqlParameter>()
                {
                    new SqlParameter("@idPersona", idPersona),
                    new SqlParameter("@codigo", number),
                }
            );

            return View();
        }
    }
}
