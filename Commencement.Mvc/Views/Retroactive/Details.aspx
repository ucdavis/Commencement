<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.RegistrationParticipation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Retroactive Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Retroactive Details</h2>
    
    <% using (Html.BeginForm()) { %>
    
        <%: Html.AntiForgeryToken() %>

        <ul class="registration_form">
            <li><strong>Student Id</strong>
                <%: Model.Registration.Student.StudentId %>
            </li>
            <li><strong>First Name</strong>
                <%: Model.Registration.Student.FirstName %>
            </li>
            <li><strong>Last Name</strong>
                <%: Model.Registration.Student.LastName %>
            </li>
            <li><strong>Major</strong>
                <%: Model.Major.MajorName %>
            </li>
            <li><strong>Ceremony</strong>
                <%: Model.Ceremony.CeremonyName %>
            </li>
            <li><strong>Date Registered</strong>
                <%: Model.DateRegistered %>
            </li>
            <li><strong>&nbsp;</strong>
                <input type="submit" value="Cancel Registration" class="button"/>
                <%: Html.ActionLink("Cancel", "Index") %>
            </li>
        </ul>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
