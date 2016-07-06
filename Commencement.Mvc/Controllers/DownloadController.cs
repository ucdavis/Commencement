using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Mvc.Controllers
{
    public class DownloadController : ApplicationController
    {
        private readonly IRepository<Attachment> _attachmentRepository;

        public DownloadController(IRepository<Attachment> attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public ActionResult Attachment(Guid id)
        {
            var attachment = _attachmentRepository.Queryable.SingleOrDefault(a => a.PublicGuid == id);
            if (attachment == null)
            {
                return null;
            }
            return File(attachment.Contents, attachment.ContentType, attachment.FileName ?? "attachment.pdf");
        }
    }
}
