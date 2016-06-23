<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TransferRequestViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Transfer Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create Transfer Request</h2>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>

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
                <%: Html.ValidationMessageFor(model => model.TransferRequest.Ceremony) %>
                <img id="major-loader" src="<%: Url.Content("~/Images/ajax-loader.gif") %>" alt="Ajax Loader" style="display:none;"/>
            </li>
            <li><strong>Major: <span>*</span></strong>
                <%= this.Select("TransferRequest.MajorCode").FirstOption("--Select a Major--").Disabled(true) %>
                <%: Html.ValidationMessageFor(model => model.TransferRequest.MajorCode) %>
                <a href="#" id="more-majors" class="button hastip" title="View Major List" style="top:10px;"><span class="ui-icon ui-icon-grip-solid-horizontal"></span></a>
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

        var origId = '<%: Model.RegistrationParticipation.Major.Id %>';
        var origName = '<%: Model.RegistrationParticipation.Major.MajorName %>';

        $(function () {
            $('#TransferRequest_Ceremony').change(function () {

                var id = $(this).val();
                var $dd = $('#TransferRequest_MajorCode');

                $dd.children().remove();

                if (id == '') {

                    $dd.attr('disabled', 'disabled');

                } else {

                    var $loader = $('#major-loader').show();

                    $.getJSON(url, { ceremonyId: id }, function (results) {

                        $dd.removeAttr('disabled');

                        $dd.append($('<option>').attr('value', origId).html(origName + ' (' + origId + ')'));

                        $.each(results, function (index, item) {
                            $dd.append($('<option>').attr('value', item.Id).html(item.Name + ' (' + item.Id + ')'));
                        });

                        $loader.hide();

                    });
                }
            });

            $('#more-majors').click(function (event) {

                var ceremony = $('#TransferRequest_Ceremony').val();
                var url = '<%:Url.Action("MajorList") %>';

                if (ceremony != '') {
                    window.open(url + '\\' + ceremony);
                } else {
                    alert('Please select a ceremony.');
                }

                event.preventDefault();
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
