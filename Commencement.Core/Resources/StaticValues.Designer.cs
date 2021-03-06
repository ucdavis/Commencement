﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Commencement.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class StaticValues {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StaticValues() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Commencement.Core.Resources.StaticValues", typeof(StaticValues).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Student record could not be found..
        /// </summary>
        public static string Error_StudentNotFound {
            get {
                return ResourceManager.GetString("Error_StudentNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was a problem sending {0} an email..
        /// </summary>
        public static string Student_Add_Permission_Problem {
            get {
                return ResourceManager.GetString("Student_Add_Permission_Problem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must agree to the disclaimer.
        /// </summary>
        public static string Student_agree_to_disclaimer {
            get {
                return ResourceManager.GetString("Student_agree_to_disclaimer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The deadline to register for the ceremony has passed..
        /// </summary>
        public static string Student_CeremonyDeadlinePassed {
            get {
                return ResourceManager.GetString("Student_CeremonyDeadlinePassed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  There was a problem sending you an email.  Please print this page for your records..
        /// </summary>
        public static string Student_Email_Problem {
            get {
                return ResourceManager.GetString("Student_Email_Problem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Student has multiple majors but did not supply a major code..
        /// </summary>
        public static string Student_Major_Code_Not_Supplied {
            get {
                return ResourceManager.GetString("Student_Major_Code_Not_Supplied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No matching ceremony found.  Please try your registration again..
        /// </summary>
        public static string Student_No_Ceremony_Found {
            get {
                return ResourceManager.GetString("Student_No_Ceremony_Found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No matching registration found.  Please try your registration again..
        /// </summary>
        public static string Student_No_Registration_Found {
            get {
                return ResourceManager.GetString("Student_No_Registration_Found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have successfully edited your commencement registration. .
        /// </summary>
        public static string Student_Register_Edit_Successful {
            get {
                return ResourceManager.GetString("Student_Register_Edit_Successful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have successfully registered for commencement..
        /// </summary>
        public static string Student_Register_Successful {
            get {
                return ResourceManager.GetString("Student_Register_Successful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have successfully submitted your registration petition..
        /// </summary>
        public static string Student_RegistrationPetition_Successful {
            get {
                return ResourceManager.GetString("Student_RegistrationPetition_Successful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXEC usp_SearchStudent :studentid.
        /// </summary>
        public static string StudentService_BannerLookup_SQL {
            get {
                return ResourceManager.GetString("StudentService_BannerLookup_SQL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXEC usp_SearchStudentByLogin :login.
        /// </summary>
        public static string StudentService_BannerLookupByLogin_SQL {
            get {
                return ResourceManager.GetString("StudentService_BannerLookupByLogin_SQL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXEC usp_LookupStudentName :studentid.
        /// </summary>
        public static string StudentService_BannerLookupName_SQL {
            get {
                return ResourceManager.GetString("StudentService_BannerLookupName_SQL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXEC usp_SearchStudent :studentid, :term.
        /// </summary>
        public static string StudentService_SearchStudent_SQL {
            get {
                return ResourceManager.GetString("StudentService_SearchStudent_SQL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXEC usp_SearchStudentByLogin :login, :term.
        /// </summary>
        public static string StudentService_SearchStudentByLogin_SQL {
            get {
                return ResourceManager.GetString("StudentService_SearchStudentByLogin_SQL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancellation.
        /// </summary>
        public static string Template_Cancellation {
            get {
                return ResourceManager.GetString("Template_Cancellation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Electronic Ticket Distribution.
        /// </summary>
        public static string Template_ElectronicTicketDistribution {
            get {
                return ResourceManager.GetString("Template_ElectronicTicketDistribution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email All Students.
        /// </summary>
        public static string Template_EmailAllStudents {
            get {
                return ResourceManager.GetString("Template_EmailAllStudents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Major Move.
        /// </summary>
        public static string Template_MoveMajor {
            get {
                return ResourceManager.GetString("Template_MoveMajor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notify Open Ticket Petition.
        /// </summary>
        public static string Template_NotifyOpenTicketPetitions {
            get {
                return ResourceManager.GetString("Template_NotifyOpenTicketPetitions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registration Confirmation.
        /// </summary>
        public static string Template_RegistrationConfirmation {
            get {
                return ResourceManager.GetString("Template_RegistrationConfirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registration Petition.
        /// </summary>
        public static string Template_RegistrationPetition {
            get {
                return ResourceManager.GetString("Template_RegistrationPetition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registration Petition - Approved.
        /// </summary>
        public static string Template_RegistrationPetition_Approved {
            get {
                return ResourceManager.GetString("Template_RegistrationPetition_Approved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remaining Tickets.
        /// </summary>
        public static string Template_RemainingTickets {
            get {
                return ResourceManager.GetString("Template_RemainingTickets", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ticket Petition.
        /// </summary>
        public static string Template_TicketPetition {
            get {
                return ResourceManager.GetString("Template_TicketPetition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ticket Petition - Decision.
        /// </summary>
        public static string Template_TicketPetition_Decision {
            get {
                return ResourceManager.GetString("Template_TicketPetition_Decision", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Visa Decision.
        /// </summary>
        public static string Template_VisaLetterDecision {
            get {
                return ResourceManager.GetString("Template_VisaLetterDecision", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;I agree to adhere to the University policy which prohibits the unlawful use, sale, distribution, dispensing, or possession of alcohol or of controlled substances by students on University premises and official University functions.  Students shall not use illegal substances or abuse legal substances in a manner that impairs scholarly activities or student life.  (Adapted from UCDP&amp;P  Manual Chapter 380, Section 18) Students violating this policy may be excluded from privileges and activities when there i [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Txt_Disclaimer {
            get {
                return ResourceManager.GetString("Txt_Disclaimer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If you selected to get your tickets through mail, your extra tickets will be mailed to you.  Otherwise you will have to come to the Dean&apos;s Office at 150 Mrak Hall to pickup your tickets..
        /// </summary>
        public static string Txt_ExtraTicketRequestDisclaimer {
            get {
                return ResourceManager.GetString("Txt_ExtraTicketRequestDisclaimer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;script type=&quot;text/javascript&quot;&gt;
        ///
        ///  var _gaq = _gaq || [];
        ///  _gaq.push([&apos;_setAccount&apos;, &apos;UA-5512876-5&apos;]);
        ///  _gaq.push([&apos;_trackPageview&apos;]);
        ///
        ///  (function() {
        ///    var ga = document.createElement(&apos;script&apos;); ga.type = &apos;text/javascript&apos;; ga.async = true;
        ///    ga.src = (&apos;https:&apos; == document.location.protocol ? &apos;https://ssl&apos; : &apos;http://www&apos;) + &apos;.google-analytics.com/ga.js&apos;;
        ///    var s = document.getElementsByTagName(&apos;script&apos;)[0]; s.parentNode.insertBefore(ga, s);
        ///  })();
        ///
        ///&lt;/script&gt;.
        /// </summary>
        public static string Txt_GoogleAnalytics {
            get {
                return ResourceManager.GetString("Txt_GoogleAnalytics", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome {0}.  After you have submitted your registration you may log back in and update any of your information up until the registration deadline..
        /// </summary>
        public static string Txt_Introduction {
            get {
                return ResourceManager.GetString("Txt_Introduction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome {0}.  You are registering AFTER the registration deadline.  Please note that due to the late registration we can not guarantee your name will appear in the program or that you will receive the maximum # of tickets alloted per person.  After the deadline tickets are distributed on a first come first serve basis..
        /// </summary>
        public static string Txt_Introduction_AfterPrintingDeadling {
            get {
                return ResourceManager.GetString("Txt_Introduction_AfterPrintingDeadling", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are eligible for multiple ceremonies, please &lt;strong&gt;choose the major&lt;/strong&gt; you would like to walk with during the ceremony..
        /// </summary>
        public static string Txt_MultipleCeremonies {
            get {
                return ResourceManager.GetString("Txt_MultipleCeremonies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;&lt;strong&gt;
        ///            According to our records you do not meet the requirements to participate in the {0} commencement ceremony.
        ///        &lt;/strong&gt;&lt;/p&gt;
        ///        
        ///        &lt;p&gt;&lt;strong&gt;
        ///            Please see our website to review the requirements: &lt;a href=&quot;http://caes.ucdavis.edu/NewsEvents/Events/Commencement/general-information&quot;&gt;http://caes.ucdavis.edu/NewsEvents/Events/Commencement/general-information&lt;/a&gt;
        ///        &lt;/strong&gt;&lt;/p&gt;
        ///        &lt;br/&gt;
        ///        &lt;p&gt;&lt;strong&gt;If you would like to submit a petition to partic [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Txt_NotAuthorized {
            get {
                return ResourceManager.GetString("Txt_NotAuthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This petition is to request authorization to participate in the College of Agricultural and Environmental Science’s graduation ceremony for {0}..
        /// </summary>
        public static string Txt_RegistrationPetition {
            get {
                return ResourceManager.GetString("Txt_RegistrationPetition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;
        ///        Thank you for submitting your petition. We will do our best to review your request within one week. 
        ///        You will be notified by email of the decision. 
        ///    &lt;/p&gt;
        ///    &lt;p&gt;
        ///        Petitions submitted after the &lt;a href=&quot;http://caes.ucdavis.edu/NewsEvents/Events/Commencement/important-dates&quot;&gt;registration deadline&lt;/a&gt; will be reviewed on first come first serve basis. 
        ///    &lt;/p&gt;
        ///    
        ///    &lt;h3&gt;Please review our FAQ page for any further questions: 
        ///        &lt;a href=&quot;http://caes.ucdavis.edu/NewsEvents/E [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Txt_RegistrationPetitionConfirmation {
            get {
                return ResourceManager.GetString("Txt_RegistrationPetitionConfirmation", resourceCulture);
            }
        }
    }
}
