using ClosedXML.Excel;
using MMSL.Domain.Entities;
using MMSL.Domain.Entities.Fabrics;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MMSL.Server.Core.Import
{
    public class ExcelParser
    {
        public List<Fabric> ParseFabricExcel(string path)
        {
            List<Fabric> fabricsResult = new List<Fabric>();

            try
            {
                FileStream originalSourceFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                MemoryStream forParsingMemoryStream = new MemoryStream();
                originalSourceFileStream.CopyTo(forParsingMemoryStream);
                originalSourceFileStream.Dispose();
                originalSourceFileStream.Close();

                XLWorkbook xlWorkbook = new XLWorkbook(forParsingMemoryStream);

                foreach (IXLWorksheet xlSheet in xlWorkbook.Worksheets)
                {
                    IXLRow xlHeaderRow = xlSheet.RowsUsed().FirstOrDefault();
                    if (xlHeaderRow != null)
                    {
                        Tuple<string, IXLAddress>[] rawHeadersWithAddresses = xlHeaderRow.CellsUsed()
                            .Select<IXLCell, Tuple<string, IXLAddress>>(cell => new Tuple<string, IXLAddress>(cell.Value.ToString(), cell.Address))
                            .ToArray<Tuple<string, IXLAddress>>();

                        IEnumerable<IXLRow> xlDataRows = xlSheet.Rows(xlHeaderRow.RowNumber() + 1, xlSheet.RowsUsed().Last().RowNumber());

                        for (int i = 0; i < xlDataRows.Count(); i++)
                        {
                            try
                            {
                                
                                string[] relatedEntityPropertyKeys = new string[]{
                                    "Fabric Code",
                                    "Description",
                                    "Status",
                                    "Meters",
                                    "Mill",
                                    "Color",
                                    "Composition",
                                    "GSM",
                                    "Count",
                                    "Weave",
                                    "Pattern",
                                };

                                Fabric fabric = BuildEntityImportRow(xlDataRows.ElementAt(i), i, rawHeadersWithAddresses, relatedEntityPropertyKeys);

                                fabricsResult.Add(fabric);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Log.Logger.Warning(exc.Message);
            }

            return fabricsResult;
        }

        private Fabric BuildEntityImportRow(IXLRow xlRow, int rawIndex, IEnumerable<Tuple<string, IXLAddress>> rawHeadersStrings, string[] relatedEntityPropertyKeys)
        {
            Fabric assetGroupRow = new Fabric();

            if (xlRow != null)
            {
                for (int i = 0; i < rawHeadersStrings.Count(); i++)
                {
                    IXLCell xlCell = xlRow.Cell(rawHeadersStrings.ElementAt(i).Item2.ColumnNumber);

                    if (relatedEntityPropertyKeys.Contains<string>(rawHeadersStrings.ElementAt(i).Item1))
                    {
                        string targetPropertyName = rawHeadersStrings.ElementAt(i).Item1;

                        if (xlCell != null)
                        {
                            PropertyInfo[] props = assetGroupRow.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(HeaderAttribute))).ToArray<PropertyInfo>();
                            foreach (PropertyInfo property in props)
                            {
                                HeaderAttribute headerAttribute = property.GetCustomAttribute<HeaderAttribute>();
                                if (headerAttribute != null && headerAttribute.Header.Equals(targetPropertyName))
                                {
                                    if (targetPropertyName == "Status")
                                    {
                                        FabricStatuses result = ParseFabricStatus(TryExtractValueFromIXLCell(xlCell));
                                        property.SetValue(assetGroupRow, result);
                                    }
                                    else if (targetPropertyName == "Meters")
                                    {
                                        float result = ParseFabricNumber(TryExtractValueFromIXLCell(xlCell));
                                        property.SetValue(assetGroupRow, result);
                                    }
                                    else
                                    {
                                        property.SetValue(assetGroupRow, TryExtractValueFromIXLCell(xlCell));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return assetGroupRow;
        }

        private FabricStatuses ParseFabricStatus(string rawValue)
        {
            FabricStatuses result = default(FabricStatuses);

            string normalizedValue = new string(rawValue.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray()).ToLower();

            if (FabricStatuses.Discontinued.ToString().ToLower() == normalizedValue)
                result = FabricStatuses.Discontinued;
            else if (FabricStatuses.InStock.ToString().ToLower() == normalizedValue)
                result = FabricStatuses.InStock;
            else if (FabricStatuses.OutOfStock.ToString().ToLower() == normalizedValue)
                result = FabricStatuses.OutOfStock;

            //Enum.TryParse<FabricStatuses>(new string(rawValue.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray()), out result);

            return result;
        }

        private float ParseFabricNumber(string rawValue)
        {
            float result = 0;

            float.TryParse(rawValue, out result);

            return result;
        }

        private string TryExtractValueFromIXLCell(IXLCell xlCell)
        {

            string result = null;

            if (xlCell != null)
            {
                /// Don't remove it, not sure how it works but 
                /// IXLCell cell value will be changed (issue: date-time can be evaluated as ticks/date-format)
                //string richTextValue = xlCell.RichText?.ToString();
                //string value = xlCell.Value?.ToString();
                string cachedValue = xlCell.CachedValue?.ToString();

                /// TODO: urgent fix <see cref="IXLCell.Value"/> throws exception `Unknown function...`
                //if (string.IsNullOrEmpty(xlCell.FormulaA1)
                //    && string.IsNullOrEmpty(xlCell.FormulaR1C1)) {
                //    result = xlCell.Value?.ToString();
                //} else {
                //    result = xlCell.CachedValue?.ToString();
                //}

                result = cachedValue;
            }

            return result;
        }
    }
}
