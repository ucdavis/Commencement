<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Term Code Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul class="btn">
    <li>
        <%= Html.ActionLink<TermCodeController>(a => a.Index() , "Back to List") %>
    </li>
</ul>

<div class="page_bar">
    <div class="col1"><h2>Details for <%= Html.Encode(Model.Id) %></h2></div>
    <div class="col2"><%= Html.ActionLink<TermCodeController>(a => a.Edit(Model.Id) , "Edit", new{@class="button"}) %></div>
</div>

<fieldset>
        
    <legend>Details</legend>
        
    <ul class="registration_form">
        <li>
            <strong><%: Html.LabelFor(x=>x.Name, DisplayOptions.HumanizeAndColon) %></strong>
            <%=Html.DisplayFor(a => a.Name) %>
        </li>
        <li>
            <strong><%=Html.LabelFor(x=>x.IsActive, DisplayOptions.HumanizeAndColon)%></strong>
            <%: Model.IsActive ? "Yes" : "No" %>
        </li>
        <li>
            <strong>Registration Dates:</strong>                
            <%: string.Format("{0} to {1}", Model.RegistrationBegin.ToString("d"), Model.RegistrationDeadline.ToString("d")) %>
        </li>
        <li>
            <strong><%: Html.LabelFor(x=>x.FileToGraduateDeadline, DisplayOptions.HumanizeAndColon) %></strong>
            <%: Model.FileToGraduateDeadline.ToString("d") %>
        </li>
        <li>
            <strong><%: Html.LabelFor(x=>x.CapAndGownDeadline, DisplayOptions.HumanizeAndColon) %></strong>
            <%: Model.CapAndGownDeadline %>
        </li>
    </ul>

</fieldset>

<fieldset>
    <legend>Landing Text</legend>
    
    <div class="content">
        <%= Model.LandingText %>
    </div>
    
</fieldset>

<fieldset>
    <legend>Registration Welcome</legend>
    <%= Model.RegistrationWelcome %>
</fieldset>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

