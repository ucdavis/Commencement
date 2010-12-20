<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<HomeController>(a=>a.Index(), "Back Home") %></li>
    </ul>

    <ul>
        <li><a href="<%: Url.Action("ExtraTicketPetitions", "Petition") %>">Extra Ticket Petitions</a></li>
        <li><a href="<%: Url.Action("RegistrationPetitions", "Petition") %>">Registration Petitions</a></li>
    </ul>


<%--    <%= Html.AntiForgeryToken() %>



    <div id="Ceremony_Container" style="margin:0 auto; width:500px;">
    
        <ul style="list-style:none;">
            <% foreach(var ceremony in Model.Ceremonies) { %>
                <li>
                    <strong><%= ceremony.Name %> (<%= ceremony.DateTime %>) :</strong>
                    <%= ceremony.AvailableTickets %>
                </li>
            <% } %>
        </ul>
    
    </div>

    <div id="tabs">

        <ul>
            <li><a href="#tabs-1">Extra Ticket Petition</a></li>
            <li><a href="#tabs-2">Registration Petition</a></li>
        </ul>

        <div id="tabs-1">
            <%
                Html.Grid(Model.PendingExtraTicket)
                    .Name("PendingExtraTicketPetitions")
                    .Columns(col=>
                                 {
                                     col.Add(a =>
                                                 { %>
                                                 
                                                    <%= Html.ActionLink<PetitionController>(b=>b.DecideExtraTicketPetition(a.Id, true), "Approve") %>
                                                    |
                                                    <%= Html.ActionLink<PetitionController>(b=>b.DecideExtraTicketPetition(a.Id, false), "Deny") %>
                                                 
                                                 <% });
                                     col.Bound(a => a.Student.StudentId);
                                     col.Bound(a => a.Student.FullName).Title("Name");
                                     //col.Bound(a => a.ExtraTicketPetition.NumberTickets).Title("# Extra Tickets");
                                     col.Add(a =>
                                                 {%>
                                                    <div class="ticket_container">
                                                        <%= Html.Hidden("id", a.Id) %>
                                                        <span class="ticket_amount"><%= a.ExtraTicketPetition.NumberTickets %></span>
                                                        <span class="ticket_box" style="display:none;"><input type="text" size="1" value="<%= a.ExtraTicketPetition.NumberTickets %>" /></span>
                                                        <span class="ticket_edit_btn">
                                                            <a href="javascript:;" class="edit_btn">[Edit]</a>
                                                        </span>
                                                        <span class="ticket_edit_btns" style="display:none;">
                                                            <a href="javascript:;" class="save_btn">[Save]</a>
                                                            <a href="javascript:;" class="cancel_btn">[Cancel]</a>
                                                        </span>
                                                    </div>
                                                 <%}).Title("# Extra Tickets");
                                     col.Bound(a => a.TotalTickets).Title("# Tickets");
                                     col.Bound(a => a.ExtraTicketPetition.DateSubmitted);
                                 })
                    .Render();
            %>
        </div>
        
        <div id="tabs-2">
            <%  Html.Grid(Model.PendingRegistrationPetitions)
                    .Name("PendingRegistrationPetitions")
                    .Columns(col=>
                                 {
                                     col.Add(a =>
                                                 {
                                                     {%>
                                                     
                                                     <%= Html.ActionLink<PetitionController>(b=>b.DecideRegistrationPetition(a.Id, true), "Approve") %>
                                                     |
                                                     <%= Html.ActionLink<PetitionController>(b=>b.DecideRegistrationPetition(a.Id, false), "Deny") %>
                                                     
                                                     <%}
                                                 });
                                     col.Bound(a => a.StudentId);
                                     col.Bound(a => a.FullName);
                                     col.Bound(a => a.DateSubmitted);
                                     col.Add(a =>
                                                 {%>
                                                 <%= Html.ActionLink<PetitionController>(b=>b.RegistrationPetition(a.Id), "Details") %>
                                                 <%});
                                 }
                    )
                    .Render();
                 %>
        </div>
    </div>
--%>    
    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<%--    <script type="text/javascript">
        var antiForgeryToken;

        $(function() {
            antiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            $("#tabs").tabs();

            $(".edit_btn").click(function() {
                var $container = $(this).parents("div.ticket_container");

                $container.children(".ticket_amount").hide();
                $container.children(".ticket_edit_btn").hide();

                $container.children(".ticket_box").show();
                $container.children(".ticket_edit_btns").show();
            });
            $(".save_btn").click(function() {
                var $container = $(this).parents("div.ticket_container");

                var id = $container.children("input#id").val();
                var tickets = $container.children(".ticket_box").children("input").val();

                // make the save
                var url = '<%= Url.Action("UpdateTicketAmount", "Petition") %>' + "/" + id;
                $.post(url, { id: id, tickets: tickets, __RequestVerificationToken: antiForgeryToken }, function(result) {
                    if (result) {
                        $container.children(".ticket_amount").html(tickets);

                        ChangeTicketToView($container);
                    }
                    else { alert("There was an error saving, please try again."); }
                });


            });
            $(".cancel_btn").click(function() {
                var $container = $(this).parents("div.ticket_container");

                // revert values back to original
                $container.children(".ticket_box").children("input").val($container.children(".ticket_amount").html());

                ChangeTicketToView($container);
            });
        });

        function ChangeTicketToView($container) {
            $container.children(".ticket_amount").show();
            $container.children(".ticket_edit_btn").show();

            $container.children(".ticket_box").hide();
            $container.children(".ticket_edit_btns").hide();
        }
    </script>--%>

</asp:Content>
