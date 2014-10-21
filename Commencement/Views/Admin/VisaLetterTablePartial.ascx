<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IList<Commencement.Core.Domain.VisaLetter>>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="NPOI.SS.Formula.Functions" %>

    <% Html.Grid(Model)
           .Name("VisaLetters")
           .Columns(col =>
                        {
                            col.Add(a=> {%>
                                <%= Html.ActionLink<AdminController>(b => b.VisaLetterDetails(a.Id), "Details") %>
                                <% if (!a.IsCanceled){ %>
                                    | <%= Html.ActionLink<AdminController>(b => b.VisaLetterDecide(a.Id), "Decide") %>
                                <% } %>
                                
                            <% });
                            col.Bound(a => a.Student.StudentId); //Remove once testing is done
                            col.Bound(a => a.Student.TotalUnits);
                            col.Bound(a => a.Student.FullName).Title("Name");
                            col.Bound(a => a.CollegeCode);
                            col.Bound(a => a.ApprovedBy);
                            col.Bound(a => a.DateDecided);
                            col.Bound(a => a.DateCreated);
                            col.Bound(a => a.Status);                            
                        })
           .Sortable()
           .Render();
        %>