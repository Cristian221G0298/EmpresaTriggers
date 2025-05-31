using Empresa2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresa.Repositories
{
    class EmpleadosRepository
    {
        EmpresaContext context = new();

        public void Insert(Empleados e)
        {
            context.Empleados.Add(e);
            context.SaveChanges();
        }

        public void Update(Empleados e)
        {
            context.Empleados.Update(e);
            context.SaveChanges();
        }

        public void Delete(Empleados e)
        {
            context.Empleados.Remove(e);
            context.SaveChanges();
        }

        public IEnumerable<Empleados> GetAllEmpleados()
        {
            return context.Empleados.Where(x=>x.Eliminar==false).Include(x=>x.IdSeccionNavigation);
        }

        public int TotalEmpleados => context.Empleados.Count();
        public decimal MontoSueldos => context.Empleados.Sum(x => x.Sueldo);
        
        public Empleados GetEmpleadoById(int id)
        {
            return context.Empleados.Where(x => x.Id == id).First();
        }
    }
}
