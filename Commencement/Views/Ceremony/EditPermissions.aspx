<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Ceremony>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Edit Ceremony Permissions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
    <%= Html.ActionLink<CeremonyController>(a => a.Edit(Model.Id) , "Back to Ceremony") %>
    </li>
    <li>
        <%= Html.ActionLink<CeremonyController>(a=>a.AddEditor(Model.Id), "Add Editor") %>
    </li>
    </ul>

    <h2>Edit Permissions for <%: Model.Name %></h2>

    <% Html.Grid(Model.Editors)
		.Transactional()
		.PrefixUrlParameters(false)
		.Name("Editors")
		.CellAction(cell =>
		                {
                            if (cell.Column.Name == "Owner")
                            {
                                cell.Text = cell.DataItem.Owner ? "Yes" : "No";
                            }
		                })
		.Columns(col =>
		             {
		                 col.Add(a => { 
                                using (Html.BeginForm("RemoveEditor", "Ceremony", FormMethod.Post)) { %>

                                    <%= Html.AntiForgeryToken() %>
                                    <%= Html.Hidden("id", Model.Id) %>
                                    <%= Html.Hidden("ceremonyEditorId", a.Id)%>
                                    <%= Html.SubmitButton("Submit", "Remove", new {@class="remove_button"}) %>

                                <% } %>
                                <% });
		                 col.Bound(a => a.User.FirstName);
		                 col.Bound(a => a.User.LastName);
		                 col.Bound(a => a.Owner);
		             })
		.Render();
		%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <style type="text/css">
        .remove_button
        {
            background:none;
            border: 0;
            cursor:pointer;
        }
        .remove_button:hover
        {
            color:#0D548A;
        }
    </style>

</asp:Content>
