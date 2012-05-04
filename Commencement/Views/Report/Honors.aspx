<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Core.Domain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Honors
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <ul class="btn">
    <li><%= Html.ActionLink<ReportController>(a=>a.Index(), "Back to Report Home")  %></li>
    </ul>
    
    <h2>Honors Report</h2>

    <% using (Html.BeginForm()) { %>
    
        <%: Html.AntiForgeryToken() %>
        
        <% var postModel = (HonorsPostModel)ViewData["HonorsPostModel"]; %>

        <table>
            
            <tr>
                <td colspan="5">
                    College: <%= this.Select("honorsPostModel.College").Options((List<College>)ViewData["Colleges"], x => x.Id, x=> x.Name).FirstOption("--Select College--") %>
                </td>
            </tr>
        
            <tr>
                <th>Units</th>
                <th>Honors</th>
                <th>High Honors</th>
                <th>Highest Honors</th>
            </tr>
            <tr>
                <th>45-90</th>
                <td><%: Html.TextBox("honorsPostModel.Honors4590")%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors4590", string.Empty, new { @class = "hidable" })%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors.4590", string.Empty, new { @class = "hidable" })%></td>
            </tr>
            <tr>
                <th>90-135</th>
                <td><%: Html.TextBox("honorsPostModel.Honors90135")%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors90135", string.Empty, new { @class = "hidable" })%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors.90135", string.Empty, new { @class = "hidable" })%></td>
            </tr>
            <tr>
                <th>135+</th>
                <td><%: Html.TextBox("honorsPostModel.Honors135")%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors135", string.Empty, new { @class = "hidable" })%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors.135", string.Empty, new { @class = "hidable" })%></td>
            </tr>

        </table>
    
        <ul class="registration_form">
            <li>
                <strong>&nbsp;</strong>
                <%: Html.SubmitButton("Submit", "Submit Request", new {@class="button"}) %>
                |
                <%= Html.ActionLink<ReportController>(a=>a.Index(), "Cancel")  %>
            </li>
        
        </ul>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <style type="text/css">
        table { border-spacing: 0;}
        table th, table td { padding: 1em;}
        th { background-color: #0D548A;color: white;}
        input[type="text"] {min-width: 100px;  width: 100px;}
    </style>
    
    <script type="text/javascript">
        $(function () {

            $("#College").change(function () {

                debugger;   

                // LS is special is how honors are calculated
                if ($(this).val() == "LS") {
                    $(".hidable").attr('disabled', true);
                } else {
                    $(".hidable").attr('disabled', false);
                }
            });

        });
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
