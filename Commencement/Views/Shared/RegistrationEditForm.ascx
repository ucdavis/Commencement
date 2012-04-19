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
        
        <div class="disclaimer">
            Any special needs you may have.  Please do not provide special needs of your guests.
        </div>

        <% Html.RenderPartial("SpecialNeedsPartial"); %>

    </fieldset>