using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MySchoolSystem.Controllers
{
    public class AdonsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Weather(int latitude, int longitude)
        {
            //https://api.open-meteo.com/v1/forecast?latitude=33.44&longitude=-112.07&hourly=temperature_2m&temperature_unit=fahrenheit
            return View();
        }
    }
}
