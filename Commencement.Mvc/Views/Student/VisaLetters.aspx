<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IList<Commencement.Core.Domain.VisaLetter>>" %>
<%@ Import Namespace="Commencement.Mvc.Controllers" %>
<%@ Import Namespace="Commencement.Mvc.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Visa Letters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<HomeController>(a=>a.Index(), "Home") %></li>
    </ul>

    <div>

            <strong>
                Welcome to the student visa letter support system. To submit requests click the Request New Letter button. Remember you must submit a new request for each individual person you need a letter for. The history and status of your past requests can be found in the table below. Thank you.
            </strong> 

    </div>

    <div class="page_bar">
        <div class="col1"><h2>Visa Letters</h2></div>
        <div class="col2">
            <%: Html.ActionLink<StudentController>(a => a.RequestVisaLetter(), "Request New Letter", new { @class = "button" })%>
        </div>
    </div>
    

    
    <%: Html.AntiForgeryToken() %>

    <% Html.Grid(Model)
           .Name("VisaLetters")
           .Columns(col =>
           {
                col.Add(a=> {%>
                    <% if (a.IsApproved){ %>
                        <%= Html.ActionLink<StudentController>(b => b.VisaLetterPdf(a.Id), "Print Letter") %>
                        |
                    <% } %>
                    <%= Html.ActionLink<StudentController>(b => b.VisaLetterDetails(a.Id), "Details") %>
                    <% });
                col.Bound(a => a.RelativeFirstName);
                col.Bound(a => a.RelativeLastName);
                col.Bound(a => a.DateCreated);
                col.Bound(a => a.IsApproved);
                col.Bound(a => a.IsDenied);
                col.Add(a=> {%>
                    <% if (a.IsPending){ %>
                        <%= Html.ActionLink<StudentController>(b => b.CancelVisaLetterRequest(a.Id), "Cancel Request") %>
                    <% } %>
                    <% });
                        })
           .Render();
        %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">


    <style type="text/css">
        .cancel { cursor: pointer;}
    </style>

</asp:Content>
