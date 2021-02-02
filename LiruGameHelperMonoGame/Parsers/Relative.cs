using System;

namespace LiruGameHelperMonoGame.Parsers
{
    public static class Relative
    {
        public static float Parse(string input, out bool relative)
        {
            // Try to parse the relative, with exceptions being thrown.
            tryParse(input, out float value, out relative, true);

            // Return the relative.
            return value;
        }

        public static bool TryParse(string input, out float value, out bool relative) => tryParse(input, out value, out relative, false);

        private static bool tryParse(string input, out float value, out bool relative, bool throwException = false)
        {
            // If the input is empty or null, throw an exception or set the output to (false, 0) and return false.
            if (string.IsNullOrWhiteSpace(input))
            {
                if (throwException) throw new ArgumentNullException(nameof(input), "Given string cannot be null, empty, or whitespace.");
                else { relative = false; value = 0.0f; return false; }
            }
            // Trim any empty space from the beginning and end of the string.
            input = input.Trim();

            // If the input ends with a percentage sign, treat it as relative.
            relative = input.EndsWith("%");

            // If the input is relative, remove the percentage sign from the string.
            if (relative) input = input.TrimEnd('%');

            // Parse the string as a float. If it does not parse, throw an exception or set the output to (false, 0) and return false.
            if (!float.TryParse(input, out value))
            {
                if (throwException) throw new FormatException("Relative value must be a regular float value, with a '%' sign if desired.");
                else { relative = false; value = 0.0f; return false; }
            }

            // If the input is relative, divide it by 100 to get the float value.
            if (relative) value /= 100;

            // Return true as the parse operation was successful.
            return true;
        }
    }
}
