<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Student>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddStudentConfirm
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>AddStudentConfirm</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            Pidm:
            <%= Html.Encode(Model.Pidm) %>
        </p>
        <p>
            StudentId:
            <%= Html.Encode(Model.StudentId) %>
        </p>
        <p>
            FirstName:
            <%= Html.Encode(Model.FirstName) %>
        </p>
        <p>
            LastName:
            <%= Html.Encode(Model.LastName) %>
        </p>
        <p>
            Units:
            <%= Html.Encode(String.Format("{0:F}", Model.Units)) %>
        </p>
        <p>
            Email:
            <%= Html.Encode(Model.Email) %>
        </p>
        <p>
            Login:
            <%= Html.Encode(Model.Login) %>
        </p>
        <p>
            DateAdded:
            <%= Html.Encode(String.Format("{0:g}", Model.DateAdded)) %>
        </p>
        <p>
            DateUpdated:
            <%= Html.Encode(String.Format("{0:g}", Model.DateUpdated)) %>
        </p>
        <p>
            FullName:
            <%= Html.Encode(Model.FullName) %>
        </p>
        <p>
            StrMajors:
            <%= Html.Encode(Model.StrMajors) %>
        </p>
        <p>
            StrMajorCodes:
            <%= Html.Encode(Model.StrMajorCodes) %>
        </p>
        <p>
            Id:
            <%= Html.Encode(Model.Id) %>
        </p>
    </fieldset>
    <p>
        <%=Html.ActionLink("Edit", "Edit", new { /* id=Model.PrimaryKey */ }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

