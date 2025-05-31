using Empresa2.Models;
using Empresa2.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Empresa.Repositories
{
    public class SeccionesRepository
    {
        EmpresaContext context = new();
        public ObservableCollection<SeccionesDTO> GetSeccionesConEmpleados()
        {
            ObservableCollection<SeccionesDTO> seccionesDTO = new();

            var x = context.Secciones.Include(x => x.Empleados).Select(x => new SeccionesDTO
            {
                Id = x.Id,
                Nombre = x.Nombre,
                NúmeroDeEmpleados = x.NumeroEmpleados,
                MontoPagado = x.Empleados.Sum(x => x.Sueldo),
                Empleados = x.Empleados.Select(x => new EmpleadosDTO
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Sueldo = x.Sueldo
                }).ToList()
            });

            foreach(var s in x)
            {
                seccionesDTO.Add(s);
            }

            return seccionesDTO;
        }

        public IEnumerable<Empleados> GetEmpleadosBySeccion(Secciones s) 
        {
            return context.Secciones.Include(x => x.Empleados).First(x => x.Id == s.Id).Empleados;
            
        }
        public IEnumerable<Secciones> GetAllSecciones()
        {
            context = new();
            return context.Secciones.Include(x=>x.Empleados).Where(x=>x.Eliminar==false).OrderBy(x=>x.Nombre);
        }
        public Secciones? GetById(int id)
        {
            return context.Secciones.Include(x => x.Empleados).Where(x => x.Eliminar == false && x.Id == id).FirstOrDefault();
        }

        public void InsertarSeccion(Secciones s)
        {
            context.Secciones.Add(s);
            context.SaveChanges();
        }
        public void EliminarSeccion(Secciones s)
        {
            if (s != null)
            {
                var t = context.Secciones.FirstOrDefault(x=>x.Id==s.Id);
                var x = t.Empleados;
                
                if (context.Secciones.Find(s.Id).Empleados.Count == 0)
                {
                    context.Remove(s);
                }
                else
                {
                    s.Eliminar = true;
                }
                context.SaveChanges();
            }
            
        }

        public void EditarSeccion(Secciones s)
        {
            // al cambiar el sueldo maximo de la seccion, modificar el sueldo de los empleados que ganan mas de lo permitido. 
            List<Empleados> empleadostemp = context.Empleados.ToList();
            
            context.Update(s);
            foreach (var item in empleadostemp)
            {
                if (item.Sueldo > s.SueldoMaximo)
                {
                    item.Sueldo = s.SueldoMaximo;
                }
            }
            context.SaveChanges();
            
            
        }
    }
}
