using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Commencement.Jobs.Common.Logging
{
    public static class LogHelper
    {
        private static bool _loggingSetup = false;

        public static void ConfigureLogging()
        {
            if (_loggingSetup) return; //only setup logging once

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Stackify()
                .WriteTo.Console()
                .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException +=
                (sender, eventArgs) =>
                    Log.Fatal(eventArgs.ExceptionObject as Exception, eventArgs.ExceptionObject.ToString());

            _loggingSetup = true;
        }
    }
}
