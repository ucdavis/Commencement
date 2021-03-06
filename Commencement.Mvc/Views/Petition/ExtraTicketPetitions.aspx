﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminExtraTicketPetitionViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Extra Ticket Petitions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<PetitionController>(a=>a.Index(), "Back to Petitions") %></li>
    </ul>

    <div class="page_bar">
        <div class="col1"><h2>Extra Ticket Petitions</h2></div>
        <div class="col2">
        
            <% using (Html.BeginForm("ExtraTicketPetitions", "Petition", FormMethod.Get)) { %>
                Ceremony At: <%= this.Select("ceremonyId").Options(Model.Ceremonies, x=>x.Id, x=>x.DateTime.ToString("g")).Selected(Model.Ceremony != null ? Model.Ceremony.Id : 0) %>
                View All : <%= Html.CheckBox("ViewAll", Model.ViewAll)%>
                <input type="submit" value="View" class="button" />
            <% } %>

            <% if (Model.Ceremony != null) { %>
                <% using (Html.BeginForm("ApproveAllExtraTicketPetition", "Petition", FormMethod.Post)) { %>
        
                    <%= Html.AntiForgeryToken() %>
                    <%: Html.Hidden("id", Model.Ceremony.Id) %>
                    <%: Html.SubmitButton("ApproveAll", "Approve All", new { @class="button", onclick="return confirm('Are you REALLY sure you want to approve all?')"})%>
                <% } %>
            <% } %>
        </div>
    </div>

    <% if (Model.Ceremony != null) { %>
        
        <div id="ticketCountInline" style="position: relative; top: -20px;">
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
    
        <% var count = 1; %>
        
        <% Html.Grid(Model.RegistrationParticipations)
               .Name("Participations")
               .Columns(col =>
                            {
                                col.Add(a=>{ %><%: count++ %><%}).Title("Row");
                                col.Add(a => { %>
                                    <% if (a.ExtraTicketPetition.IsPending) { %>
                                    <img src='<%: Url.Content("~/Images/CheckMark-1.png") %>' class="decision" data-approve="true" data-participationId='<%: a.Id %>' />
                                    <img src='<%: Url.Content("~/Images/Cancel-1.png") %>' class="decision" data-approve="false" data-participationId='<%: a.Id %>' />
                                    <% } %>
                                <% }).HtmlAttributes(new {Style = "width:50px;"});
                                col.Bound(a => a.Registration.Student.LastName);
                                col.Bound(a => a.Registration.Student.FirstName);
                                col.Bound(a => a.NumberTickets).Title("# Tickets");
                                col.Add(a=>{ %>

                                    <img src='<%: Url.Content("~/Images/speechbubble.jpg") %>' class="hastip" title="<%: !string.IsNullOrEmpty(a.ExtraTicketPetition.Reason) ? a.ExtraTicketPetition.Reason : "n/a" %>" />
                                    
                                <%}).Title("Reason");
                                col.Bound(a => a.ExtraTicketPetition.NumberTicketsRequested).Title("# Requested");
                                if (Model.Ceremony.HasStreamingTickets){col.Bound(a => a.ExtraTicketPetition.NumberTicketsRequestedStreaming).Title("# Streaming");}
                                col.Add(a => { %>
                                    <% if (a.ExtraTicketPetition.IsPending) { %>
                                    <%: Html.TextBox("TicketsApproved", a.ExtraTicketPetition.NumberTickets.HasValue ? a.ExtraTicketPetition.NumberTickets : a.ExtraTicketPetition.NumberTicketsRequested, new {@class="tickets", participationId=a.Id, ceremonyId=a.Ceremony.Id}) %>
                                    <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading" />
                                    <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="cancel" />
                                    <img src="<%: Url.Content("~/Images/CheckMark-1.png") %>" class="check" />
                                    <% } else { %>
                                        <%: a.ExtraTicketPetition.NumberTickets %>
                                    <% } %>
                                <% }).Title("# Approved");
                                if (Model.Ceremony.HasStreamingTickets)
                                {
                                    col.Add(a => {%>
                                        <% if (a.ExtraTicketPetition.IsPending) { %>
                                        <%: Html.TextBox("StreamingApproved", a.ExtraTicketPetition.NumberTicketsStreaming.HasValue ? a.ExtraTicketPetition.NumberTicketsStreaming.Value : a.ExtraTicketPetition.NumberTicketsRequestedStreaming, new { @class = "tickets streaming", participationId = a.Id, ceremonyId = a.Ceremony.Id })%>
                                        <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading" />
                                        <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="cancel" />
                                        <img src="<%: Url.Content("~/Images/CheckMark-1.png") %>" class="check" />
                                        <% } else { %>
                                            <%: a.ExtraTicketPetition.NumberTicketsRequestedStreaming %>
                                        <% } %>
                                    <%}).Title("# Streaming Approved");
                                }
                                col.Bound(a => a.ExtraTicketPetition.DateSubmitted).Title("Submitted");
                            })
               .DataBinding(data => data.Server().Select("ExtraTicketPetitions", "Petition", new {viewAll = Model.ViewAll, ceremonyId = Model.Ceremony.Id}))
               .Sortable()
               .Render(); %>
    <% } %>

    <%= Html.AntiForgeryToken() %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<%--    <script type="text/javascript" src='<%: Url.Content("~/Scripts/jquery.bt.min.js") %>'></script>
    <link href="<%= Url.Content("~/Content/jquery.bt.css") %>" rel="Stylesheet" type="text/css" media="screen" />--%>

    <script type="text/javascript">

        $(document).ready(function () {
            $(".tickets").blur(function () { SaveTicketAmount($(this).attr("participationId"), $(this).attr("ceremonyId"), $(this).val(), $(this).hasClass("streaming"), this); });
            //$(".decision").click(function () { MakeDecision($(this).attr("participationId"), $(this).val() == "Approve", this); });

            $(".decision").click(function () { MakeDecision($(this).data("participationId"), $(this).data("approve"), $(this)); });

            SetScrollingBox($("#ticketCountBar"), $("#ticketCountInline"));

            //$(".reason").each(function (index, item) { $(item).bt($(item).siblings("div.reasonText").html()); });
        });

        function SaveTicketAmount(id, ceremonyId, amount, streaming, box) {
            // validate the amount is a number

            $(box).siblings(".loading").show();

            var url = '<%: Url.Action("UpdateTicketAmount", "Petition") %>';
            $.post(url, { id: id, tickets: amount, ceremonyId: ceremonyId, streaming: streaming, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
                // something with the messages
                $(box).siblings(".loading").fadeOut(2000);

                if (data.Message != null && data.Message != "") {
                    $(box).siblings(".cancel").show();
                    alert(data.Message);
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

                var viewAll = $("#ViewAll").is(':checked');

                if (data == "") {
                    if (viewAll) {

                        $button.parents("td").find("img").fadeOut(1500, function () { $(this).remove(); });

                        var $ticketsApproved = $button.parents("tr").find("#TicketsApproved");
                        $ticketsApproved.parents("td").html($ticketsApproved.val());

                        var $streamingApproved = $button.parents("tr").find("#StreamingApproved");
                        $streamingApproved.parents("td").html($streamingApproved.val());

                    }
                    else {
                        // delete the row, becuase we are showing only pending
                        $button.parents("tr").fadeOut(1500, function () { $(this).remove(); });
                    }
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
        
        .page_bar .col1 {width: 30%;}
        .page_bar .col2 {width: 70%;}
        .page_bar form {display: inline-block; margin-left: 10px;}
    </style>

</asp:Content>
