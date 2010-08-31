<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.ReportViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li><%= Html.ActionLink<AdminController>(a=>a.Index(), "Home")  %></li></ul>

    <h2>Reporting</h2>
    <ul class="registration_form">
        <li><strong>Term:</strong>
            <%= this.Select("termCode").Options(Model.TermCodes, x=>x.Id, x=>x.Description).Selected(Model.TermCode.Id) %>
        </li>
        
        <li><strong><%= Html.ActionLink<ReportController>(a=>a.RegistrationData(), "Registration Data") %></strong>
            Statistics of all past and present terms broken down by ceremony.
        </li>
    </ul>

    <h2>Label Printing</h2>
    <ul class="registration_form">
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, false), "Print Pending Labels")%></strong>
            This will print all pending labels that need to be printed and will update records so that they will not be printed in this list again.
        </li>
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, true), "Print All Labels")%></strong>
            This will print all labels for the current term, regardless of whether they have been printed already.
        </li>
    </ul>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
