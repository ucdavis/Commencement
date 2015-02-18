<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.VisaLetter>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Cancel Visa Letter Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Cancel Visa Letter Request</h2>
    
    <% using (Html.BeginForm()) { %>

        <%: Html.AntiForgeryToken() %>

        <% Html.RenderPartial("VisaLetterDetailsPartial"); %>
    
        <fieldset>
            <legend>Confirm Action</legend>
        
            <h2>Are you sure you want to cancel your Visa Letter Request?</h2>

            <ul style="list-style-type: none;">
                <li><label><%: Html.RadioButton("Cancel", true)%> Yes</label></li>
                <li><label><%: Html.RadioButton("Cancel", false)%> No</label></li>
            </ul>
        
            <ul class="registration_form" style="margin-top: 1em;">
                <li><strong>&nbsp;</strong>
                    <input type="submit" value="Confirm" class="button" />            
                </li>
            </ul>

        </fieldset>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
