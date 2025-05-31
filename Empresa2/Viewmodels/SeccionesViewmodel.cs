using CommunityToolkit.Mvvm.Input;
using Empresa2.Models;
using Empresa.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Empresa2.Services;
using Empresa2.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;

namespace Empresa.Viewmodels
{
    public class SeccionesViewmodel : INotifyPropertyChanged
    {
        SeccionesRepository repo = new();
        ReporteSeccionesService reportesSecciones = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Secciones> ListaSecciones { get; set; } = new();
        public ObservableCollection<SeccionesDTO> SeccionesDTO { get; set; } = new();
        public ICommand VerAgregarSeccionesCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand GuardarEliminarCommand { get; set; }
        public ICommand ImprimirSeccionesConEmpleadosCommand { get; set; }
        public string Modo { get; set; } = "Ver";
        public Secciones Seccion { get; set; } = new();

        public SeccionesViewmodel() {

            VerAgregarSeccionesCommand = new RelayCommand(VerAgregarSecciones);
            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
            EditarCommand = new RelayCommand<Secciones>(Editar);
            EliminarCommand = new RelayCommand<Secciones>(VerEliminar);
            GuardarEliminarCommand = new RelayCommand(GuardarEliminar);
            ImprimirSeccionesConEmpleadosCommand = new RelayCommand(DescargarSecciones);
            CargarSecciones();
        }

        private void CargarSecciones()
        {
            ListaSecciones.Clear();
            var e = repo.GetAllSecciones();
            foreach (var seccion in e)
            {
                ListaSecciones.Add(seccion);
            }
            SeccionesDTO = repo.GetSeccionesConEmpleados();
        }
        private void GuardarEliminar()
        {
            if (Modo == "Eliminar")
            {
                repo.EliminarSeccion(Seccion);
                CargarSecciones();
            }

            
            Modo = "Ver";
            PropertyChanged?.Invoke(this, new(null));
        }

        private void VerEliminar(Secciones? s)
        {
            Seccion = s;
            Modo = "Eliminar";
           
            PropertyChanged?.Invoke(this, new(null));
            
        }

        private void Editar(Secciones s)
        {
            Seccion = s;
            Secciones seccionClon = new Secciones
            {
                Nombre = Seccion.Nombre,
                SueldoMaximo = Seccion.SueldoMaximo,
                NumeroEmpleados = Seccion.NumeroEmpleados,
                Id = Seccion.Id,
                Eliminar = Seccion.Eliminar,
                Empleados = Seccion.Empleados
            };

            Seccion = seccionClon;
            Modo = "Editar";
            PropertyChanged?.Invoke(this, new(null));
        }

        private void Cancelar()
        {
            Modo = "Ver";
            PropertyChanged?.Invoke(this, new(null));
        }

     
        private void Guardar()
        {
            if (Seccion!=null)
            {

                if (Modo == "Editar")
                {
                    Secciones? s = repo.GetById(Seccion.Id);
                    if (s != null)
                    {
                        s.Nombre = Seccion.Nombre;
                        s.SueldoMaximo = Seccion.SueldoMaximo;
                        s.NumeroEmpleados = Seccion.NumeroEmpleados;
                        s.Empleados = Seccion.Empleados;
                        repo.EditarSeccion(s);
                        CargarSecciones();
                    }
                }

                if (Modo == "Agregar")
                {
                    repo.InsertarSeccion(Seccion);
                    CargarSecciones();
                }
                
            }
            Modo = "Ver";
            PropertyChanged?.Invoke(this, new(null));
        }

        private void VerAgregarSecciones()
        {
            Modo = "Agregar";
            Seccion = new();
            PropertyChanged?.Invoke(this, new(null));
        }

        void DescargarSecciones()
        {
            byte[] pdf = reportesSecciones.GetReporteSecciones(SeccionesDTO.ToList());
            string ruta = "ReporteSecciones.pdf";
            File.WriteAllBytes(ruta, pdf);
        }
    }
}
