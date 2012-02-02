<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UC Davis Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

    <div class="left_col">
    <h2>Commencement Participation</h2>

    <p>
        Registration for the SPRING 2012 College of Agricultural & Environmental Sciences (CA&ES) Commencement Ceremony is from Wednesday, February 1, 2012 and <strong>ends on Friday, April 13th.</strong>
    </p>

    <p>
        In order to participate in the Spring 2012 CA&ES Commencement Ceremony, you will need to meet the following criteria:
    </p>

    <ul>
        <li>You must complete 160 units by the end of the Winter 2012 quarter</li>
        <li>Your major must be administered by the College of Agricultural & Environmental Sciences. (<a href="http://caes.ucdavis.edu/NewsEvents/Events/Commencement/major-assignment-by-ceremony">List of majors</a>)</li>
        <li>You must have a UCD LoginID and Kerberos password. (More information: <a href="https://accounts.ucdavis.edu/faq.html">Here</a> )</li>
        <li><strong>Note:</strong> This registration is only to participate the CA&ES Commencement Ceremony and receive tickets for your guests. To officially graduate from the University, you must file an application with the <a href="http://registrar.ucdavis.edu/graduation">Registrar's Office</a>. THESE ARE SEPARATE STEPS WITH DIFFERENT DEADLINES AND REQUIRE SEPARATE ACTIONS ON YOUR PART. (<i>walking in the ceremony does not equate to degree completion</i>). 
        </li>
    </ul>
    <p>
        **Upon completion of the registration process, you will receive a registration confirmation e-mail. If you do not receive this e-mail within 24 hours, please re-register. For questions or additional information, please contact the <a href="mailto:commencement@caes.ucdavis.edu">CA&ES commencement coordinator</a>.
    </p>

    <p>
        <strong>
        The University policy prohibits the selling of commencement tickets for money or commercial value.  Violation will result in referral to Student Judicial Affairs.
        </strong>
    </p>


    <% if (Model.Ceremonies.Min(a => a.RegistrationBegin) <= DateTime.Now && Model.Ceremonies.Max(a=>a.RegistrationDeadline >= DateTime.Now) ) { %>
        <span class="reg_btn"><%: Html.ActionLink<StudentController>(a => a.Index(), "Register")%></span>
    <% } %>
</div>

<div class="right_col">
<h3>Dates</h3>
<ul>
<li><strong>Monday, April 16, 2012</strong> <em>Applications deadline for <a href="http://caes.ucdavis.edu/NewsEvents/Events/Commencement/student-speaker">student Commencement speaker</a></em></li>
<li><strong>Tentative: Monday, 14th and Tuesday, May 15th (9am-6pm)</strong> <em>Ticket pick-up at the ARC Pavilion ticket office</em></li>
<li><strong>Sunday, June 17, 2012 (9:00 am and 2:00 pm)</strong> <em>Commencement Ceremony at UC Davis ARC Pavilion </em></li>
</ul>
</div>

<div id="col_ft">
<p><i>After April 13th we will accept late registration until June 8th but due to printing deadlines we cannot guarantee your name will appear in the program or that you will receive the maximum number of tickets allotted per person </i></p>
</div>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<style type="text/css">
    #footer {display:none; height:0; margin-top:0;}
    .main {padding-bottom:0;}
</style>
</asp:Content>
