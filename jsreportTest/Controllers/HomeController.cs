using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;
using jsreportTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace jsreportTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IJsReportMVCService JsReportMVCService { get; }


        public HomeController(ILogger<HomeController> logger, IJsReportMVCService jsReportMVCService)
        {
            _logger = logger;
            JsReportMVCService = jsReportMVCService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


      [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult PdfView()
        {


            var rs = new LocalReporting()
                .UseBinary(JsReportBinary.GetBinary())
                .KillRunningJsReportProcesses();

            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf)
                .Configure((r) => r.Template.Chrome = new Chrome
                {
                    Landscape = true,
                    MarginTop = "130px",
                    MarginLeft = "105px",
                    MarginBottom = "110px",
                    MarginRight = "105px",
                    Format = "A4"

                }
               )
               .OnAfterRender((r) => HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=\"myReport.pdf\"");



            return View();
        }




    }
}
