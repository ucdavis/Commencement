<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminExtraTicketPetitionViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ExtraTicketPetitions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Extra Ticket Petitions</h2>

    <% using (Html.BeginForm("ExtraTicketPetitions", "Petition", FormMethod.Get)) { %>
        Ceremony At: <%= this.Select("ceremonyId").Options(Model.Ceremonies, x=>x.Id, x=>x.DateTime.ToString("g")).Selected(Model.CeremonyId.HasValue ? Model.CeremonyId.Value : 0) %>
        <input type="submit" value="Submit" />
    <% } %>

    <% if (Model.CeremonyId.HasValue) { %>
        <% Html.Grid(Model.RegistrationParticipations)
               .Name("Participations")
               .Columns(col =>
                            {
                                col.Add(a => { %>
                                <% });
                                col.Bound(a => a.Registration.Student.LastName);
                                col.Bound(a => a.Registration.Student.FirstName);
                                col.Bound(a => a.NumberTickets).Title("# Tickets");
                                col.Bound(a => a.ExtraTicketPetition.NumberTicketsRequested).Title("# Tickets Requested");
                                col.Bound(a => a.ExtraTicketPetition.NumberTicketsRequestedStreaming).Title("# Requested Streaming");
                                col.Add(a => { %>
                                    <%: Html.TextBox("TicketsApproved", a.ExtraTicketPetition.NumberTickets, new {@class="tickets", petitionId=a.ExtraTicketPetition.Id}) %>
                                    <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading" />
                                <% }).Title("# Tickets Approved");
                                col.Add(a =>{%>
                                    <%: Html.TextBox("StreamingApproved", a.ExtraTicketPetition.NumberTicketsStreaming, new { @class = "tickets streaming", petitionId=a.ExtraTicketPetition.Id })%>
                                    <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading" />
                                <%}).Title("# Streaming Approved");
                            })
               .Render(); %>
    <% } %>

    <%= Html.AntiForgeryToken() %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            $(".tickets").blur(function () { SaveTicketAmount($(this).attr("petitionId"), $(this).val(), $(this).hasClass("streaming")); });
        });

        function SaveTicketAmount(id, amount, streaming) {
            // validate the amount is a number

            var url = '<%: Url.Action("UpdateTicketAmount", "Petition") %>';
            $.post(url, { id: id, tickets: amount, streaming: streaming, __RequestVerificationToken: antiForgeryToken = $("input[name='__RequestVerificationToken']").val() }, function (data) { 
                // something with the messages
            });
        }
    </script>

    <style = type="text/css">
        .tickets
        {
            width: 50px;
        }
        
        .loading
        {
            float:right;
            display:none;
        }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
