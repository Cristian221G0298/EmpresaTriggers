using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresa2.Models.DTO
{
    public class SeccionesDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal MontoPagado {  get; set; }
        public int NúmeroDeEmpleados {  get; set; }
        public List<EmpleadosDTO> Empleados { get; set; } = new();
    }

    public class EmpleadosDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Sueldo {  get; set; }
    }
}
