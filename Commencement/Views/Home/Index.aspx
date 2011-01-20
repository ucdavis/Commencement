<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content">

    <div class="left_col">
    <h2>Commencement Participation</h2>

    <p>
    Registration for the <%: Model.Name %> College of Agricultural & Environmental Science's (CA&ES) 
    Commencement Ceremony will begin Tuesday February 1, 2011 and <strong>ends on Monday, April 11th</strong><sup>1</sup>.
    </p>

    <p>
        In order to participate in the <%: Model.Name %> CA&ES Commencement Ceremony, you will need to meet the following criteria:
    </p>

    <ul>
        <li>You must complete 160 units by the end of Winter 2011 quarter</li>
        <li>Your major must be administered by the College of Agricultural & Environmental Science. (<a href="http://caes.ucdavis.edu/NewsEvents/Events/Commencement/major-assignment-by-ceremony">List of majors</a>)</li>
        <li>You must have a UCD LoginID and Kerberos password. (More information: <a href="https://accounts.ucdavis.edu/faq.html">Here</a> )</li>
        <li><strong>Note:</strong> This registration is only to participate the CA&ES Commencement Ceremony and receive tickets for your guests.<sup>2</sup> 
            To officially graduate from the University, you must file an application with the <a href="http://registrar.ucdavis.edu/graduation">Registrar's Office</a>. 
            THESE ARE SEPARATE STEPS WITH DIFFERENT DEADLINES AND REQUIRE SEPARATE ACTIONS ON YOUR PART. <i>walking in the ceremony does not equate to degree completion).</i>
        </li>
    </ul>

    <p>
        Remember to order your cap and gowns: Commencement Caps and Gowns can be rented at the UC Davis Bookstore (<a href="http://bookstore.ucdavis.edu/graduation/">Here</a>). In addition, the UC Davis Bookstore sells graduation related merchandise such as class rings, announcements, frames, and plaques.
    </p>
    <p>
        <strong>April 15, 2011</strong>: Applications deadline for student Commencement speaker
    </p>
    <p>
        <strong>mid-May</strong>: Letters of instruction and Commencement admission tickets available for pick-up
    </p>
    <p>
        <strong>Saturday, June 11, 2011: Commencement Ceremony: 9:00 AM and 2:00 PM, UC Davis ARC Pavilion</strong>
    </p>
    <p>
        <a href="http://www.gradimages.com/register.cfm">GradTrak</a> will be photographing all graduates at the upcoming graduation ceremony. Receive a $5 discount on photo orders over $25 by registering prior to the ceremony.
    </p>
    <p>
        <strong>
        The University policy prohibits the selling of commencement tickets for money or commercial value.  Violation will result in referral to Student Judicial Affairs.
        </strong>
    </p>



    <span class="reg_btn"><%: Html.ActionLink<StudentController>(a => a.Index(), "Register")%></span>
</div>

<div class="right_col">
<h3>Dates</h3>
<ul>
<li><strong>Tuesaday, February 1, 2011</strong> <em>Registration opens</em></li>
<li><strong>Monday, April 11th</strong> <em>Registration ends</em></li>
<li><strong>Friday, April 15, 2011</strong> <em>Applications deadline for student Commencement speaker </em></li>
<li><strong>mid-May</strong> <em>Letters of instruction and Commencement admission tickets available for pick-up </em></li>
<li><strong>Saturday, June 11, 2011</strong> <em>Commencement Ceremony: 9:00 AM and 2:00 PM, UC Davis ARC Pavilion</em></li>
</ul>
</div>

<div id="col_ft">
<p><sup>1</sup>We will accept late registration until June 3rd but due to printing deadlines we cannot guarantee your name will appear in the program or that you will receive the maximum number of tickets alloted per person. </p>
<p><sup>2</sup>Upon completion of the registration process, you will receive a registration confirmation e-mail. If you do not receive this e-mail within 24 hours, please re-register. For questions or additional information, please contact the CA&ES commencement coordinator. </p>
</div>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
