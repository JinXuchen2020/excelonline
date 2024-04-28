using ExcelOnline.Api.Transfers;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Text.RegularExpressions;

namespace ExcelOnline.Api.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        public async Task<IEnumerable<T>> ImportFile<T>(IFormFile file, List<ExcelHeader> expectHeaders)
        {
            if (file == null)
                throw new Exception($"导入模板失败-无法加载文件");

            var extension = Path.GetExtension(file.FileName);
            try
            {
                using var stream = file.OpenReadStream();
                IWorkbook wk;
                if (extension.Equals(".xls"))
                {
                    wk = new HSSFWorkbook(stream);
                }
                else
                {
                    wk = new XSSFWorkbook(stream);
                }

                return await ParseExcel<T>(wk?.GetSheetAt(0), expectHeaders);
            }
            catch (Exception e)
            {
                throw new Exception($"导入模板失败-{e.Message}");
            }
        }

        public async Task<byte[]> CreateExcel<T>(string sheetName, List<ExcelHeader> headers, IEnumerable<T> dataList)
        {
            var workbook = new XSSFWorkbook();
            CreateSheet(workbook, sheetName, headers);

            headers.Where(c => c.Constraints != null && c.Constraints.Any()).ToList().ForEach(c =>
            {
                var index = headers.IndexOf(c);
                AddValidationData(workbook, 0, index, index, c.Constraints.ToArray());
            });

            await this.InsertData(workbook, headers, dataList);

            var ms = new MemoryStream();
            workbook.Write(ms);

            return ms.ToArray();
        }

        public async Task<List<T>> ParseExcel<T>(ISheet sheet, List<ExcelHeader> expectHeaders)
        {
            var importList = new List<T>();
            if (sheet != null)
            {
                var headers = sheet.GetRow(0).Cells;
                if (!headers.Select(c => c.StringCellValue).SequenceEqual(expectHeaders.Select(c => c.HeaderName)))
                {
                    throw new Exception("模板列名或列数有误!");
                }

                var properties = typeof(T).GetProperties();

                for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
                {
                    var row = sheet.GetRow(i);
                    var result = (T)Activator.CreateInstance(typeof(T));
                    foreach (var property in properties)
                    {
                        var expectHeader = expectHeaders.Find(c => c.PropetyName == property.Name);
                        if (expectHeader != null)
                        {
                            var index = headers.FindIndex(c => c.StringCellValue.Equals(expectHeader.HeaderName));
                            if (index >= 0)
                            {
                                var cellValue = row.GetCell(index);
                                switch (property.PropertyType)
                                {
                                    case var c when c == typeof(bool) || c == typeof(bool?):
                                        if (expectHeader.Constraints.Any())
                                        {
                                            if(cellValue != null)
                                            {
                                                property.SetValue(result, cellValue.StringCellValue.ToLower() == expectHeader.Constraints.ElementAt(0).ToLower());
                                            }
                                        }
                                        break;
                                    case var c when c == typeof(DateTime?) || c == typeof(DateTime):
                                        property.SetValue(result, cellValue.DateCellValue);
                                        break;
                                    case var c when c == typeof(int):
                                        if(cellValue.CellType == CellType.Numeric)
                                        {
                                            property.SetValue(result, (int)cellValue.NumericCellValue);
                                        }
                                        else
                                        {
                                            var intValue = int.Parse(Regex.Match(cellValue.StringCellValue, "/d").Value);
                                            property.SetValue(result, intValue);
                                        }
                                        break;
                                    default:
                                        var strValue = cellValue?.ToString();
                                        property.SetValue(result, strValue);
                                        break;
                                }
                            }
                        }
                    }

                    importList.Add(result);
                }
            }

            return await Task.FromResult(importList);
        }

        private async Task InsertData<T>(XSSFWorkbook workbook, List<ExcelHeader> headers, IEnumerable<T> dataList)
        {
            ISheet sheet = workbook.GetSheetAt(0);

            var cellStyle = workbook.CreateCellStyle();
            cellStyle.WrapText = true;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            var isHasSubHeader = headers.Any(c => c.Children.Count() > 0);
            var actualHeaders = headers.Where(c => !string.IsNullOrEmpty(c.PropetyName)).ToList();
            if (isHasSubHeader)
            {
                foreach(var subHeader in headers.Where(c => c.Children.Count() > 0))
                {
                    actualHeaders.AddRange(subHeader.Children);
                }
            }

            for (var index = 0; index < dataList.Count(); index++)
            {
                var row = isHasSubHeader ? sheet.CreateRow(index + 2) : sheet.CreateRow(index + 1);
                for (var i = 0; i < actualHeaders.Count; i++)
                {
                    var cell = row.CreateCell(i);
                    var headerValue = actualHeaders[i];
                    if (headerValue.Wrap.HasValue && headerValue.Wrap.Value)
                    {
                        cell.CellStyle = cellStyle;
                    }
                    var propertyName = headerValue.PropetyName;
                    var property = typeof(T).GetProperty(propertyName);
                    var cellValue = property.GetValue(dataList.ElementAt(index));
                    switch (property.PropertyType)
                    {
                        case var c when c == typeof(bool):
                            if (headerValue.Constraints.Any())
                            {
                                cell.SetCellValue((bool)cellValue ? headerValue.Constraints.ElementAt(0) : headerValue.Constraints.ElementAt(1));
                            }
                            else
                            {
                                cell.SetCellValue((bool)cellValue);
                            }
                            
                            break;
                        case var c when c == typeof(bool?):
                            if (headerValue.Constraints.Any())
                            {
                                cell.SetCellValue(((bool?)cellValue).Value ? headerValue.Constraints.ElementAt(0) : headerValue.Constraints.ElementAt(1));
                            }
                            else
                            {
                                cell.SetCellValue(((bool?)cellValue).Value);
                            }
                            break;
                        case var c when c == typeof(DateTime?):
                            if (((DateTime?)cellValue).HasValue)
                            {
                                var dateTimeValue = ((DateTime?)cellValue).Value;
                                cell.SetCellValue(dateTimeValue.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                cell.SetCellValue(cellValue as string);
                            }
                            
                            break;
                        case var c when c == typeof(int):
                            cell.SetCellValue((int)cellValue);
                            break;
                        case var c when c == typeof(string[]):
                            var target = string.Join("\n", (string[])cellValue);
                            cell.SetCellValue(target);
                            break;
                        default:
                            cell.SetCellValue(cellValue as string);
                            break;
                    }
                }
            }

            await Task.CompletedTask;
        }

        private static XSSFWorkbook CreateSheet(XSSFWorkbook workbook, string sheetName, List<ExcelHeader> headers)
        {
            var sheet = (XSSFSheet)workbook.CreateSheet(sheetName);
            var isHasSubHeader = headers.Any(c => c.Children.Count() > 0);

            var headerRow = sheet.CreateRow(0);
            var subHeaderRow = sheet.CreateRow(1);
            int columnIndex = 0;
            foreach (var header in headers) 
            {
                var headerWith = header.HeaderWidth;
                var cell = headerRow.CreateCell(columnIndex);
                cell.SetCellValue(header.HeaderName);
                foreach (var subHeader in header.Children) 
                {
                    sheet.SetColumnWidth(columnIndex, subHeader.HeaderWidth * 256);
                    var subCell = subHeaderRow.CreateCell(columnIndex);
                    subCell.SetCellValue(subHeader.HeaderName);
                    columnIndex++;
                }

                if (header.Children.Count() > 0) 
                {
                    var cellStyle = workbook.CreateCellStyle();
                    cellStyle.WrapText = true;
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    cell.CellStyle = cellStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, columnIndex - header.Children.Count(), columnIndex - 1));
                }
                else
                {
                    sheet.SetColumnWidth(columnIndex, headerWith * 256);
                    if (isHasSubHeader) 
                    {
                        var cellStyle = workbook.CreateCellStyle();
                        cellStyle.WrapText = true;
                        cellStyle.VerticalAlignment = VerticalAlignment.Center;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 1, columnIndex, columnIndex));
                    }
                    columnIndex++;
                }
            }

            return workbook;
        }

        private static void AddValidationData(XSSFWorkbook workbook, int sheetNum, int startColumn, int endColumn, string[] constraintList)
        {
            var sheet = (XSSFSheet)workbook.GetSheetAt(sheetNum);
            var validationHelper = new XSSFDataValidationHelper(sheet);

            var typeAddressList = new CellRangeAddressList(1, 65535, startColumn, endColumn);
            var typeConstraint = validationHelper.CreateExplicitListConstraint(constraintList);
            var typeDataValidation = validationHelper.CreateValidation(typeConstraint, typeAddressList);
            typeDataValidation.SuppressDropDownArrow = true;
            typeDataValidation.ShowErrorBox = true;
            typeDataValidation.ErrorStyle = 0;
            typeDataValidation.CreateErrorBox("InvalidValue", "请从下拉菜单里选择");
            sheet.AddValidationData(typeDataValidation);
        }
    }
}
