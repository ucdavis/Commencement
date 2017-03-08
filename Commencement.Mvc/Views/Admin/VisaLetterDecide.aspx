<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.VisaLetter>" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Request Visa Letter
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.Index(), "Home") %></li>
        <li><%: Html.ActionLink<AdminController>(a=>a.VisaLetters(null, null,null, false), "Back to list of letters") %></li>
        <li><%: Html.ActionLink<AdminController>(a=>a.VisaLetterPreviewPdf(Model.Id), "Preview (Before Any Changes)", new{target="_blank"}) %></li>
    </ul>

    <h2>Edit <%: Model.Student.FullName %></h2>
    
    <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>


    <ul class="registration_form">
        <li><strong>Student Id:</strong>

            <%: Html.ActionLink<AdminController>(a => a.StudentDetails(Model.Student.Id, false), Model.Student.StudentId) %>
        </li>

    <% Html.RenderPartial("EditVisaLetterPartial"); %>
    
    <%--TODO: Add admin only edit fields--%>
        <li><strong>Ceremony Date:</strong>
            <%= Html.TextBox("CeremonyDateTime", Model.CeremonyDateTime.HasValue ? Model.CeremonyDateTime.Value.Date.ToString("d"): string.Empty, new {@class="ceremony_time date"}) %>
            <%= Html.ValidationMessage("SpecialCheck") %>
        </li>          
        <li><strong>Decision:</strong>
                <ul>
                <li><label><%: Html.RadioButton("Decide", 'A', new{@class = "jcs"})%> Approve</label></li>
                <li><label><%: Html.RadioButton("Decide", 'D')%> Deny</label></li>
                <li><label><%: Html.RadioButton("Decide", 'N')%> Clear Decision</label></li>
                    </ul>
        </li>     
 

        <li><strong>&nbsp;</strong>
            <%: Html.SubmitButton("Submit", "Save", new { @class="button" })%>           
        </li>
        </ul>   
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>
