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

        #region Label Generator
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

            var doc = GenerateLabelDoc(registrations, printAll);

            ASCIIEncoding encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(doc);

            return File(bytes, "application/word", "labels.doc");
        }

        private string GenerateLabelDoc(List<Registration> registrations, bool printAll)
        {
            var labels = new StringBuilder();
            var rows = GenerateRows(registrations, printAll);
            foreach (var r in rows)
            {
                // put all 3 cells into a row
                labels.Append(string.Format(Labels.Avery5160_LabelRow, r.GetCell1, r.GetCell2, r.GetCell3));
            }

            var doc = string.Format(Labels.Avery5160_Doc, labels);
            doc = doc.Replace("&", "&amp;");

            return doc;
        }

        // create a list of the data broken down into rows
        private List<LabelRow> GenerateRows(List<Registration> registrations, bool printAll)
        {
            var rows = new List<LabelRow>();
            var row = new LabelRow();

            foreach (var reg in registrations)
            {
                // if no space is avaible add it to the list
                if (!row.HasSpace())
                {
                    rows.Add(row);
                    row = new LabelRow();
                }

                // calculate the number of tickets
                var tickets = !reg.LabelPrinted || printAll ?  reg.NumberTickets : 0;
                tickets += reg.ExtraTicketPetition != null && (!reg.LabelPrinted || printAll) ? reg.ExtraTicketPetition.NumberTickets : 0;

                if (tickets > 0)
                {
                    string cell = string.Empty;

                    if (reg.MailTickets)
                    {
                        var address2 = string.IsNullOrEmpty(reg.Address2) ? string.Empty : string.Format(Labels.Avergy5160_Mail_Address2, reg.Address2);
                        cell = string.Format(Labels.Avery5160_MailCell, reg.Student.FullName, reg.Address1,
                                                 address2, reg.City + ", " + reg.State.Id + " " + reg.Zip, tickets);


                    }
                    else
                    {
                        cell = string.Format(Labels.Avery5160_PickupCell, reg.Student.FullName,
                                             reg.Student.StudentId, reg.Major.Name, tickets);
                    }

                    row.AddCell(cell);
                }
            }

            return rows;
        }
        #endregion

        public ActionResult RegistrationData()
        {
            var viewModel = RegistrationDataViewModel.Create(Repository);
            return View(viewModel);
        }
    }

    public class LabelRow
    {
        public LabelRow()
        {
            Cell1 = string.Empty;
            Cell2 = string.Empty;
            Cell3 = string.Empty;
        }

        public bool HasSpace()
        {
            return string.IsNullOrEmpty(Cell1) || string.IsNullOrEmpty(Cell2) || string.IsNullOrEmpty(Cell3);
        }
        public bool AddCell(string contents)
        {
            if (string.IsNullOrEmpty(Cell1)) {
                Cell1 = contents;
                return true;
            }
            if (string.IsNullOrEmpty(Cell2)) {
                Cell2 = contents;
                return true;
            }
            if (string.IsNullOrEmpty(Cell3))
            {
                Cell3 = contents;
                return true;
            }

            return false;
        }

        public string GetCell1 { get { return string.IsNullOrEmpty(Cell1) ? Labels.Avery5160_EmptyCell : Cell1; }}
        public string GetCell2 { get { return string.IsNullOrEmpty(Cell2) ? Labels.Avery5160_EmptyCell : Cell2; } }
        public string GetCell3 { get { return string.IsNullOrEmpty(Cell3) ? Labels.Avery5160_EmptyCell : Cell3; } }

        public string Cell1 { get; set; }
        public string Cell2 { get; set; }
        public string Cell3 { get; set; }
    }
}
