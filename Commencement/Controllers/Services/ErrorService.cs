using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commencement.Controllers.Services
{
    public interface IErrorService
    {
        void ReportError(Exception ex);
    }

    public class ErrorService : IErrorService
    {
        private delegate void BeginReportErrorHandlerDelegate(Exception ex);

        public void ReportError(Exception ex)
        {
            var deligate = new BeginReportErrorHandlerDelegate(BeginReportErrorHandler);
            var callback = new AsyncCallback(EndReportErrorHandler);

            deligate.BeginInvoke(ex, callback, null);
        }

        public static void BeginReportErrorHandler(Exception ex)
        {
            throw ex;
        }

        public static void EndReportErrorHandler(IAsyncResult ar) {}
    }
}