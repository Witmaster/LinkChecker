using System;
using System.Net;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace LinkChecker
{
    class ExcelHandler
    {
        public bool isOK = false;
        private static Microsoft.Office.Interop.Excel.Application xlApp;
        private static Workbook xlWorkBook;
        private static Worksheet currentSheet;
        private static string linkurl;
        public ExcelHandler(string filename, string url)
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                isOK = false;
                MessageBox.Show("Microsoft Excel не установлен или установлен некорректно");
                System.Windows.Forms.Application.Exit();          
            }
            else
            {
                isOK = true;
                xlWorkBook = xlApp.Workbooks.Open(filename);
                xlWorkBook.SaveAs(filename);
                linkurl = url;
            }
        }

        public void Parse()
        {
            currentSheet = xlWorkBook.Worksheets.get_Item(1);
            Range range = currentSheet.UsedRange;
            string page = "";
            for (int i = 1; i < range.Rows.Count+1; i++)
            { 
                    using (WebClient clien = new WebClient())
                    {
                    try
                    {
                        page = clien.DownloadString((string)(currentSheet.Cells[i, 1] as Range).Value);
                    }
                    catch (Exception) { }
                        if (page.Contains(linkurl))
                        {
                            currentSheet.Cells[i, 2] = "Ссылка есть";
                        }
                        else
                    {
                            currentSheet.Cells[i, 2] = "Ссылки нет";
                    }
                }
            }
            xlWorkBook.Save();
            Dispose();
        }
        //public void WriteToTab(string data, string name, int sheet, int offset)
        //{
        //    offset++;
        //    currentSheet = xlWorkBook.Worksheets.get_Item(sheet);
        //    if (name.Length > 30) { currentSheet.Name = name.Remove(30); } else { currentSheet.Name = name; }
        //    currentSheet.Cells[offset, 1] = "Keyword";
        //    currentSheet.Cells[offset, 2] = "Search volume";
        //    currentSheet.Cells[offset, 3] = "Competition";
        //    currentSheet.Cells[offset, 4] = "CPC";
        //    currentSheet.Cells[offset, 5] = "Advisability";
        //    currentSheet.Cells[offset, 6] = "Root";
        //    var dataarray = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        //    string[] splitline = new string[6];
        //    for (int i = 1; i < dataarray.Length; i++)
        //    {
        //        splitline = dataarray[i].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        //        for (int j = 0; j < splitline.Length; j++)
        //        {
        //            currentSheet.Cells[i + offset, j + 1] = splitline[j];
        //        }
        //        string temp = advisabilityFormula;
        //        temp = temp.Replace("|", (i+ offset).ToString());
        //        currentSheet.Cells[i + offset, 5] = temp;
        //        currentSheet.Cells[i + offset, 6] = currentSheet.Name;
        //    }
       
        //}

        public void Dispose()
        {
            xlWorkBook.Save();
            xlWorkBook.Close(true,Type.Missing,Type.Missing);
            xlApp.Quit();
            ReleaseObject(currentSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);

        }
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
