<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RegisterConfirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Petition Submitted Successfully</h2>

    <p>
        Thank you for submitting your petition. We will do our best to review your request within one week. 
        You will be notified by email of the decision. 
    </p>
    <p>
        Petitions submitted after the registration deadline will be reviewed on first come first serve basis. 
    </p>
    
    <h3>Please review our FAQ page for any further questions: 
        <a href="http://caes.ucdavis.edu/NewsEvents/Events/Commencement/frequently-asked-questions">http://caes.ucdavis.edu/NewsEvents/Events/Commencement/frequently-asked-questions</a>
    </h3>
    
    <p>
        Best,<br />
        commencement@caes.ucdavis.edu
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
