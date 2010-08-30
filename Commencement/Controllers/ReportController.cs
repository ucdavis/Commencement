using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers
{
    public class ReportController : ApplicationController
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            var viewModel = ReportViewModel.Create(Repository);
            return View(viewModel);
        }

        public ActionResult GenerateAveryLabels(string termCode)
        {
            var registrations = Repository.OfType<Registration>().Queryable.Where(a => a.Student.TermCode.Id == termCode);

            StringBuilder labels = new StringBuilder();

            foreach(var r in registrations)
            {
                labels.Append(string.Format(Labels.Avery5160_LabelRow, r.Student.FullName, r.Student.StudentId
                                            , r.Major.Name, r.TotalTickets, r.Student.FullName, r.Address1, r.Address2, r.City + ", " + r.State.Id + " " + r.Zip
                                            , r.TotalTickets
                                  ));
            }

            var doc = string.Format(Labels.Avery5160_Doc, labels);
            doc = doc.Replace("&", "&amp;");

            ASCIIEncoding encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(doc);

            return File(bytes, "application/word", "labels.doc");
        }
    }

    public class ReportViewModel
    {
        public IEnumerable<vTermCode> TermCodes { get; set; }
        public TermCode TermCode { get; set; }

        public static ReportViewModel Create(IRepository repository)
        {
            var viewModel = new ReportViewModel()
                                {
                                    TermCodes = repository.OfType<vTermCode>().Queryable.Where(a => a.StartDate < DateTime.Now),
                                    TermCode = TermService.GetCurrent()
                                };

            return viewModel;
        }
    }
}
