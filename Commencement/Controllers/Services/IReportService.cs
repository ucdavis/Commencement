using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;

namespace Commencement.Controllers.Services
{
    public interface IReportService
    {
        byte[] GenerateLetter(VisaLetter visaLetter);
        byte[] WritePdfWithErrorMessage(string message);
    }

    public class ReportService : IReportService
    {
        private readonly IRepository _repository;

        public ReportService (IRepository repository)
        {
            _repository = repository;
            //To use a custom font, but it still doesn't look like the berkley font in the sample letter.
            //string fontpath = HttpContext.Current.Server.MapPath("~/Content/font/BerkeleyUCDavis-Medium.ttf");
            //BaseFont customfont = BaseFont.CreateFont(fontpath, BaseFont.CP1252, BaseFont.EMBEDDED);

            //_font = new Font(customfont, 12);
        }

        #region Declarations
        // colors
        private readonly BaseColor _headerColor = BaseColor.GRAY;

        // standard body font
        private readonly Font _pageHeaderFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE);
        private readonly Font _headerFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
        private readonly Font _summaryFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.GRAY);



        private readonly Font _font = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        private readonly Font _linkFont = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.UNDERLINE, new BaseColor(0,0,255));
        private readonly Font _boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
        private readonly Font _italicFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.ITALIC);
        private readonly Font _italicFontWhite = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLDITALIC, BaseColor.WHITE);
        private readonly Font _smallPrint = new Font(Font.FontFamily.HELVETICA, 8);

       

        // width of the content
        private float _pageWidth;
        private float _pageHeight; 
        #endregion Declarations





        public byte[] GenerateLetter(VisaLetter visaLetter)
        {
            var doc = InitializeDocument();
            var ms = new MemoryStream();
            var writer = PdfWriter.GetInstance(doc, ms); //This binds the memory stream to the doc.
            doc.Open();

            var table = InitializeTable();

            WriteHeader(table, doc);

            #region Date Approved
            table = InitializeTable();
            table.AddCell(InitializeCell(visaLetter.DateDecided.HasValue ? visaLetter.DateDecided.Value.Date.ToString("MMMM dd, yyyy") : DateTime.Now.Date.ToString("MMMM dd, yyyy"), halignment: Element.ALIGN_RIGHT, bottomBorder: false, overridePaddingTop:10, overridePaddingBottom:20));
            #endregion Date Approved

            table.AddCell(InitializeCell("To Whom It May Concern", halignment: Element.ALIGN_LEFT, bottomBorder: false, overridePaddingBottom:25));

            table.AddCell(InitializeCell(string.Format("RE:  {0} {1} Request for Visa", visaLetter.StudentFirstName, visaLetter.StudentLastName), halignment: Element.ALIGN_LEFT, bottomBorder: false, overridePaddingBottom:10));

            table.AddCell(InitializeCell(string.Format("{0} {1} is completing {2} requirements for a {3} degree in {4}.  {5} is participating in the College of {6}’s commencement ceremony held at the UC Davis campus in Davis, California, United States of America on {7}."
                , visaLetter.StudentFirstName               //0
                , visaLetter.StudentLastName                //1
                , visaLetter.Gender == "M" ? "his" : "her"  //2
                , visaLetter.Degree                         //3
                , visaLetter.MajorName                      //4
                , visaLetter.Gender == "M" ? "He" : "She"   //5
                , visaLetter.CollegeName                    //6
                , visaLetter.CeremonyDateTime.HasValue ? visaLetter.CeremonyDateTime.Value.Date.ToString("MMMM dd, yyyy") : "ERROR") //7
                , halignment: Element.ALIGN_LEFT, bottomBorder: false));

            table.AddCell(InitializeCell(string.Format("{0} {1} would be extremely appreciative if {2} {3}, {4} {5} could attend the ceremony.  {6}. {4} {5} resides at:"
                , visaLetter.Gender == "M" ? "Mr." : "Ms."  //0
                , visaLetter.StudentLastName                //1
                , visaLetter.Gender == "M" ? "his" : "her"  //2
                , visaLetter.RelationshipToStudent          //3
                , visaLetter.RelativeFirstName              //4
                , visaLetter.RelativeLastName               //5
                , visaLetter.RelativeTitle                  //6
                )
                , halignment: Element.ALIGN_LEFT, bottomBorder: false));

            table.AddCell(InitializeCell(visaLetter.RelativeMailingAddress, halignment: Element.ALIGN_LEFT, bottomBorder: false, overridePaddingBottom: 10));

            table.AddCell(InitializeCell(string.Format("This is a memorable occasion and we hope that {0}. {1} {2} can attend."
                , visaLetter.RelativeTitle                  //0
                , visaLetter.RelativeFirstName              //1
                , visaLetter.RelativeLastName               //2
                ), halignment: Element.ALIGN_LEFT, bottomBorder: false));

            WriteSignature(visaLetter, table, doc);


           // doc.Add(table);




            WriteFooter(visaLetter, writer);

            doc.Close();

            return ms.ToArray();
        }

        private void WriteHeader(PdfPTable table, Document doc)
        {
            table.AddCell(InitializeCell("UNIVERSITY OF CALIFORNIA, DAVIS", halignment: Element.ALIGN_LEFT, bottomBorder: false, font: _boldFont));
            doc.Add(table);

            string url = HttpContext.Current.Server.MapPath("~/Images/visaLetterHeader.gif");
            var img = Image.GetInstance(new Uri(url));
            img.ScaleToFit(_pageWidth, _pageHeight);
            doc.Add(img);


            table = InitializeTable();
            table.AddCell(InitializeCell("One Shields Avenue", halignment: Element.ALIGN_RIGHT, bottomBorder: false, font: _smallPrint, padding: false));
            table.AddCell(InitializeCell("Davis, California  95616", halignment: Element.ALIGN_RIGHT, bottomBorder: false, font: _smallPrint, padding: false));
            doc.Add(table);
        }

        private void WriteSignature(VisaLetter visaLetter, PdfPTable table, Document doc)
        {
            string url;
            Image img;
            var user = _repository.OfType<vUser>().Queryable.FirstOrDefault(a => a.LoginId == visaLetter.ApprovedBy);
            Check.Require(user != null, "Approval User is required");

            var cell = InitializeCell("Should you need more information, please contact me at ", halignment: Element.ALIGN_LEFT, bottomBorder: false);
            var anchor = new Anchor(user.Email, _linkFont);
            anchor.Reference = string.Format("mailto:{0}", user.Email);

            cell.Phrase.Add(anchor);

            table.AddCell(cell);

            doc.Add(table);

            table = InitializeTable(2);
            table.SetWidths(new float[]{40,60});
            table.AddCell(InitializeCell("", halignment: Element.ALIGN_LEFT, bottomBorder: false, overridePaddingTop: 20));

            table.AddCell(InitializeCell("Kind regards.", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false, overridePaddingTop: 20));
            

            url = HttpContext.Current.Server.MapPath(string.Format("~/Images/vl_{0}_signature.png", user.LoginId.ToLower().Trim()));
            if (File.Exists(url))
            {
                img = Image.GetInstance(new Uri(url));
                img.ScaleToFit(140f, 40f);
            }
            else
            {
                throw new Exception("Image not found " + url);
            }

            var imageCell = new PdfPCell(img);
            imageCell.BorderWidthLeft = 0;
            imageCell.BorderWidthRight = 0;
            imageCell.BorderWidthTop = 0;
            imageCell.BorderWidthBottom = 0;
            imageCell.HorizontalAlignment = Element.ALIGN_LEFT;

            table.AddCell(InitializeCell("", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false));
            table.AddCell(imageCell); // (InitializeCell(img, halignment: Element.ALIGN_RIGHT, bottomBorder: false));


            table.AddCell(InitializeCell("", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false));
            table.AddCell(InitializeCell(user.FullName, halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false));

            table.AddCell(InitializeCell("", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false, overridePaddingTop: 0));
            table.AddCell(InitializeCell("Commencement Coordinator", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false, overridePaddingTop: 0));

            table.AddCell(InitializeCell("", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false, overridePaddingTop: 0));
            table.AddCell(InitializeCell("UC Davis", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false, overridePaddingTop: 0));

            table.AddCell(InitializeCell("", halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false, overridePaddingTop: 0));
            table.AddCell(InitializeCell(user.Phone, halignment: Element.ALIGN_LEFT, bottomBorder: false, padding: false, overridePaddingTop: 0));

            doc.Add(table);
        }

        private void WriteFooter(VisaLetter visaLetter, PdfWriter writer)
        {
            var ct = new ColumnText(writer.DirectContent);
            Phrase myText = new Phrase(string.Format("Letter Id:  {0}", visaLetter.ReferenceGuid), _smallPrint);
            ct.SetSimpleColumn(myText, 0, 50, _pageWidth + 40, 0, 0, Element.ALIGN_RIGHT);
            ct.Go();
        }

        public byte[] WritePdfWithErrorMessage(string message)
        {
            var doc = InitializeDocument();
            var ms = new MemoryStream();
            var writer = PdfWriter.GetInstance(doc, ms); //This binds the memory stream to the doc.
            doc.Open();

            var table = InitializeTable();
            table.AddCell(InitializeCell(message, halignment: Element.ALIGN_CENTER, bottomBorder: false));
            doc.Add(table);

            doc.Close();

            return ms.ToArray();
        }



        private Document InitializeDocument()
        {
            var doc = new Document(PageSize.LETTER, 36 /* left */, 36 /* right */, 40 /* top */, 52 /* bottom */);
            doc.SetPageSize(PageSize.LETTER);

            // set the variable for the page's actual content size
            _pageWidth = doc.PageSize.Width - (doc.LeftMargin + doc.RightMargin);
            _pageHeight = doc.PageSize.Height - (doc.TopMargin + doc.BottomMargin);

            return doc;
        }

        private PdfPTable InitializeTable(int columns = 1)
        {
            var table = new PdfPTable(columns);

            // set the styles
            table.TotalWidth = _pageWidth;
            table.LockedWidth = true;
            table.SpacingAfter = 2f;

            return table;
        }

        //TODO: Verify what we need from here. This was copied from Ace.
        private PdfPCell InitializeCell(string text = null
          , Font font = null
          , bool header = false
          , int? halignment = null
          , int? valignment = null
          , int? colspan = null
          , bool sideBorders = false
          , bool topBottomBorders = false
          , bool bottomBorder = true
          , bool padding = true
          , BaseColor backgroundColor = null
          , bool topBoarder = false
          , bool bottomExtraBoarder = false
          , bool leftBoarder = false
          , bool rightBoarder = false
          , int? overrideLeftPadding = null
          , int? overrideRightPadding = null
          , int? overridePaddingTop = null
          , int? overridePaddingBottom = null)
        {
            var cell = new PdfPCell();
            if (!string.IsNullOrEmpty(text)) cell = new PdfPCell(new Phrase(text, font ?? _font));

            if (header)
            {
                cell.BackgroundColor = _headerColor;
            }

            if (halignment.HasValue)
            {
                cell.HorizontalAlignment = halignment.Value;
            }

            if (valignment.HasValue)
            {
                cell.VerticalAlignment = valignment.Value;
            }

            if (colspan.HasValue)
            {
                cell.Colspan = colspan.Value;
            }

            if (backgroundColor != null)
            {
                cell.BackgroundColor = backgroundColor;
            }

            if (padding)
            {
                cell.Padding = 5;
            }

            if (!sideBorders)
            {
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
            }

            if (!topBottomBorders)
            {
                cell.BorderWidthTop = 0;
                if (!bottomBorder)
                {
                    cell.BorderWidthBottom = 0;
                }
            }
            if (rightBoarder)
            {
                cell.BorderWidthRight = 0.8f;
            }
            if (leftBoarder)
            {
                cell.BorderWidthLeft = 0.8f;
            }
            if (topBoarder)
            {
                cell.BorderWidthTop = 0.8f;
            }
            if (bottomExtraBoarder)
            {
                cell.BorderWidthBottom = 0.8f;
            }

            if (overrideLeftPadding.HasValue)
            {
                cell.PaddingLeft = overrideLeftPadding.Value;
            }
            if (overrideRightPadding.HasValue)
            {
                cell.PaddingRight = overrideRightPadding.Value;
            }
            if (overridePaddingTop.HasValue)
            {
                cell.PaddingTop = overridePaddingTop.Value;
            }
            if (overridePaddingBottom.HasValue)
            {
                cell.PaddingBottom = overridePaddingBottom.Value;
            }

            return cell;
        }
    }
}