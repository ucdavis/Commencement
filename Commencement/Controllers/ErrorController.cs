using System.Web.Mvc;

namespace Commencement.Controllers
{
    public class ErrorController : Controller
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
                    title = "Student not found";
                    description = "The student you are looking for was not found.";
                    break;
                case ErrorType.NoCeremony:
                    title = "No available ceremony found.";
                    description = "There are no available ceremonies found for your major.  Please contact .....";
                    break;
                default:
                    title = "Unknown Error.";
                    description = "An unknown error has occurred.  IT has been notified of the issue.";
                    break;
            };

            return View(ErrorViewModel.Create(title, description, errorType));
        }

        public enum ErrorType
        {
            UnauthorizedAccess = 0,
            FileDoesNotExist,
            FileNotFound,
            UnknownError,
            StudentNotFound,
            NoCeremony
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
