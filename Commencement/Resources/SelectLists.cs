using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commencement.Resources
{
     public static class SelectLists
    {
        public static List<SelectListItem> PersonPrefixes = new List<SelectListItem>()
        {
            new SelectListItem() {Text = "", Value = ""},
            new SelectListItem() {Text = "Ms", Value = "Ms"},
            new SelectListItem() {Text = "Mrs", Value = "Mrs"},
            new SelectListItem() {Text = "Mr", Value = "Mr"},
            new SelectListItem() {Text = "Dr", Value = "Dr"},
        }; 
    }
}