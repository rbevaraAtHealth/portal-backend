using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

namespace CodeMatcher.Api.V2.Common
{
    public static class Helper
    {
        public static string ToCSV(this DataTable dataTable)
        {
            StringBuilder strb = new StringBuilder();

            //column headers
            strb.AppendLine(string.Join(",", dataTable.Columns.Cast<DataColumn>()
                .Select(s => "\"" + s.ColumnName + "\"")));

            //rows
            dataTable.AsEnumerable().Select(s => strb.AppendLine(
                string.Join(",", s.ItemArray.Select(
                    i => "\"" + i.ToString() + "\"")))).ToList();

            return strb.ToString();
        }
    }
}
