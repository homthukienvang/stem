using System.Text.RegularExpressions;

namespace Extensions
{
    public static class StringHelper
    {
        /// <summary>
        /// The vietnamese signs
        /// </summary>
        private static readonly string[] VietnameseSigns =
        {
            "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        /// <summary>
        /// Converts to unsigned.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string ConvertToUnsigned(string values)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    values = values.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return values;
        }
        public static string FormatTitle(string orgTitle)
        {
            if (string.IsNullOrEmpty(orgTitle)) return "";
            string title = orgTitle.Trim();
            title = ConvertToUnsigned(title);
            title = title.Trim();
            title = title.Replace(",", "-");
            title = title.Replace("'", "-");
            title = title.Replace("\"", "-");
            title = Regex.Replace(title, @"[^A-Za-z0-9_\.~,]+", " ");
            return title + " | " + orgTitle;
        }
    }
}