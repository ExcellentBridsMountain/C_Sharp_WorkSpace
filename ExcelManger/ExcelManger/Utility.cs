using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;

namespace ExcelManger
{
    static class Utility
    {

        internal static RangeIndex GetCellIndexWithValue(Worksheet e_WorkSheet,string cellValue,int maxRow, int maxColumn)
        {
            for (int rowIndex = 1; rowIndex <= maxRow; rowIndex++)
            {
                for (int columnIndex = 1; columnIndex <= maxColumn; columnIndex++)
                {
                    string strCellText = ((Range)e_WorkSheet.Cells[rowIndex, columnIndex]).Text;

                    if ( true == stringMatch(cellValue,strCellText) )
                    {
                        return new RangeIndex(rowIndex, columnIndex);
                    }
                }
            }

            return new RangeIndex(-1, -1);
        }

        private static  bool stringMatch(string strgoal,string strmatch)
        {
            string rule = @"\s*" + @strgoal + @"\s*";
            Regex regex = new Regex(rule);
            try
            {
                return regex.IsMatch(strmatch);
            }
            catch (Exception)
            {
                return false;
            }
        }


    }

    class RangeIndex
    {
        int rowIndex;
        int columnIndex;

        internal RangeIndex(int rowIndex,int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
        }
    }
}
