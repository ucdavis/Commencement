<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>

<%= this.CheckBoxList("SpecialNeeds").Options(Model.SpecialNeeds).ItemClass("SpecialNeeds")%>