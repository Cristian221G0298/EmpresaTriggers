using Empresa2.Models.DTO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresa2.Services
{
    public class ReporteSeccionesService
    {
        public byte[] GetReporteSecciones(List<SeccionesDTO> secciones)
        {
            MemoryStream archivoMemoria = new();
            Document documentoPDF = new(PageSize.LETTER);
            PdfWriter.GetInstance(documentoPDF, archivoMemoria);
            documentoPDF.Open();

            Font fuenteTitulo = FontFactory.GetFont("Arial", 20, Font.BOLD);
            Font fuenteSubtitulo = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font fuenteDato = FontFactory.GetFont("Arial", 12);

            documentoPDF.Add(new Paragraph("Reporte de Secciones")
            {
                Alignment = Element.ALIGN_CENTER,
                Font = fuenteTitulo
            });

            documentoPDF.Add(new Paragraph("\n"));

            foreach (var seccion in secciones)
            {
                documentoPDF.Add(new Paragraph($"Sección: {seccion.Nombre} (ID: {seccion.Id})", fuenteSubtitulo));
                documentoPDF.Add(new Paragraph($"Número de empleados: {seccion.NúmeroDeEmpleados}", fuenteDato));
                documentoPDF.Add(new Paragraph($"Monto total pagado: {seccion.MontoPagado.ToString("C2")}", fuenteDato));
                documentoPDF.Add(new Paragraph("\n"));

                PdfPTable tabla = new(3);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { .5f, 4f, 2f });

                string[] encabezados = { "No.", "Nombre", "Sueldo" };
                foreach (var texto in encabezados)
                {
                    var celda = new PdfPCell(new Phrase(texto, fuenteDato))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    tabla.AddCell(celda);
                }

                int i = 1;
                foreach (var emp in seccion.Empleados)
                {
                    tabla.AddCell(new PdfPCell(new Phrase(i.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tabla.AddCell(new Phrase(emp.Nombre));
                    tabla.AddCell(new Phrase(emp.Sueldo.ToString("C2")));
                    i++;
                }

                documentoPDF.Add(tabla);
                documentoPDF.Add(new Paragraph("\n\n"));
            }

            documentoPDF.Close();
            return archivoMemoria.ToArray();
        }
    }
}
