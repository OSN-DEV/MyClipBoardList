using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyClipBoardList {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {

        #region Declaration
        private Mutex mutex = new Mutex(false, "MyApps");
        #endregion

        #region Event
        private void Application_Startup(object sender, StartupEventArgs e) {
            var name = this.GetType().Assembly.GetName().Name;
            mutex = new Mutex(false, name);
            if (!mutex.WaitOne(TimeSpan.Zero, false)) {
                mutex.Close();
                this.Shutdown();
                return;
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e) {
            if (mutex != null) {
                mutex.ReleaseMutex();
                mutex.Close();
            }
        }
        #endregion
    }
}
