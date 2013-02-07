<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.RegistrationPetition>" %>

<div class="ui-corner-all ceremony">
    
    <div class="title ui-corner-top">
        <%: string.Format("{0} ({1})", Model.Ceremony.CeremonyName, Model.Ceremony.DateTime) %>
        
        <% if (!Context.User.IsInRole("User") && !Context.User.IsInRole("Admin")) { %>
            <%: Html.ActionLink("Cancel Petition", "CancelRegistrationPetition", "Student", new { id = Model.Id }, new { @class = "button cancel-btn", style = "float:right;" })%>
            <div style="clear:both;"></div>
        <% } %>
    </div>

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
    
    <div class="foot ui-corner-bottom">
        <span><strong>Submitted:</strong> <i><%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateSubmitted) %></i></span>
        <span style="float: right;"><strong>Last Update:</strong> <i><%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateDecision) %></i></span>
    </div>
    
</div>

<style type="text/css">
    a.cancel-btn.ui-state-default { background: none rgb(185, 185, 185) !important;}
</style>