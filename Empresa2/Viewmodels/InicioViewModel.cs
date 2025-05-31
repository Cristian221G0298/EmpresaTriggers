using CommunityToolkit.Mvvm.Input;
using Empresa.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Empresa.Viewmodels
{
    public class InicioViewModel : INotifyPropertyChanged
    {
        public ICommand AdministrarSeccionesCommand { get; set; }
        public ICommand AdmnistrarEmpleadosCommand { get; set; }
        public ICommand IniciarCommand { get; set; }
        public ICommand ReportesCommand { get; set; }

        public UserControl Vista { get; set; } = new UserControl();

        public UserControl EstadisticasV = new EstadisticasView();
        public UserControl SeccionesV = new SeccionesView();
        public UserControl EmpleadosV = new EmpleadosView();
        public UserControl ReporteV = new ReportesView();


        public InicioViewModel() 
        {
            AdministrarSeccionesCommand = new RelayCommand(IrSecciones);
            AdmnistrarEmpleadosCommand = new RelayCommand(IrEmpleados);
            IniciarCommand = new RelayCommand(IrInicio);
            ReportesCommand = new RelayCommand(IrReportes);
            IrInicio();
        }

        private void IrReportes()
        {
            Vista = ReporteV;
            PropertyChanged?.Invoke(this, new(null));
        }

        private void IrEmpleados()
        {
            Vista = EmpleadosV;
            PropertyChanged?.Invoke(this, new(null));
        }

        private void IrInicio()
        {
            Vista = EstadisticasV;
            PropertyChanged?.Invoke(this, new(null));
        }

        private void IrSecciones()
        {
            Vista = SeccionesV;
            PropertyChanged?.Invoke(this, new(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
