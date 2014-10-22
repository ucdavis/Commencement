using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Commencement.Controllers.Services
{
    public interface IReportService
    {
        byte[] GenerateLetter(VisaLetter visaLetter);
        byte[] WritePdfWithErrorMessage(string message);
    }

    public class ReportService : IReportService
    {

        #region Declarations
        // colors
        private readonly BaseColor _headerColor = BaseColor.GRAY;

        // standard body font
        private readonly Font _pageHeaderFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE);
        private readonly Font _headerFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
        private readonly Font _summaryFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.GRAY);

        private readonly Font _font = new Font(Font.FontFamily.TIMES_ROMAN, 10);
        private readonly Font _boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
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
            table.AddCell(InitializeCell(visaLetter.DateDecided.Value.Date.ToString("MMMM dd, yyyy"), halignment: Element.ALIGN_RIGHT, bottomBorder: false));
            doc.Add(table);



            doc.Close();

            return ms.ToArray();
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
          , int? overrideRightPadding = null)
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

            return cell;
        }
    }
}