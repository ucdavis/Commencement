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
        <% if (!string.IsNullOrEmpty(Model.CellNumberForText))
       { %>
    <li><strong>Cell Number For Texting:</strong>
        <span><%= Html.Encode(Model.CellNumberForText)%></span> <span><%=Html.Encode(Model.CellCarrier) %></span><%-- //TODO: Nicer Display--%>
    </li>
    <% } %>
    <li><strong>How To Say Your Name Phonetically:</strong>
        <span><%= Html.Encode(Model.Phonetic)%></span>
    </li>
    <li>
        <strong>Special Needs: </strong>
        <%: string.Join(", ", Model.SpecialNeeds.Select(a=>a.Name)) %>
    </li>
    
</ul>