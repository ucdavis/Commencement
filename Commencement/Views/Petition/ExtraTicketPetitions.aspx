<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminExtraTicketPetitionViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Extra Ticket Petitions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<PetitionController>(a=>a.Index(), "Back to Petitions") %></li>
    </ul>

    <h2>Extra Ticket Petitions</h2>

    <% using (Html.BeginForm("ExtraTicketPetitions", "Petition", FormMethod.Get)) { %>
        Ceremony At: <%= this.Select("ceremonyId").Options(Model.Ceremonies, x=>x.Id, x=>x.DateTime.ToString("g")).Selected(Model.Ceremony != null ? Model.Ceremony.Id : 0) %>
        <input type="submit" value="Submit" />
    <% } %>

    <% if (Model.Ceremony != null) { %>
        
        <% using (Html.BeginForm("ApproveAllExtraTicketPetition", "Petition", FormMethod.Post))
           { %>
        
            <%= Html.AntiForgeryToken() %>
            <%: Html.Hidden("id", Model.Ceremony.Id) %>
            <%: Html.SubmitButton("ApproveAll", "Approve All") %>

        <% }%>

        <div id="ticketCountInline">
            <ul>
                <li><strong>Projected Available:</strong> <span class="projectedAvailabeTickets"><%: Model.Ceremony.ProjectedAvailableTickets %></span></li>
                <% if (Model.Ceremony.HasStreamingTickets) { %><li><strong>Projected Streaming Available:</strong> <span class="projectedAvailableStreaming"><%: Model.Ceremony.ProjectedAvailableStreamingTickets.HasValue ? Model.Ceremony.ProjectedAvailableStreamingTickets.Value.ToString() : "n/a" %></span></li><% } %>
                <li><strong>Projected Ticket Count:</strong> <span class="projectedTickets"><%: Model.Ceremony.ProjectedTicketCount %></span></li>
                <% if (Model.Ceremony.HasStreamingTickets) { %><li><strong>Projected Streaming Count:</strong> <span class="projectedStreaming"><%: Model.Ceremony.ProjectedTicketStreamingCount.HasValue ? Model.Ceremony.ProjectedTicketStreamingCount.Value.ToString() : "n/a" %></span></li><% } %>
            </ul>
        </div>
        <div id="ticketCountBar">
            <ul>
                <li><strong>Projected Available:</strong> <span class="projectedAvailabeTickets"><%: Model.Ceremony.ProjectedAvailableTickets %></span></li>
                <% if (Model.Ceremony.HasStreamingTickets) { %><li><strong>Projected Streaming Available:</strong> <span class="projectedAvailableStreaming"><%: Model.Ceremony.ProjectedAvailableStreamingTickets.HasValue ? Model.Ceremony.ProjectedAvailableStreamingTickets.Value.ToString() : "n/a" %></span></li><% } %>
                <li><strong>Projected Ticket Count:</strong> <span class="projectedTickets"><%: Model.Ceremony.ProjectedTicketCount %></span></li>
                <% if (Model.Ceremony.HasStreamingTickets) { %><li><strong>Projected Streaming Count:</strong> <span class="projectedStreaming"><%: Model.Ceremony.ProjectedTicketStreamingCount.HasValue ? Model.Ceremony.ProjectedTicketStreamingCount.Value.ToString() : "n/a" %></span></li><% } %>
            </ul>
        </div>
    
    
        <% Html.Grid(Model.RegistrationParticipations)
               .Name("Participations")
               .Columns(col =>
                            {
                                col.Add(a => { %>
                                    <img src='<%: Url.Content("~/Images/CheckMark-1.png") %>' class="decision" data-approve="true" data-participationId='<%: a.Id %>' />
                                    <img src='<%: Url.Content("~/Images/Cancel-1.png") %>' class="decision" data-approve="false" data-participationId='<%: a.Id %>' />
                                <% }).HtmlAttributes(new {Style = "width:50px;"});
                                col.Bound(a => a.Registration.Student.LastName);
                                col.Bound(a => a.Registration.Student.FirstName);
                                col.Bound(a => a.NumberTickets).Title("# Tickets");
                                col.Add(a=>{ %>
                                    <img src='<%: Url.Content("~/Images/speechbubble.jpg") %>' class="reason" />
                                    <div class="reasonText" style="display:none;"><%: !string.IsNullOrEmpty(a.ExtraTicketPetition.Reason) ? a.ExtraTicketPetition.Reason : "n/a" %></div>
                                <%}).Title("Reason");
                                col.Bound(a => a.ExtraTicketPetition.NumberTicketsRequested).Title("# Requested");
                                if (Model.Ceremony.HasStreamingTickets){col.Bound(a => a.ExtraTicketPetition.NumberTicketsRequestedStreaming).Title("# Streaming");}
                                col.Add(a => { %>
                                    <%: Html.TextBox("TicketsApproved", a.ExtraTicketPetition.NumberTickets.HasValue ? a.ExtraTicketPetition.NumberTickets : a.ExtraTicketPetition.NumberTicketsRequested, new {@class="tickets", participationId=a.Id, ceremonyId=a.Ceremony.Id}) %>
                                    <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading" />
                                    <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="cancel" />
                                    <img src="<%: Url.Content("~/Images/CheckMark-1.png") %>" class="check" />
                                <% }).Title("# Approved");
                                if (Model.Ceremony.HasStreamingTickets)
                                {
                                    col.Add(a => {%>
                                        <%: Html.TextBox("StreamingApproved", a.ExtraTicketPetition.NumberTicketsStreaming.HasValue ? a.ExtraTicketPetition.NumberTicketsStreaming.Value : a.ExtraTicketPetition.NumberTicketsRequestedStreaming, new { @class = "tickets streaming", participationId = a.Id, ceremonyId = a.Ceremony.Id })%>
                                        <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading" />
                                        <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="cancel" />
                                        <img src="<%: Url.Content("~/Images/CheckMark-1.png") %>" class="check" />
                                    <%}).Title("# Streaming Approved");
                                }
                            })
               .Render(); %>
    <% } %>

    <%= Html.AntiForgeryToken() %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript" src='<%: Url.Content("~/Scripts/jquery.bt.min.js") %>'></script>
    <link href="<%= Url.Content("~/Content/jquery.bt.css") %>" rel="Stylesheet" type="text/css" media="screen" />

    <script type="text/javascript">

        $(document).ready(function () {
            $(".tickets").blur(function () { SaveTicketAmount($(this).attr("participationId"), $(this).attr("ceremonyId"), $(this).val(), $(this).hasClass("streaming"), this); });
            //$(".decision").click(function () { MakeDecision($(this).attr("participationId"), $(this).val() == "Approve", this); });

            $(".decision").click(function () { MakeDecision($(this).data("participationId"), $(this).data("approve"), $(this)); });

            SetScrollingBox($("#ticketCountBar"), $("#ticketCountInline"));

            $(".reason").each(function (index, item) { $(item).bt($(item).siblings("div.reasonText").html()); });
        });

        function SaveTicketAmount(id, ceremonyId, amount, streaming, box) {
            // validate the amount is a number

            $(box).siblings(".loading").show();

            var url = '<%: Url.Action("UpdateTicketAmount", "Petition") %>';
            $.post(url, { id: id, tickets: amount, ceremonyId: ceremonyId, streaming: streaming, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
                // something with the messages
                $(box).siblings(".loading").fadeOut(2000);

                if (data.Message != null) {
                    $(box).siblings(".cancel").show();
                }
                else {
                    $(box).siblings(".check").show().delay(5000).fadeOut(2000);
                    UpdateCounts(data);
                }

            });
        }

        function UpdateCounts(data) {
            $(".projectedAvailabeTickets").html(data.ProjectedAvailableTickets);
            if (data.ProjectedAvailableStreamingTickets == null) {
                $(".projectedAvailableStreaming").html("n/a");
            } else {
                $(".projectedAvailableStreaming").html(data.ProjectedAvailableStreamingTickets);
            }
            $(".projectedTickets").html(data.ProjectedTicketCount);
            if (data.ProjectedStreamingCount == null) {
                $(".projectedStreaming").html("n/a");
            } else {
                $(".projectedStreaming").html(data.ProjectedStreamingCount);
            }
        }

        function MakeDecision(id, approved, $button) {
            var url = '<%: Url.Action("DecideExtraTicketPetition", "Petition") %>';

            $.post(url, { id: id, isApproved: approved, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
                if (data == "") {
                    $button.parents("tr").fadeOut(1500, function () { $(this).remove(); });
                }
            });
        }

        function SetScrollingBox($bar, $inline) {
            $(window).scroll(function () {
                var scrollPosition = $(document).scrollTop();
                var inlinePosition = $inline[0].offsetHeight + $inline[0].offsetTop;

                if (scrollPosition > inlinePosition) {
                    $bar.fadeIn(500);
                }
                else {
                    $bar.fadeOut(500);
                }
            });
        }
    </script>

    <style type="text/css">
        
        .decision
        {
            cursor: pointer;
        }
        
        .tickets
        {
            width: 50px;
        }
        
        .loading
        {
            display:none;
        }
        
        .cancel
        {
            float:right;
            display:none;
        }
        
        .check
        {
            float:right;
            display:none;
        }
        
        
        ul
        {
            list-style:none;
        }
        
        ul li
        {
            display:inline;
            margin-left:20px;
        }
        #Participations
        {
            
        }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
