<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IList<Commencement.Core.Domain.Student>>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Students who would be emailed
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page_bar">
        <div class="col1"><h2>Students who would be emailed</h2></div>
        <div class="col2">
            <%: Html.ActionLink("Back to List", "Index", "EmailQueue", new {@class="button"}) %>
        </div>
    </div>
    

        <table >
            <thead>
                <tr>
                    <th >Pidm</th>
                    <th></th>
                    <th >Email</th>
                    <th >Name</th>
                    
                    <th>Earned Units</th>
                    <th>Current Units</th>
                    <th >Total Units</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var student in Model) { %>
                    <tr>
                        <td><%: student.Pidm %></td>
                        <td></td>
                        <td><%: student.Email %></td>
                        <td><%: student.FullName %></td>
                        
                        <td><%: student.EarnedUnits %></td>
                        <td><%: student.CurrentUnits %></td>
                        <td><%: student.TotalUnits %></td>
                    </tr>            
                <% } %>
            </tbody>
        </table>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
