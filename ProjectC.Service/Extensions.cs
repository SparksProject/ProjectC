using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace ProjectC.Service
{
    public static class Extensions
    {
        private const string UserIdColumnName = "UserId";

        public static List<dynamic> ToDynamicList(this DataTable dataTable, int userId)
        {
            var target = new List<dynamic>();

            if (dataTable is null)
            {
                return target;
            }

            var userIdExists = dataTable.Columns.Contains(UserIdColumnName);

            if (!userIdExists)
            {
                throw new ArgumentException($"Raporda {UserIdColumnName} alanı eksik!");
            }

            foreach (DataRow row in dataTable.Rows)
            {
                dynamic dyn = new ExpandoObject();

                var i = 0;

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName.Equals(UserIdColumnName)) // raporlarda bu kolon görünmesin!
                    {
                        continue;
                    }

                    var dic = (IDictionary<string, dynamic>)dyn;

                    var newColumnName = $"prop{i++}";

                    dic[newColumnName] = new ExpandoObject();

                    var columnValues = (IDictionary<string, dynamic>)dic[newColumnName];

                    columnValues.Add("DataType", column.DataType.Name);
                    columnValues.Add("Value", row[column]);
                    columnValues.Add("Caption", column.ColumnName);
                }

                if (!((int)row[UserIdColumnName]).Equals(userId)) // herkes yalnızca kendi UserId'sine ait veriyi görsün.
                {
                    continue;
                }

                target.Add(dyn);
            }

            return target;
        }

        public static List<dynamic> ToDynamicList(this DataTable dataTable)
        {
            var target = new List<dynamic>();

            if (dataTable is null)
            {
                return target;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                dynamic dyn = new ExpandoObject();

                foreach (DataColumn column in dataTable.Columns)
                {
                    var dic = (IDictionary<string, dynamic>)dyn;

                    var newColumnName = column.ColumnName;

                    dic[newColumnName] = new ExpandoObject();

                    var columnValues = (IDictionary<string, dynamic>)dic[newColumnName];

                    columnValues.Add("DataType", column.DataType.Name);
                    columnValues.Add("Value", row[column]);
                }

                target.Add(dyn);
            }

            return target;
        }

        public static string TurkishCharacterToEnglish(string text)
        {
            char[] turkishChars = { 'ı', 'ğ', 'İ', 'Ğ', 'ç', 'Ç', 'ş', 'Ş', 'ö', 'Ö', 'ü', 'Ü' };
            char[] englishChars = { 'i', 'g', 'I', 'G', 'c', 'C', 's', 'S', 'o', 'O', 'u', 'U' };

            // Match chars
            for (int i = 0; i < turkishChars.Length; i++)
            {
                text = text.Replace(turkishChars[i], englishChars[i]);
            }

            return text;
        }
    }
}