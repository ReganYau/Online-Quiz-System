using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineQuizSystem
{
    // Static class used to parse and write CSV values.
    public static class CsvUtil
    {
        // Method used to parse a single CSV line into a list.
        public static List<string> ParseLine(string line)
        {
            var fields = new List<string>(); // Declaration and initialisation of list used to store fields.
            if (line == null) return fields; // Returning empty list if line is null.

            var cur = new StringBuilder(); // Declaration and initialisation of StringBuilder used for current field.
            bool inQuotes = false; // Declaration and initialisation of flag used to track quoted fields.

            // Looping through each character in the line.
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i]; // Getting current character.

                // Handling quote characters.
                if (c == '"')
                {
                    // Handling escaped quotes inside a quoted field.
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        cur.Append('"'); // Appending a single quote.
                        i++; // Skipping the second quote.
                    }
                    else
                    {
                        inQuotes = !inQuotes; // Toggling quote state.
                    }
                }
                // Handling commas only when not inside quotes.
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(cur.ToString()); // Adding completed field to list.
                    cur.Clear(); // Clearing builder for next field.
                }
                else
                {
                    cur.Append(c); // Appending character to current field.
                }
            }

            fields.Add(cur.ToString()); // Adding final field.
            return fields; // Returning parsed field list.
        }

        // Method used to escape a CSV field if it contains commas/quotes/newlines.
        public static string Escape(string value)
        {
            value ??= ""; // Defaulting null to empty string.

            // Checking if value needs quoting.
            bool mustQuote = value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r");
            if (!mustQuote) return value; // Returning original value if quoting not required.

            string v = value.Replace("\"", "\"\""); // Escaping quotes by doubling them.
            return $"\"{v}\""; // Returning quoted field.
        }

        // Method used to join multiple fields into a single CSV line.
        public static string Join(params string[] fields)
        {
            // Looping through each field and escaping it if needed.
            for (int i = 0; i < fields.Length; i++)
                fields[i] = Escape(fields[i]);

            return string.Join(",", fields); // Returning CSV line.
        }
    }
}
