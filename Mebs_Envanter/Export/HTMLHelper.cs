using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace Mebs_Envanter.Export
{
    internal class HTMLHelper : FileExportHelper
    {
        private String ConvertDataTableToHtmlString(DataTable targetTable)
        {


            StringBuilder htmlBuilder = new StringBuilder();


            htmlBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
            htmlBuilder.Append("style='border: solid 1px Black; font-size: small;'>");

            //Create Header Row
            htmlBuilder.Append("<tr align='left' valign='top'>");

            foreach (DataColumn targetColumn in targetTable.Columns)
            {
                htmlBuilder.Append("<td align='left' valign='top'>");
                htmlBuilder.Append(targetColumn.ColumnName);
                htmlBuilder.Append("</td>");
            }

            htmlBuilder.Append("</tr>");

            //Create Data Rows
            foreach (DataRow myRow in targetTable.Rows)
            {
                htmlBuilder.Append("<tr align='left' valign='top'>");

                foreach (DataColumn targetColumn in targetTable.Columns)
                {
                    htmlBuilder.Append("<td align='left' valign='top'>");
                    htmlBuilder.Append(myRow[targetColumn.ColumnName].ToString());
                    htmlBuilder.Append("</td>");
                }

                htmlBuilder.Append("</tr>");
            }

            //Create Bottom Portion of HTML Document
            htmlBuilder.Append("</table>");

            return htmlBuilder.ToString();

        }

        private string ConvertDataTableToHtml(DataSet dsInput)
        {
            string htmlString = "";

            if (dsInput == null)
            {
                throw new System.ArgumentNullException("dsInput");
            }

            StringBuilder htmlBuilder = new StringBuilder();

            //Create Top Portion of HTML Document
            htmlBuilder.Append("<html>");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<title>");
            htmlBuilder.Append("Page-");
            htmlBuilder.Append(Guid.NewGuid().ToString());
            htmlBuilder.Append("</title>");
            htmlBuilder.Append("</head>");
            htmlBuilder.Append("<body>");

            foreach (DataTable dt in dsInput.Tables)
            {
                htmlBuilder.Append(ConvertDataTableToHtmlString(dt));
            }

            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");

            //Create String to be Returned
            htmlString = htmlBuilder.ToString();

            return htmlString;
        }

        public override void Export(DataSet dsInput, string filename)
        {
            // we get the xml headers first
            string excelTemplate = ConvertDataTableToHtml(dsInput);


            // now we write the file
            try
            {
                File.Delete(filename);
                StreamWriter sw = new StreamWriter(filename, false, Encoding.Unicode);

                sw.Write(excelTemplate);

                sw.Flush();
                sw.Close();

                sw.Dispose();
                sw = null;
                OpenAfterSave(openAfterSave, filename);
            }
            catch (Exception)
            {
            }
        }



        
    }
}
