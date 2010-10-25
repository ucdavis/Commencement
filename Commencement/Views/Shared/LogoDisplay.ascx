<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.College>" %>

<% switch (Model.Id)
   {
       case "AE": { %>
        <span class="caes"><a href="http://caes.ucdavis.edu"><img src="<%= Url.Content("~/Images/logo-caes.png") %>" alt="College of Agricultural &amp; Environmental Sciences" /></a></span>
       <% }
           break;
       case "LS": { %>
        <span class="caes"><a href="http://ls.ucdavis.edu"><img src="<%= Url.Content("~/Images/logo-ls.png") %>" alt="College of Agricultural &amp; Environmental Sciences" /></a></span>
       <% }
           break;
       case "BI": { %>
        <span class="caes"><a href="http://biosci.ucdavis.edu/"><img src="<%= Url.Content("~/Images/logo-cbs.png") %>" alt="College of Agricultural &amp; Environmental Sciences" /></a></span>
       <% }
           break;
       case "EN": { %>
        <span class="caes"><a href="http://engineering.ucdavis.edu/"><img src="<%= Url.Content("~/Images/logo-coe.png") %>" alt="College of Agricultural &amp; Environmental Sciences" /></a></span>
       <% }
           break;
       case "GS": { %>
        <span class="caes"><a href="http://gradstudies.ucdavis.edu/index.cfm"><img src="<%= Url.Content("~/Images/logo-gs.png") %>" alt="College of Agricultural &amp; Environmental Sciences" /></a></span>
       <% }
           break;
   };%>

