<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Mvc.Controllers.ViewModels.RegistrationModel>" %>

<ul class="registration_form">
    <li><strong>&nbsp;</strong>
        <%= this.Select("SpecialNeeds").Options(Model.SpecialNeeds).FirstOption("--Special Needs Request--") %>    
    </li>
</ul>

<div id="sn-disclaimers" style="margin: 1em; color: red;">
    <% foreach (var sn in Model.SpecialNeeds)
       { var tip = Model.FullSpecialNeeds.FirstOrDefault(a => a.Id.ToString() == sn.Value);
          
           if (tip != null)
           {%>
    <div id="tip-<%: sn.Value %>" style="display: <%: sn.Selected ? "block" : "none" %>">
        <p><%: tip.Tip %></p>
    </div>
    <% }
       } %>
</div>

<script type="text/javascript">
    $(function () {
        $("#SpecialNeeds").change(function () {
            // hide all
            $("#sn-disclaimers div").hide();

            var id = $(this).val();
            $("#sn-disclaimers div#tip-" + id).show();
        });
    });
</script>