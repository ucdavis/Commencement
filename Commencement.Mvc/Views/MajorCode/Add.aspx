<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.ViewModels.AddMajorCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink("Back to Administration", "Index") %></li>
    </ul>

    <h2>Add / Activate Major</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>

    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
        <ul class="registration_form">
            <li><strong>Major:</strong>
                <%= this.Select("majorId").Options(Model.MajorCodes, x=>x.Id, x=>x.Name).FirstOption("--Select a Major--").Selected(!Model.NewMajor && Model.MajorCode != null ? Model.MajorCode.Id : string.Empty) %>    
            </li>
        </ul>

        <div class="ui-corner-all" style="border: 1px solid lightgray; text-align: center; padding: 1em; margin: 1em;">
        --Or--
        </div>

        <ul class="registration_form">
            <li><strong>Major Code:</strong>
                <%: Html.TextBox("MajorCode", Model.NewMajor ? Model.MajorCode.Id : string.Empty) %>   
                <%: Html.ValidationMessageFor(a=>a.MajorCode.Id) %>             
            </li>
            <li><strong>Major Name:</strong>
                <%: Html.TextBox("MajorName", Model.NewMajor ? Model.MajorCode.Name : string.Empty) %>
                <%: Html.ValidationMessageFor(a=>a.MajorCode.Name) %>
            </li>
        </ul>

        <ul class="registration_form" style="margin-top: 1.5em;">
            <li><strong>&nbsp;</strong>
                <input type="submit" class="button" value="Add Major" />
                |
                <%: Html.ActionLink("Cancel", "Index") %>
            </li>
        </ul>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

