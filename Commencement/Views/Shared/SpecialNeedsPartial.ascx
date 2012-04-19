<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>

<%--<%= this.CheckBoxList("SpecialNeeds").Options(Model.SpecialNeeds).ItemClass("SpecialNeeds").Class("radio_list")%>--%>

<ul class="registration_form">
    <li><strong>&nbsp;</strong>
        <%= this.Select("SpecialNeeds").Options(Model.SpecialNeeds).FirstOption("--Special Needs Request--") %>    
    </li>
</ul>
