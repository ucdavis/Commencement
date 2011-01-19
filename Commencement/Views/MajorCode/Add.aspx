<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AddMajorCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Add / Activate Major</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>

    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
        <ul class="registration_form">
            <li><strong>Major:</strong>
                <%= this.Select("majorId").Options(Model.MajorCodes, x=>x.Id, x=>x.Name).FirstOption("--Select a Major--").Selected(!Model.NewMajor && Model.MajorCode != null ? Model.MajorCode.Id : string.Empty) %>    
            </li>
        </ul>

        <p>
        --Or--
        </p>

        <ul class="registration_form">
            <li><strong>Major Code:</strong>
                <%: Html.TextBox("MajorCode", Model.NewMajor ? Model.MajorCode.Id : string.Empty) %>
            </li>
            <li><strong>Major Name:</strong>
                <%: Html.TextBox("MajorName", Model.NewMajor ? Model.MajorCode.Name : string.Empty) %>
            </li>
        </ul>

        <p>
            <%: Html.SubmitButton("Add", "Add") %>
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

