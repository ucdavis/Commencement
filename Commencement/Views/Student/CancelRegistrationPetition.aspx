<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.RegistrationPetition>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Cancel Registration Petition
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Cancel Registration Petition</h2>
    
    <% using (Html.BeginForm()) { %>

        <%: Html.AntiForgeryToken() %>

        <fieldset>
            <legend>Petition Details</legend>
        
            <ul class="registration_form">
                <li><strong>Date Submitted:</strong>
                    <%= Html.Encode(Model.DateSubmitted.ToString("g")) %></li>
                <li><strong>Date Decision:</strong>
                    <%= Html.Encode(Model.DateDecision != null ? Model.DateDecision.ToString() : string.Empty) %></li>
                <li><strong>Approved:</strong>
                    <%: Model.Status %></li>
                <li><strong>Reason for Petition:</strong>
                    <%= Html.Encode(Model.ExceptionReason) %></li>
                <li><strong>Transfer Units From*:</strong>
                    <%= Html.Encode(Model.TransferUnitsFrom) %></li>
                <li><strong>Transfer Units*:</strong>
                    <%= Html.Encode(Model.TransferUnits) %></li>
            </ul>
        </fieldset>
    
        <fieldset>
            <legend>Confirm Action</legend>
        
            <h2>Are you sure you want to cancel your registration petition?</h2>

            <ul style="list-style-type: none;">
                <li><label><%: Html.RadioButton("Cancel", true)%> Yes, I would like to cancel my petition</label></li>
                <li><label><%: Html.RadioButton("Cancel", false)%> No, I would like to keep my petition</label></li>
            </ul>
        
            <ul class="registration_form" style="margin-top: 1em;">
                <li><strong>&nbsp;</strong>
                    <input type="submit" value="Confirm" class="button" />            
                    <%: Html.ActionLink("Cancel", "DisplayRegistration") %>
                </li>
            </ul>

        </fieldset>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
