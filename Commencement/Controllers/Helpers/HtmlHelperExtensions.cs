using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commencement.Helpers;
using Telerik.Web.Mvc.UI;

namespace Commencement.Controllers.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static CustomGridBuilder<T> Grid<T>(this HtmlHelper htmlHelper, IEnumerable<T> dataModel) where T : class
        {
            var builder = htmlHelper.Telerik().Grid(dataModel);

            return new CustomGridBuilder<T>(builder);
        }
    }
}
