<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.Registration>" %>


<ul class="registration_form">
    <li><strong>Address Line 1:</strong>
        <span><%= Html.Encode(Model.Address1) %></span>
    </li>
    <li><strong>Address Line 2:</strong>
        <span><%= Html.Encode(Model.Address2) %></span>
    </li>
    <li><strong>City:</strong>
        <span><%= Html.Encode(Model.City) %></span> </li>
    <li><strong>State:</strong>
        <span><%= Html.Encode(Model.State) %></span> </li>
    <li><strong>Zip Code:</strong>
        <span><%= Html.Encode(Model.Zip) %></span> </li>
    <li class="prefilled"><strong>Email Address:</strong><span>
        <%= Html.Encode(Model.Student.Email) %></span> </li>
    <li class="prefilled"><strong>Ticket Password:</strong><span>
        <%= Html.Encode(Model.TicketPassword) %></span> </li>
    <% if (!string.IsNullOrEmpty(Model.Email))
       { %>
    <li><strong>Secondary Email Address:</strong>
        <span><%= Html.Encode(Model.Email)%></span>
    </li>
    <% } %>
    <li><strong>How To Say Your Name Phonetically:</strong>
        <span><%= Html.Encode(Model.Phonetic)%></span>
    </li>
    <li><strong>Cell Phone for Texts (Data rates may Apply):</strong>
        <% if (!string.IsNullOrWhiteSpace(Model.CellNumberForText)) { %>
        <span><%= Html.Encode(Model.CellNumberForText) %></span>
        <% } else { %>
        <span>Not Supplied</span>
        <% } %>
    </li>
    <li>
        <strong>Special Needs: </strong>
        <%: string.Join(", ", Model.SpecialNeeds.Select(a=>a.Name)) %>
    </li>
    
</ul>