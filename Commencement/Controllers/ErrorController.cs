using System.Web.Mvc;
using MvcContrib;

namespace Commencement.Controllers
{
    public class ErrorController : ApplicationController
    {
        //
        // GET: /Error/

        public ActionResult Index(ErrorType? errorType)
        {
            string title, description;

            // set the default error type
            if (errorType == null) { errorType = ErrorType.UnknownError; }

            switch (errorType)
            {
                case ErrorType.UnauthorizedAccess:
                    title = "Unauthorized Access";
                    description = "You are not authorized for your request.";
                    break;
                case ErrorType.FileDoesNotExist:
                    title = "File Does Not Exist";
                    description = "File not found.";
                    break;
                case ErrorType.FileNotFound:
                    title = "File Does Not Exist";
                    description = "File not found.";
                    break;
                case ErrorType.StudentNotFound:
                    title = "Record not found.";
                    description = "We were unable to find your record.  Please contact the commencement coordinator at commencement@caes.ucdavis.edu.";
                    break;
                case ErrorType.NoCeremony:
                    title = "No available ceremony found.";
                    description = "There are no available ceremonies found for your major.  Please contact the commencement coordinator at commencement@caes.ucdavis.edu.";
                    break;
                case ErrorType.RegistrationClosed:
                    title = "Registration is closed";
                    description = "Deadline has now passed.  Registration is now closed.";
                    break;
                case ErrorType.DuplicatePetition:
                    title = "Duplicate Petition";
                    description = "You have already submitted a petition.";

                    break;
                case ErrorType.SubmittedPetition:
                    title = "Existing Petition Exists";
                    description = "Our records indicate that you have already submitted a petition.";
                    break;
                case ErrorType.SJA:
                    title = "Commencement Registration for the College of Agricultural and Environmental Sciences";
                    description =
                        "According to our records you are not eligible for participation in the commencement ceremony.  If you have any questions please contact Student Judicial Affairs for further information. Thank you";
                    break;
                case ErrorType.PreviouslyWalked:
                    title = "Commencement Registration for the College of Agricultural and Environmental Sciences";
                    description = "Our records indicate that you have previously participated in a commencement ceremony.";
                    break;
                default:
                    title = "Unknown Error.";
                    description = "An unknown error has occurred.  IT has been notified of the issue.";
                    break;
            };

            return View(ErrorViewModel.Create(title, description, errorType));
        }

        public RedirectToRouteResult NotCAESStudent()
        {
            return this.RedirectToAction(a => a.Index(ErrorType.UnauthorizedAccess));
        }

        public RedirectToRouteResult SJA()
        {
            return this.RedirectToAction(a => a.Index(ErrorType.SJA));
        }

        public RedirectToRouteResult PreviouslyWalked()
        {
            return this.RedirectToAction(a => a.Index(ErrorType.PreviouslyWalked));
        }

        public enum ErrorType
        {
            UnauthorizedAccess = 0,
            FileDoesNotExist,
            FileNotFound,
            UnknownError,
            StudentNotFound,
            NoCeremony,
            DuplicatePetition,
            RegistrationClosed,
            SubmittedPetition,
            SJA,
            PreviouslyWalked
        }
    }

    public class ErrorViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ErrorController.ErrorType? ErrorType { get; set; }

        public static ErrorViewModel Create(string title, string description, ErrorController.ErrorType? errorType)
        {
            return new ErrorViewModel() { Title = title, Description = description, ErrorType = errorType};
        }
    }
}
