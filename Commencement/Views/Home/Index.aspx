<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UC Davis Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

    <div class="left_col">
        
    <h2>Commencement Participation</h2>

    <%= Model.LandingText %>
   
    <% if (Model.CanRegister()) { %>
        <span class="reg_btn button"><%: Html.ActionLink<StudentController>(a => a.Index(), "Continue")%></span>
    <% } else if ((bool)ViewData["Registered"]) { %>
        <span class="reg_btn button"><%: Html.ActionLink<StudentController>(a => a.Index(), "View Registration")%></span>
    <% }%>
</div>

<div class="right_col">
<h3>Dates</h3>
<ul>
    <li><strong>February 1 - April 12, 2013</strong>
        File for candidacy period for Spring 2013 graduation with the Office of the <a href="http://registrar.ucdavis.edu/">University Registrar</a>
    </li>
    <li><strong>March 1 – TBD</strong>
        Online pre-order period for <a href="http://bookstore.ucdavis.edu/graduation/">cap and gown</a> rental through the UC Davis Stores
    </li>
    <li><strong>Tuesday, March 5th 9am-6pm</strong>
        Grad Faire in the Griffin Lounge, Memorial Union
    </li>
    <li><strong>Friday, April 12th by 5pm</strong>
        <a href="http://commencement.ucdavis.edu/general_info/index.html#speaker">Student Speaker</a> Applications due
    </li>
</ul>
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
