using System;
using System.Globalization;

namespace LiruGameHelper.Parsers
{
    /// <summary> Holds static settings for parsers. </summary>
    public static class ParserSettings
    {
        private static IFormatProvider formatProvider = CultureInfo.CreateSpecificCulture("en-GB");

        /// <summary> Sets the format used for all parsers, defaulting to en-GB (decimal point is the <c>.</c> character). This cannot be set to null. </summary>
        public static IFormatProvider FormatProvider
        {
            get => formatProvider;
            set => formatProvider = value ?? formatProvider;
        }

        /// <summary> The character used as a separator when parsing values, defaulting to <c>,</c>. </summary>
        public static char Separator { get; set; } = ',';
    }
}
