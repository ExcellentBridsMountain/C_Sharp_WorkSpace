using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

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

        private ExcelManger()
        {
            List<Process> lstBefore = new List<Process>();
            List<Process> lstAfter = new List<Process>();
            List<int> lstCreatedProcessId = new List<int>();
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

        internal void Close()
        {
            this.e_app.Quit();

            foreach (var item in this.lstCreatedProcessId )
            {
                Process tempProcess = Process.GetProcessById(item);
                if ( tempProcess.HasExited )
                {
                    tempProcess.Kill();
                }
            }
            this.e_app = null;
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
