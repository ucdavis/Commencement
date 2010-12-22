<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input:radio").click(function () {
                $(this).siblings("input:radio").attr('checked', false);
            });
        });
    </script>

    <h2>Student Information</h2>
    <% Html.RenderPartial("StudentInformationPartial", Model.Student); %>
    
    <h2>Contact Information</h2>    
    <% Html.RenderPartial("ContactEditPartial"); %>        

    <h2>Ceremony Information</h2>
    <% Html.RenderPartial("CeremonyEditPartial"); %>

    <h2>Special Needs</h2>
    <% Html.RenderPartial("SpecialNeedsPartial"); %>