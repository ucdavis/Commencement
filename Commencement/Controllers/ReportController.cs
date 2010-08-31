using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class ReportController : ApplicationController
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            var viewModel = ReportViewModel.Create(Repository);
            return View(viewModel);
        }
        
        public ActionResult GenerateAveryLabels(string termCode, bool printAll)
        {
            List<Registration> registrations;

            if (printAll)
            {
                registrations = Repository.OfType<Registration>().Queryable.Where(a => a.Student.TermCode.Id == termCode).ToList();
            }
            else
            {
                registrations = Repository.OfType<Registration>().Queryable.Where(a => a.Student.TermCode.Id == termCode
                                                                                       && (!a.LabelPrinted
                                                                                           ||
                                                                                           (a.ExtraTicketPetition != null && !a.ExtraTicketPetition.LabelPrinted)
                                                                                          )).ToList();
            }

            StringBuilder labels = new StringBuilder();

            foreach(var r in registrations)
            {
                // calculate the number of tickets
                var tickets = r.LabelPrinted ? 0 : r.NumberTickets;
                tickets += r.ExtraTicketPetition != null && !r.LabelPrinted ? r.ExtraTicketPetition.NumberTickets : 0;

                if (tickets > 0)
                {
                    labels.Append(string.Format(Labels.Avery5160_LabelRow, r.Student.FullName, r.Student.StudentId
                                                , r.Major.Name, tickets, r.Student.FullName, r.Address1
                                                , r.Address2, r.City + ", " + r.State.Id + " " + r.Zip
                                                , tickets
                                      ));

                    // update the record
                    r.SetLabelPrinted();
                    Repository.OfType<Registration>().EnsurePersistent(r);
                }
            }

            var doc = string.Format(Labels.Avery5160_Doc, labels);
            doc = doc.Replace("&", "&amp;");

            ASCIIEncoding encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(doc);

            return File(bytes, "application/word", "labels.doc");
        }

        public ActionResult RegistrationData()
        {
            var viewModel = RegistrationDataViewModel.Create(Repository);
            return View(viewModel);
        }
    }
}
