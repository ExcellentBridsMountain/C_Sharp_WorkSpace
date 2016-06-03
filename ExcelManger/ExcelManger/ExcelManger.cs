using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace ExcelManger
{

    abstract class ExcelMangerBase
    {
        protected Application e_app;

        /// <summary>
        /// hook
        /// </summary>
        protected virtual void doBeforeCreatExcelProcess()
        {
        }

        private bool creatExcelProcess()
        {
            if ( null == this.e_app)
            {
                try
                {
                    this.e_app = new Application();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }

            return true;
        }

        /// <summary>
        /// hook
        /// </summary>
        protected virtual void doAfterCreatExcelProcess()
        {
        }

        internal Application GetExcelApplication()
        {
            if ( null == this.e_app )
            {
                this.doBeforeCreatExcelProcess();

                while (false == this.creatExcelProcess())
                {
                    ;
                }

                this.doAfterCreatExcelProcess();
            }

            return this.e_app;
        }


    }

    class ExcelManger : ExcelMangerBase
    {
        /// <summary>
        /// Excel Application's Friend Name note don't have ".exe"
        /// </summary>
        private static string excelFriendName = "EXCEL";
        private List<Process> lstBefore;
        private List<Process> lstAfter;
        private List<int> lstCreatedProcessId ;

        private static ExcelManger singleTag = null;

        private Workbook e_Workbook = null;

        private Worksheet e_WorkSheet = null;

        private ExcelManger()
        {
          lstBefore = new List<Process>();
          lstAfter = new List<Process>();
          lstCreatedProcessId = new List<int>();
        }

        /// <summary>
        /// Open a Excel workbook and select sheet one
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        internal bool OpenWorkBook(string strPath)
        {
            if ( null == this.e_app)
            {
                this.GetExcelApplication();
            }

            Regex regex=new Regex(@".*\.xlsx$");

            if ( !File.Exists(strPath) && !regex.IsMatch(strPath)) //check
            {
                return false;
            }

            try
            {
                this.e_Workbook = this.e_app.Workbooks.Open(strPath);
                this.e_WorkSheet = this.e_Workbook.Worksheets[1];
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Get a Excel workSheet by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal Worksheet GetAWorkSheet(int index)
        {
            if ( null == this.e_Workbook )
            {
                return null;
            }

            if ( this.e_Workbook.Worksheets.Count < index)
            {
                return null;
            }

            return this.e_Workbook.Worksheets[index];
        }

        /// <summary>
        /// Get a Excel workSheet by sheetName
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        internal Worksheet GetAWorkSheet(string sheetName)
        {
            if (null == this.e_Workbook)
            {
                return null;
            }

            foreach (Worksheet tempSheet in this.e_Workbook.Worksheets)
            {
                if ( tempSheet.Name.Equals(sheetName) )
                {
                    return tempSheet;
                }
            }

            return null;
        }


        internal void Close()
        {
            this.e_app.Quit();

            foreach (var item in this.lstCreatedProcessId)
            {
                Process tempProcess = Process.GetProcessById(item);
                if (tempProcess.HasExited)
                {
                    tempProcess.Kill();
                }
            }
            this.e_app = null;
        }

        internal static ExcelManger GetExcelManger()
        {
            if ( null == singleTag)
            {
                singleTag = new ExcelManger();
            }

            return singleTag;
        }

        protected override void doBeforeCreatExcelProcess()
        {
            this.lstBefore.Clear();
            this.lstBefore = getProcessList();
        }

        protected override void doAfterCreatExcelProcess()
        {
            this.lstAfter.Clear();
            this.lstAfter = getProcessList();
            getCreatedProcessId();
        }

        private void getCreatedProcessId()
        {
            this.lstCreatedProcessId.Clear();
            if (0 == this.lstBefore.Count)
            {
                foreach (var item in lstAfter)
                {
                    this.lstCreatedProcessId.Add(item.Id);
                }
            }
            else
            {
                bool findTag = false;
                foreach (var pAfter in this.lstAfter)
                {
                    findTag = false;
                    foreach (var pBefore in this.lstBefore)
                    {
                        if (pBefore.Id == pAfter.Id)
                        {
                            findTag = true;
                            break;
                        }
                    }

                    if (false == findTag)
                    {
                        this.lstCreatedProcessId.Add(pAfter.Id);
                    }

                }
            }

            this.lstAfter.Clear();
            this.lstBefore.Clear();
        }

        private List<Process> getProcessList()
        {
            return Process.GetProcessesByName(excelFriendName).ToList();
        }

    }


}
