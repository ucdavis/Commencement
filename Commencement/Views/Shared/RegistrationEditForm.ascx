<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input:radio").click(function () {
                $(this).siblings("input:radio").attr('checked', false);
            });
        });
    </script>
    
    <fieldset>
        
        <legend>Student Information</legend>
        
        <% Html.RenderPartial("StudentInformationPartial", Model.Student); %>

    </fieldset>
    
    <fieldset>
        
        <legend>Contact Information</legend>
        
        <% Html.RenderPartial("ContactEditPartial"); %>   

    </fieldset>
    
    <fieldset>
        
        <legend>Ceremony</legend>
        
        <% Html.RenderPartial("CeremonyEditPartial"); %>

    </fieldset>
    
    <fieldset>
        
        <legend>Special Needs</legend>
        
        <% Html.RenderPartial("SpecialNeedsPartial"); %>

    </fieldset>