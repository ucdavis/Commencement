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

        public static List<SelectListItem> CollegeNames = new List<SelectListItem>()
        { //Use the college name in the database instead? This probably ensures correct text on the letter.
            new SelectListItem() {Text = "", Value = ""},
            new SelectListItem() {Value = "AE", Text = "Agricultural and Environmental Sciences"},
            new SelectListItem() {Value = "BI", Text = "Biological Sciences"},
            new SelectListItem() {Value = "EN", Text = "Engineering"},
            new SelectListItem() {Value = "GS", Text = "Graduate Studies"},
            new SelectListItem() {Value = "LS", Text = "Letters & Science"},
            new SelectListItem() {Value = "LW", Text = "Law"},
            new SelectListItem() {Value = "MD", Text = "Medicine"},
            new SelectListItem() {Value = "SM", Text = "School of Management"},
            new SelectListItem() {Value = "UE", Text = "University Extension"},
            new SelectListItem() {Value = "VM", Text = "Veterinary Medicine"},
        };

        public static List<SelectListItem> DegreeType = new List<SelectListItem>()
        {
            new SelectListItem() {Text = "", Value = ""},
            new SelectListItem() {Text = "Bachelor of Arts", Value = "Bachelor of Arts"},
            new SelectListItem() {Text = "Bachelor of Science", Value = "Bachelor of Science"},
            new SelectListItem() {Text = "Bachelor of Arts and Science", Value = "Bachelor of Arts and Science"},
        };
    }
}