using Dealer.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dealer.WebUI.Controllers
{
    // Controlador principal de la interfaz web para las páginas de inicio, privacidad y errores
    public class HomeController : Controller
    {
        // Muestra la página de inicio (Index)
        public IActionResult Index()
        {
            return View();
        }

        // Muestra la página de políticas de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Gestiona y muestra la vista de errores del sistema sin almacenar caché
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}