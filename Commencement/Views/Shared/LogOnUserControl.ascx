<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        Welcome <b><%= Html.Encode(Page.User.Identity.Name) %></b>!
        
        <% if ((bool?)HttpContext.Current.Session["Emulation"] == null || (bool?)HttpContext.Current.Session["Emulation"] == false)
           { %>
        
        [ <%= Html.ActionLink("Log Off", "LogOut", "Account")%> ]
        
        <% } else { %> 
        
        [ <%= Html.ActionLink("End Emulation", "EndEmulate", "Account") %> ]
        
        <% } %>
        
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Log On", "LogOn", "Account") %> ]
<%
    }
%>
