<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Ceremony>" %>
<%@ Import Namespace="Commencement.MVC.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Edit Ceremony Permissions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
    <%= Html.ActionLink<CeremonyController>(a => a.Edit(Model.Id) , "Back to Ceremony") %>
    </li>
    </ul>

    <div class="page_bar">
        <div class="col1"><h2>Edit Permissions for <%: Model.CeremonyName %></h2></div>
        <div class="col2"><%= Html.ActionLink<CeremonyController>(a => a.AddEditor(Model.Id), "Add Editor", new { @class="button" })%></div>
    </div>

    

    <% Html.Grid(Model.Editors)
		.Transactional()
		.PrefixUrlParameters(false)
		.Name("Editors")
		.CellAction(cell =>
		                {
                            if (cell.Column.Member == "Owner")
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
