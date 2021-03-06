<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.VisaLetter>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="NPOI.HSSF.Model" %>
<%@ Import Namespace="NPOI.SS.Formula.Functions" %>

<fieldset>
            <legend>Student's Information</legend>
        
            <ul class="registration_form">
                <li><strong>Student Id:</strong>
                    <%: Html.ActionLink<AdminController>(a => a.StudentDetails(Model.Student.Id, false), Model.Student.StudentId) %>
                </li>  
                <li><strong>Total Units:</strong>
                    <%: Html.Encode(Model.Student.TotalUnits) %>
                </li>
                <li><strong>Current Units:</strong>
                    <%: Html.Encode(Model.Student.CurrentUnits) %>
                </li>
                <li><strong>Earned Units:</strong>
                    <%: Html.Encode(Model.Student.EarnedUnits) %>
                </li>
                <li><strong>Email:</strong>
                    <%= Html.Encode(Model.Student.Email) %>
                </li>  
                
                <li><strong>Student's Name On Letter:</strong>
                    <%= Html.Encode(Model.StudentFirstName) %> <%= Html.Encode(Model.StudentLastName) %>
                </li>  
                <li><strong>Student's Gender:</strong>
                    <label><%: Html.RadioButton("Gender", 'M', new{ @disabled = "disabled" })%> Male</label>
                    <label><%: Html.RadioButton("Gender", 'F', new{ @disabled = "disabled" })%> Female</label>
                </li>  
                <li><strong>Ceremony:</strong>
                    <label><%: Html.RadioButton("Ceremony", 'S', new{ @disabled = "disabled" })%> Spring</label>
                    <label><%: Html.RadioButton("Ceremony", 'F', new{ @disabled = "disabled" })%> Fall</label>
                </li>                                                  
                <li><strong>College:</strong>
                    <%= Html.Encode(Model.CollegeName) %>
                </li>      
                <li><strong>Degree:</strong>
                    <%= Html.Encode(Model.Degree) %>
                </li>                             
                <li><strong>Major:</strong>
                    <%= Html.Encode(Model.MajorName) %>
                </li>                    
            </ul>
        </fieldset>

        <fieldset>
            <legend>Letter Request Details</legend>
        
            <ul class="registration_form">
                <li><strong>Date Submitted:</strong>
                    <%= Html.Encode(Model.DateCreated.ToString("g")) %>
                </li>
                <li><strong>Need Hard Copy:</strong>
                    <label><%: Html.RadioButton("HardCopy", 'Y', new{ @disabled = "disabled" })%> Yes</label>
                    <label><%: Html.RadioButton("HardCopy", 'N', new{ @disabled = "disabled" })%> No</label>
                    <%if(Model.HardCopy == "Y" && Model.IsApproved) {%>
                        <span><%: Html.ActionLink<AdminController>(a=>a.VisaLetterPreviewPdf(Model.Id), "Hard Copy", new{@class="button", target="_blank"}) %></span>
                    <%}%>
                </li>  
                <li><strong>Letter Id:</strong>
                    <%= Html.Encode(Model.ReferenceGuid) %>
                </li>
                <li><strong>Status Of Request:</strong>
                    <%if(Model.IsPending && !Model.IsCanceled) {%>
                        <%= Html.Encode("Pending") %>
                    <%} else if(Model.IsApproved) {%>
                        <%= Html.Encode("Approved") %>
                    <%} else if(Model.IsDenied) {%>
                        <%= Html.Encode("Denied") %>
                    <%} else if(Model.IsCanceled) {%>
                        <%= Html.Encode("Canceled") %>
                    <%}%>
                </li>
                <%if(Model.DateDecided.HasValue) {%>
                    <li><strong>Decision Date:</strong>
                        <%= Html.Encode(Model.DateDecided.Value.ToString("g")) %>
                    </li>
                <%}%>
                <li><strong>Relative's Relationship:</strong>
                    <%= Html.Encode(Model.RelationshipToStudent) %>
                </li>
                
                <li><strong>Relative Name:</strong>
                    <%= Html.Encode(Model.RelativeTitle) %> <%= Html.Encode(Model.RelativeFirstName) %> <%= Html.Encode(Model.RelativeLastName) %>

                </li>
                <li><strong>Relative Address:</strong>
                    <% var address = Html.Encode(Model.RelativeMailingAddress).Replace(System.Environment.NewLine, "<br /><strong></strong>");
                         %>
                   <%= address %>
                </li>
                
            </ul>
        </fieldset>