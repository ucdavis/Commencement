<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Mvc.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UC Davis Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <ul class="btny">
        <li><%: Html.ActionLink<StudentController>(a=>a.VisaLetters(), "Visa Letters") %></li>
    </ul>

    <div class="content">

    <div >
        
    <h2>Commencement Participation</h2>

    <%= Model.LandingText %>
   
    <% if (Model.CanRegister()) { %>
        <span class="reg_btn button"><%: Html.ActionLink<StudentController>(a => a.Index(), "Continue")%></span>
    <% } else if ((bool)ViewData["Registered"]) { %>
        <span class="reg_btn button"><%: Html.ActionLink<StudentController>(a => a.Index(), "View Registration")%></span>
    <% }%>
</div>

<div id="col_ft">
    <p><i>After <%: Model.RegistrationDeadline.ToString("MMMM d") %> we will accept late registrations but due to printing deadlines we cannot guarantee your name will appear in the program or that you will receive the maximum number of tickets allotted per person </i></p>
</div>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<style type="text/css">
    #footer {display:none; height:0; margin-top:0;}
    .main {padding-bottom:0;}
</style>
</asp:Content>
