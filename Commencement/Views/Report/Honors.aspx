<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HonorsPostModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
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
        
        <table>
            
            <tr>
                <td colspan="5">
                    <strong class="field-label">Term:</strong> <%: Html.TextBox("honorsPostModel.TermCode", Model != null ? Model.TermCode : string.Empty) %>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <strong class="field-label">College:</strong> <%= this.Select("honorsPostModel.College").Options(Model.Colleges, x => x.Id, x=> x.Name).FirstOption("--Select College--").Selected(Model != null && Model.College != null ? Model.College.Id : string.Empty) %>
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
                <td><%: Html.TextBox("honorsPostModel.Honors4590", Model != null && Model.Honors4590 > 0 ? Model.Honors4590.ToString() : string.Empty)%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors4590", Model != null ? Model.HighHonors4590.ToString() : string.Empty, new { @class = "hidable" })%></td>
                <td><%: Html.TextBox("honorsPostModel.HighestHonors4590", Model != null ? Model.HighestHonors4590.ToString() : string.Empty, new { @class = "hidable" })%></td>
            </tr>
            <tr>
                <th>90-135</th>
                <td><%: Html.TextBox("honorsPostModel.Honors90135", Model != null && Model.Honors90135 > 0 ? Model.Honors90135.ToString() : string.Empty)%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors90135", Model != null ? Model.HighHonors90135.ToString() : string.Empty, new { @class = "hidable" })%></td>
                <td><%: Html.TextBox("honorsPostModel.HighestHonors90135", Model != null ? Model.HighestHonors90135.ToString() : string.Empty, new { @class = "hidable" })%></td>
            </tr>
            <tr>
                <th>135+</th>
                <td><%: Html.TextBox("honorsPostModel.Honors135", Model != null && Model.Honors135 > 0 ? Model.Honors135.ToString() : string.Empty)%></td>
                <td><%: Html.TextBox("honorsPostModel.HighHonors135", Model != null ? Model.HighHonors135.ToString() : string.Empty, new { @class = "hidable" })%></td>
                <td><%: Html.TextBox("honorsPostModel.HighestHonors135", Model != null ? Model.HighestHonors135.ToString() : string.Empty, new { @class = "hidable" })%></td>
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
    
    <hr style="margin: 1em 0; border: 1px dotted lightgray;"/>

    <% Html.Grid(Model.HonorsReports.OrderByDescending(a => a.DateRequested))
           .Name("HonorsReports")
           .Columns(col =>
                        {
                            col.Add(a => { %>
                            
                                <% if (a.Contents != null) { %>
                                <%: Html.ActionLink("Download", "DownloadHonors", "Report", new {id=a.Id}, new {})%>
                                <% } %>

                            <% });
                            col.Bound(x => x.DateRequested);
                            col.Bound(x => x.TermCode);
                            col.Bound(x => x.College.Id).Title("College");
                        })
            .Render(); %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <style type="text/css">
        table { border-spacing: 0;}
        table th, table td { padding: 1em;}
        th { background-color: #0D548A;color: white;}
        input[type="text"] {min-width: 100px;  width: 100px;}
        
        .field-label { display: inline-block;width: 75px;}
    </style>
    
    <script type="text/javascript">
        $(function () {

            $("#honorsPostModel_College").change(function () {
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
