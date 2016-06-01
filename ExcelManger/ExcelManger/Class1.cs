using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace ExcelManger
{

    abstract class ExcelMangerBase
    {
        Application e_app;

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
            this.doBeforeCreatExcelProcess();

            while ( false == this.creatExcelProcess())
            {
                ;
            }

            this.doAfterCreatExcelProcess();

            return this.e_app;

        }


    }


}
