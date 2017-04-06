<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.EmailQueue>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Email Queue Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page_bar">
        <div class="col1"><h2>Email Queue Details</h2></div>
        <div class="col2">
            <%if(ViewBag.fromStudent != null && ViewBag.fromStudent == true) {%>
            <%: Html.ActionLink("Back to Student List", "AllStudentEmail", "EmailQueue", new {id = ViewBag.studentId}, new {@class="button"}) %>
            <% } else {%>
            <%: Html.ActionLink("Back to List", "Index", "EmailQueue",null, new {@class="button"}) %>
            <% } %>
        </div>
    </div>
    
    <fieldset>
        
        <legend>Details</legend>
        
        <ul class="registration_form">
            <li class="prefilled"><strong>Student Id:</strong>
                <%: Model.Student.StudentId %>
            </li>
            <li class="prefilled"><strong><%: Html.LabelFor(x => x.Student, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Model.Student.FullName %>
            </li>
            <li class="prefilled"><strong><%: Html.LabelFor(x => x.Created, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Model.Created %>
            </li>
            <li class="prefilled"><strong>Status:</strong>
                <%: Model.SentDateTime.HasValue ? string.Format("Sent on {0}",Model.SentDateTime.ToString()) : "Pending" %>
            </li>
        </ul>

    </fieldset>

    <fieldset>
        
        <legend>Subject</legend>
        
        <div class="content"><%: Model.Subject %></div>

    </fieldset>
    
    <fieldset>
        
        <legend>Body</legend>
        
        <div class="content"><%= Model.Body %></div>

    </fieldset>
    
    <%if(Model.Attachment != null) {%>
        <fieldset>
        
        <legend>Attachment</legend>
        
        <div class="content"><%= Html.ActionLink<EmailQueueController>(b => b.AttachmentDetails(Model.Id), "Attachment", new {@class="button"}) %></div>

    </fieldset>
        
    <%}%>
    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
