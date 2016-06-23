<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.TemplateType>" %>

<!-- Registration Confirmation -->
<div id="RC" class="tokens" style='<%: Model != null && Model.Code == "RC" ? "display:block;" : "display:none;" %>'>
    <li><a href="javascript:;" class="add_token">{StudentId}</a></li>
    <li><a href="javascript:;" class="add_token">{StudentName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyTime}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyLocation}</a></li>
    <li><a href="javascript:;" class="add_token">{NumberOfTickets}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine1}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine2}</a></li>
    <li><a href="javascript:;" class="add_token">{City}</a></li>
    <li><a href="javascript:;" class="add_token">{State}</a></li>
    <li><a href="javascript:;" class="add_token">{Zip}</a></li>
    <li><a href="javascript:;" class="add_token">{DistributionMethod}</a></li>
    <li><a href="javascript:;" class="add_token">{SpecialNeeds}</a></li>
    <li><a href="javascript:;" class="add_token">{Major}</a></li>
    <li><a href="javascript:;" class="add_token">{Status}</a></li>
</div>
<!-- Registration Petition -->
<div id="RP" class="tokens" style='<%: Model != null && Model.Code == "RP" ? "display:block;" : "display:none;" %>'>
    <li><a href="javascript:;" class="add_token">{StudentId}</a></li>
    <li><a href="javascript:;" class="add_token">{StudentName}</a></li>
    <li><a href="javascript:;" class="add_token">{ExceptionReason}</a></li>
    <li><a href="javascript:;" class="add_token">{CompletionTerm}</a></li>
</div>
<!-- Ticket Petition -->
<div id="TP" class="tokens" style='<%: Model != null && Model.Code == "TP" ? "display:block;" : "display:none;" %>'>
    <li><a href="javascript:;" class="add_token">{StudentId}</a></li>
    <li><a href="javascript:;" class="add_token">{StudentName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyTime}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyLocation}</a></li>
    <li><a href="javascript:;" class="add_token">{NumberOfTickets}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine1}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine2}</a></li>
    <li><a href="javascript:;" class="add_token">{City}</a></li>
    <li><a href="javascript:;" class="add_token">{State}</a></li>
    <li><a href="javascript:;" class="add_token">{Zip}</a></li>
    <li><a href="javascript:;" class="add_token">{DistributionMethod}</a></li>
    <li><a href="javascript:;" class="add_token">{SpecialNeeds}</a></li>
    <li><a href="javascript:;" class="add_token">{Major}</a></li>
    <li><a href="javascript:;" class="add_token">{NumberOfExtraTickets}</a></li>
    <li><a href="javascript:;" class="add_token">{PetitionDecision}</a></li>
</div>
<!-- Registration Petition - Approved -->
<div id="RA" class="tokens" style='<%: Model != null && Model.Code == "RA" ? "display:block;" : "display:none;" %>'>
    <li><a href="javascript:;" class="add_token">{StudentId}</a></li>
    <li><a href="javascript:;" class="add_token">{StudentName}</a></li>
    <li><a href="javascript:;" class="add_token">{ExceptionReason}</a></li>
    <li><a href="javascript:;" class="add_token">{CompletionTerm}</a></li>
    <li><a href="javascript:;" class="add_token">{PetitionDecision}</a></li>
</div>
<!-- Ticket Petition - Decision -->
<div id="TD" class="tokens" style='<%: Model != null && Model.Code == "TD" ? "display:block;" : "display:none;" %>'>
<li><a href="javascript:;" class="add_token">{StudentId}</a></li>
    <li><a href="javascript:;" class="add_token">{StudentName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyTime}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyLocation}</a></li>
    <li><a href="javascript:;" class="add_token">{NumberOfTickets}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine1}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine2}</a></li>
    <li><a href="javascript:;" class="add_token">{City}</a></li>
    <li><a href="javascript:;" class="add_token">{State}</a></li>
    <li><a href="javascript:;" class="add_token">{Zip}</a></li>
    <li><a href="javascript:;" class="add_token">{DistributionMethod}</a></li>
    <li><a href="javascript:;" class="add_token">{SpecialNeeds}</a></li>
    <li><a href="javascript:;" class="add_token">{Major}</a></li>
    <li><a href="javascript:;" class="add_token">{NumberOfExtraTickets}</a></li>
    <li><a href="javascript:;" class="add_token">{PetitionDecision}</a></li>
</div>
<!-- Move Major -->
<%--<div id="MM" class="tokens" style='<%: Model != null && Model.Code == "MM" ? "display:block;" : "display:none;" %>'>--%>
<div id="MM"></div>
    <li><a href="javascript:;" class="add_token">{StudentId}</a></li>
    <li><a href="javascript:;" class="add_token">{StudentName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyName}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyTime}</a></li>
    <li><a href="javascript:;" class="add_token">{CeremonyLocation}</a></li>
    <li><a href="javascript:;" class="add_token">{NumberOfTickets}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine1}</a></li>
    <li><a href="javascript:;" class="add_token">{AddressLine2}</a></li>
    <li><a href="javascript:;" class="add_token">{City}</a></li>
    <li><a href="javascript:;" class="add_token">{State}</a></li>
    <li><a href="javascript:;" class="add_token">{Zip}</a></li>
    <li><a href="javascript:;" class="add_token">{DistributionMethod}</a></li>
    <li><a href="javascript:;" class="add_token">{SpecialNeeds}</a></li>
    <li><a href="javascript:;" class="add_token">{Major}</a></li>
    <li><a href="javascript:;" class="add_token">{Status}</a></li>
</div>