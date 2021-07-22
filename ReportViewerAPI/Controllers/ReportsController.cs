using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReportViewerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ReportsController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [HttpGet("print")]
        public IActionResult Print()
        {
            string mimtype = "";
            int extension = 1;
            //var fileDirPath = this.webHostEnvironment.ContentRootPath;
            //FileStream reportStream = new FileStream(fileDirPath + @"\ReportFiles\Report1.rdlc", FileMode.Open, FileAccess.Read);
            //string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ReportViewerAPI.dll", string.Empty);
            //string rdclFilePath = string.Format("{0}\\ReportFiles\\{1}.rdlc", fileDirPath, "Report1");

            var folderName = Path.Combine("ReportFiles");
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(path, "EmployeeReport.rdlc");
            //bool hasfile = System.IO.File.Exists(fullPath);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            //Encoding.GetEncoding("windows-1252");
            parameters.Add("prm", "Relatório emitido por Alexander Silva.");
            LocalReport lr = new LocalReport(fullPath);
            lr.AddDataSource("dtEmployee", GetEmployees());
            var result = lr.Execute(RenderType.Pdf, extension, parameters, mimtype);
            return File(result.MainStream, "application/pdf");
        }

        public DataTable GetEmployees()
        {
            var dt = new DataTable();
            dt.Columns.Add("EmployeeId");
            dt.Columns.Add("FirstName");
            dt.Columns.Add("Email");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Department");
            dt.Columns.Add("Designation");

            DataRow row;
            for (int i = 101; i <= 120; i++)
            {
                row = dt.NewRow();
                row["EmployeeId"] = i;
                row["FirstName"] = "Alexander"+i;
                row["Email"] = "alexander."+i+"@gmail.com";
                row["Phone"] = "55-21-98847-0"+i;
                row["Department"] = "IT";
                row["Designation"] = "Computer Engineer";

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
