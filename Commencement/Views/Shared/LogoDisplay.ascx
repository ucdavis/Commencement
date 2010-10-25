<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.College>" %>

<% if (Model.Id == "AE") { %>
<span class="caes"><a href="http://caes.ucdavis.edu"><img src="<%= Url.Content("~/Images/logo-caes.png") %>" alt="College of Agricultural &amp; Environmental Sciences" /></a></span>
<% } %>