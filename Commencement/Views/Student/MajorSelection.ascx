<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.Student>" %>
<% if (Model.Majors.Count == 1)
   { %>
   
    <% var major = Model.Majors.Single(); %>
    <span>
        <%=Html.Encode(major.Name)%>
    </span>
    <%= Html.Hidden("Registration.Major", major.Id)%>

<% }%>

<% else
    {%>

    <%= this.Select("Registration.Major").Options(Model.Majors, x=>x.Id, x=>x.Name).FirstOption("-Choose A Major-").Class("required") %>

<%
    }%>