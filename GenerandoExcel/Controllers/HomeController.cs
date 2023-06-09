﻿
using ClosedXML.Excel;
using GenerandoExcel.Entidades;
using GenerandoExcel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace GenerandoExcel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public async Task<IActionResult> ImportarPersonasDesdeExcel (IFormFile excel)
        {

            var workbook = new XLWorkbook(excel.OpenReadStream());
            var hoja = workbook.Worksheet(1);

            var primeraFilaUsada = hoja.FirstRowUsed().RangeAddress.FirstAddress.RowNumber;
            var ultimaFilaUsada = hoja.LastRowUsed().RangeAddress.FirstAddress.RowNumber;

            var personas = new List<Persona>();


            for(int i =primeraFilaUsada+1;i <= ultimaFilaUsada; i++)
            {
                var fila = hoja.Row(i);
                var persona = new Persona();

                persona.Nombre = fila.Cell(1).GetString();
                persona.Salario = fila.Cell(2).GetValue<decimal>();
                persona.FechaNacimiento = fila.Cell(3).GetDateTime();
                personas.Add(persona);
            }
            context.AddRange(personas);
            await context.SaveChangesAsync();

            return View("index");
        }

        [HttpGet]
        public async Task<FileResult> ExportarPersonasExcel()
        {
            var personas = await context.Personas.ToListAsync();
            var nombreArchivo = $"Personas.xlsx";

            return GenerarExcel(nombreArchivo, personas);
        }

        public IActionResult Index()
        {
            return View();
        }

        private FileResult GenerarExcel(string nombreArchivo, IEnumerable<Persona> personas)
        {
            DataTable dataTable= new DataTable("Personas");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Nombre")
            });

            foreach (var persona in personas)
            {
                dataTable.Rows.Add(persona.Id,persona.Nombre);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo);
                }
            }
        }
    }
}