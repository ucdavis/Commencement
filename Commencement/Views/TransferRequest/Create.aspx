<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TransferRequestViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Transfer Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create Transfer Request</h2>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.TransferRequest>("TransferRequest")%>

    <% using(Html.BeginForm()) { %>
    
        <%: Html.AntiForgeryToken() %>

        <ul class="registration_form">
            <li class="prefilled">
                <strong>Student Id</strong>
                <%: Model.RegistrationParticipation.Registration.Student.StudentId %>
            </li>
            <li class="prefilled">
                <strong>Name</strong>
                <%: Model.RegistrationParticipation.Registration.Student.FullName %>
            </li>
            <li class="prefilled">
                <strong>Registered Major</strong>
                <%: Model.RegistrationParticipation.Major.MajorName %>
            </li>
            <li class="prefilled">
                <strong>Registered Ceremony</strong>
                <%: string.Format("{0} ({1})", Model.RegistrationParticipation.Ceremony.CeremonyName, Model.RegistrationParticipation.Ceremony.DateTime) %>
            </li>
        </ul>

        <ul class="registration_form">
            <li>
                <strong><%: Html.LabelFor(model => model.TransferRequest.Ceremony) %><span>*</span></strong>
                <%= this.Select("TransferRequest.Ceremony").Options(Model.Ceremonies,x=>x.Id,x=>x.CeremonyName).Selected(Model.TransferRequest.Ceremony != null ? Model.TransferRequest.Ceremony.Id.ToString() : null).FirstOption("--Select a Ceremony--") %>
            </li>
            <li><strong>Major: <span>*</span></strong>
                <select id="TransferRequest_MajorCode" name="TransferRequest.MajorCode" disabled="disabled"></select>
            </li>
            <li><strong><%: Html.LabelFor(model => model.TransferRequest.Reason) %><span>*</span></strong>
                <%: Html.TextAreaFor(model => model.TransferRequest.Reason) %>
                <%: Html.ValidationMessageFor(model => model.TransferRequest.Reason) %>
            </li>
                     
            <li><strong>&nbsp;</strong>
                <input type="submit" value="Create" class="button" />
                |
                <%: Html.ActionLink("Cancel", "StudentDetails", "Admin", new {id= Model.RegistrationParticipation.Registration.Student.Id}, new {}) %>
            </li>

        </ul>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">

        var url = '<%: Url.Action("GetMajorsByCeremony") %>';

        $(function () {
            $('#TransferRequest_Ceremony').change(function () {

                var id = $(this).val();

                if (id == '') {

                    $('#TransferRequest_Major').children().remove();
                    $('#TransferRequest_Major').attr('disabled', 'disabled');

                } else {
                    $.getJSON(url, { ceremonyId: id }, function (results) {

                        $.each(results, function (index, item) {
                            $('#TransferRequest_Major').append($('<option>').attr('value', item.Id).html(item.Name + ' (' + item.Id + ')'));
                        });

                        $('#TransferRequest_Major').removeAttr('disabled');

                    });
                }
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
