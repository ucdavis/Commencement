using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;

namespace Commencement.Controllers.Services
{
    public interface IExcelService
    {
        byte[] Create(List<string> columns, List<List<string>> rows, HttpServerUtilityBase server);
    }

    public class ExcelService : IExcelService
    {
        public byte[] Create(List<string> columns, List<List<string>> rows, HttpServerUtilityBase server)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");

            // create header style
            var headerFont = workbook.CreateFont();
            headerFont.Boldweight = (short)FontBoldWeight.BOLD;
            var headerStyle = workbook.CreateCellStyle();
            headerStyle.FillBackgroundColor = HSSFColor.GREY_40_PERCENT.index;
            headerStyle.FillForegroundColor = HSSFColor.GREY_40_PERCENT.index;
            headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headerStyle.SetFont(headerFont);

            // Getting the row... 0 is the first row.
            var dataRow = sheet.CreateRow(0);
            for (int i = 0; i < columns.Count; i++)
            {
                dataRow.CreateCell(i).SetCellValue(columns.ElementAt(i));
                dataRow.GetCell(i).CellStyle = headerStyle;
            }

            var rowCount = 0;
            foreach (var a in rows)          // gets the array that represents a row
            {
                rowCount++;
                dataRow = sheet.CreateRow(rowCount);

                for (var i = 0; i < a.Count; i++)   // gets the individual cell
                {
                    dataRow.CreateCell(i).SetCellValue(a[i]);
                }
            }

            // Forcing formula recalculation...
            sheet.ForceFormulaRecalculation = true;

            // adjust column widths
            for (int i = 0; i < columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                return ms.ToArray();
            }
        }
    }
}