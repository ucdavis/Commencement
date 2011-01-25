<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.RegistrationPetition>" %>

    <ul class="registration_form">
        <li><strong>Date Submitted:</strong><%= Html.Encode(Model.DateSubmitted.ToString("g")) %></li>
        <li><strong>Date Decision:</strong><%= Html.Encode(Model.DateDecision != null ? Model.DateDecision.ToString() : string.Empty) %></li>
        <li><strong>Approved:</strong><%: Model.Status %></li>
        <li><strong>Reason for Petition:</strong><%= Html.Encode(Model.ExceptionReason) %></li>
        <li><strong>Term to Complete:</strong><%= Html.Encode(Model.TermCodeComplete.Description) %></li>
        <li><strong>Transfer Units From*:</strong><%= Html.Encode(Model.TransferUnitsFrom) %></li>
        <li><strong>Transfer Units*:</strong><%= Html.Encode(Model.TransferUnits) %></li>
    </ul>