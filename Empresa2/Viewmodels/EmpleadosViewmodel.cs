using CommunityToolkit.Mvvm.Input;
using Empresa2.Models;
using Empresa.Repositories;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Empresa.Viewmodels
{
    public class EmpleadosViewmodel : INotifyPropertyChanged
    {
        EmpleadosRepository repoEmpleados = new();
        SeccionesRepository repoSecciones = new();
        public string Modo { get; set; } = "Ver";
        public List<Empleados> ListaEmpleados { get; set; } = new();
        public Empleados Empleado { get; set; }

        public List<Secciones> ListaSecciones { get; set; } = new();
        public ICommand GuardarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand GuardarEliminarCommand { get; set; }
        public ICommand VerAgregarEmpleadosCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public EmpleadosViewmodel()
        {
            ListaEmpleados = repoEmpleados.GetAllEmpleados().ToList();
            ListaSecciones = repoSecciones.GetAllSecciones().ToList();
            VerAgregarEmpleadosCommand = new RelayCommand(VerAgregar);
            EditarCommand = new RelayCommand<Empleados>(VerEditar);
            EliminarCommand = new RelayCommand<Empleados>(VerEliminar);
            GuardarEliminarCommand = new RelayCommand(GuardarEliminar);
            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
            CargarEmpleados();

        }

        private void CargarEmpleados()
        {
            ListaEmpleados.Clear();
            var e = repoEmpleados.GetAllEmpleados();
            foreach (var empleado in e)
            {
                ListaEmpleados.Add(empleado);
            }
        }

        private void Cancelar()
        {
            Modo = "Ver";
            PropertyChanged?.Invoke(this, new(null));
        }

        private void Guardar()
        {
            if (Empleado != null)
            {

                if (Modo == "Editar")
                {
                    Empleados? e = repoEmpleados.GetEmpleadoById(Empleado.Id);
                    if (e != null)
                    {
                        e.Id = Empleado.Id;
                        e.Nombre = Empleado.Nombre;
                        e.Sueldo = Empleado.Sueldo;
                        e.IdSeccion = Empleado.IdSeccion;
                        e.IdSeccionNavigation = Empleado.IdSeccionNavigation;
                        repoEmpleados.Update(e);
                        CargarEmpleados();
                    }
                }

                if (Modo == "Agregar")
                {
                    repoEmpleados.Insert(Empleado);
                    CargarEmpleados();
                }
                Modo = "Ver";
                PropertyChanged?.Invoke(this, new(null));
            }
        }

        private void GuardarEliminar()
        {
            if (Modo == "Eliminar")
            {
                repoEmpleados.Delete(Empleado);
                CargarEmpleados();
            }

            Modo = "Ver";
            PropertyChanged?.Invoke(this, new(null));
        }

        private void VerEliminar(Empleados e)
        {
            Empleado = e;
            Modo = "Eliminar";
            PropertyChanged?.Invoke(this, new(null));
        }

        private void VerEditar(Empleados e)
        {
            Empleado = e;
            Empleados clon = new()
            {
                Id = Empleado.Id,
                Nombre = Empleado.Nombre,
                IdSeccion = Empleado.IdSeccion,
                Sueldo = Empleado.Sueldo,
                IdSeccionNavigation = Empleado.IdSeccionNavigation
             
            };

            Empleado = clon;
            Modo = "Editar";
            PropertyChanged?.Invoke(this, new(null));
        }

        private void VerAgregar()
        {
            Modo = "Agregar";
            Empleado = new();
            PropertyChanged?.Invoke(this, new(null));
        }
    }
}
