using CommunityToolkit.Mvvm.Input;
using Empresa2.Models;
using Empresa.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Empresa.Viewmodels
{
    public class EstadisticasViewmodel : INotifyPropertyChanged
    {
        SeccionesRepository repoSecciones = new();
        EmpleadosRepository repoEmpleados = new();
        public int NumeroSecciones => Secciones.Count();
       public int NumeroEmpleados {  get; set; }
       public decimal MontoSueldos { get; set; }
        public List<Secciones> Secciones { get; set; } = new();
        public ICommand VerEmpleadosCommand { get; set; }
        public List<Empleados> Empleados { get; set; } = new();

        public EstadisticasViewmodel()
        {
            VerEmpleadosCommand = new RelayCommand<Secciones>(VerEmpleados);

            Secciones = repoSecciones.GetAllSecciones().ToList();
            NumeroEmpleados = repoEmpleados.TotalEmpleados;
            MontoSueldos = repoEmpleados.MontoSueldos;
            PropertyChanged?.Invoke(this, new(null));
        }

        private void VerEmpleados(Secciones? s)
        {
            if (s != null)
            Empleados = repoSecciones.GetEmpleadosBySeccion(s).ToList();

            PropertyChanged?.Invoke(this, new(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
