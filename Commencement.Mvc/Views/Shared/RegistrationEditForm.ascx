<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Mvc.Controllers.ViewModels.RegistrationModel>" %>

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
            Please indicate any special needs you (personally) have that will affect your ability to participate in the ceremony (lining up, sitting, crossing the stage, etc.). For guest accommodations, please refer back to the <a href="http://commencement.ucdavis.edu" target="ucd_commencement">UC Davis Commencement site</a>.
        </div>

        <% Html.RenderPartial("SpecialNeedsPartial"); %>

    </fieldset>