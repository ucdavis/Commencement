using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace Commencement.Mvc.Controllers.Services
{
    public interface IExcelService
    {
        byte[] Create(List<string> columns, List<List<string>> rows, HttpServerUtilityBase server);
        byte[] CreateExtra(List<string> columns, List<RegistrationSurvey> registrationSurveys);
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

        public byte[] CreateExtra(List<string> columns, List<RegistrationSurvey> registrationSurveys)
        {
            var extraColumns = new List<string>();
            extraColumns.Add("Name");
            extraColumns.Add("Student Id");
            extraColumns.Add("Units Completed");
            extraColumns.Add("Major");

            extraColumns.AddRange(columns);


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
            for (int i = 0; i < extraColumns.Count; i++)
            {
                dataRow.CreateCell(i).SetCellValue(extraColumns.ElementAt(i));
                dataRow.GetCell(i).CellStyle = headerStyle;
            }

            var rowCount = 0;

            foreach (var registrationSurvey in registrationSurveys)
            {
                rowCount++;
                dataRow = sheet.CreateRow(rowCount);

                if (registrationSurvey.RegistrationParticipation != null)
                {
                    var student = registrationSurvey.RegistrationParticipation.Registration.Student;
                    dataRow.CreateCell(0).SetCellValue(student.FullName);
                    dataRow.CreateCell(1).SetCellValue(student.StudentId);
                    dataRow.CreateCell(2).SetCellValue(student.TotalUnits.ToString());
                    dataRow.CreateCell(3).SetCellValue(student.ActualStrMajors);
                }
                else
                {
                    dataRow.CreateCell(0).SetCellValue("???");
                    dataRow.CreateCell(1).SetCellValue("???");
                    dataRow.CreateCell(2).SetCellValue("???");
                    dataRow.CreateCell(3).SetCellValue("???");
                }

                var rows = registrationSurvey.SurveyAnswers.OrderBy(o => o.SurveyField.Order).Select(s => s.Answer).ToList();
                for (int i = 0; i < rows.Count; i++)
                {
                    dataRow.CreateCell((i+4)).SetCellValue(rows[(i)]);
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