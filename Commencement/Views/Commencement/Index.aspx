<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CommencementViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <%= Html.ActionLink<CommencementController>(a=>a.Create(), "Create New") %>

    <% Html.Grid(Model.Ceremonies)
           .Transactional()
           .Name("Ceremonies")
           .Columns(col =>
                        {
                            col.Command(commands => commands.Select());
                            col.Bound(a => a.DateTime);
                            col.Bound(a => a.Location);
                            col.Bound(a => a.TotalTickets);
                            col.Bound(a => a.RegistrationDeadline);
                        } )
           .Render();
        
           %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
