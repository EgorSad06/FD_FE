using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FD_MainWindow
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        //Для обучения (вывод ошибок привязки)
        protected override void OnStartup(StartupEventArgs e)
        {
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
            base.OnStartup(e);
        }
    }
}
