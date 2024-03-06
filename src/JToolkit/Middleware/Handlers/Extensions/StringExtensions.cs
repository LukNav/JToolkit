using System.Text;

namespace JToolkit.Middleware.Handlers.Extensions
{
    /// <summary>Extensions for string manipulation</summary>
    public static class StringExtension
    {
        /// <summary>
        /// Efficiently converts string with snake_case to string with camelCase.
        /// </summary>
        /// <param name="str">snake_case string</param>
        /// <returns>camelCase string</returns>
        public static string SnakeCaseToCamelCase(this string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag1 = false;
            bool flag2 = false;
            foreach (char c in str)
            {
                if (c == '_')
                {
                    flag1 = true;
                    flag2 = true;
                }
                else if (flag2)
                {
                    stringBuilder.Append(char.ToUpperInvariant(c));
                    flag2 = false;
                }
                else
                {
                    stringBuilder.Append(char.ToLowerInvariant(c));
                }
            }
            if (stringBuilder.Length > 0)
                stringBuilder[0] = char.ToLowerInvariant(stringBuilder[0]);
            return !flag1 ? str : stringBuilder.ToString();
        }

        /// <summary>
        /// Efficiently converts string with camelCase to string with snake_case.
        /// </summary>
        /// <param name="str">camelCase string</param>
        /// <returns>snake_case string</returns>
        public static string CamelCaseToSnakeCase(this string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in str)
            {
                if (c == char.ToUpperInvariant(c) && stringBuilder.Length > 0)
                    stringBuilder.Append('_');
                stringBuilder.Append(char.ToLowerInvariant(c));
            }
            return stringBuilder.ToString();
        }
    }
}