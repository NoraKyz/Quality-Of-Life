using System;
using System.Globalization;
using System.Text;

namespace Quality.Core.Extensions
{
    public static class StringExtensions
    {
        private static StringBuilder s_stringBuilder = new ();

        public static string ToTitleCase(this string input)
        {
            s_stringBuilder.Clear();
            
            for (var index = 0; index < input.Length; ++index)
            {
                var ch = input[index];
                
                if (ch == '_' && index + 1 < input.Length)
                {
                    var upper = input[index + 1];
                    
                    if (char.IsLower(upper))
                    {
                        upper = char.ToUpper(upper, CultureInfo.InvariantCulture);
                    }
                        
                    s_stringBuilder.Append(upper);
                    ++index;
                }
                else
                {
                    s_stringBuilder.Append(ch);
                }
            }

            return s_stringBuilder.ToString();
        }

        public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
        {
            return source.IndexOf(toCheck, comparisonType) >= 0;
        }

        public static string SplitPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            s_stringBuilder.Clear();
            
            s_stringBuilder.Append(char.IsLetter(input[0]) ? char.ToUpper(input[0]) : input[0]);

            for (var index = 1; index < input.Length; ++index)
            {
                var c = input[index];
                
                if (char.IsUpper(c) && !char.IsUpper(input[index - 1]))
                {
                    s_stringBuilder.Append(' ');
                }
                
                s_stringBuilder.Append(c);
            }

            return s_stringBuilder.ToString();
        }

        public static bool IsNullOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (var index = 0; index < str.Length; ++index)
                {
                    if (!char.IsWhiteSpace(str[index]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
