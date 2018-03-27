using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace CSV2
{
    class Program
    {
        private static List<List<string>> GetAllLineFields(string fullPath)
        {
            List<List<string>> allLineFields = new List<List<string>>();
            var fileInfo = new System.IO.FileInfo(fullPath);

            using (var reader = new System.IO.StreamReader(fileInfo.FullName, Encoding.Default))
            {
                Char quotingCharacter = '\0'; // no quoting-character;
                Char escapeCharacter = quotingCharacter;
                Char delimiter = '|';
                using (var csv = new CsvReader(reader, true, delimiter, quotingCharacter, escapeCharacter, '\0', ValueTrimmingOptions.All))
                {
                    csv.DefaultParseErrorAction = ParseErrorAction.ThrowException;
                    //csv.ParseError += csv_ParseError;  // if you want to handle it somewhere else
                    csv.SkipEmptyLines = true;

                    while (csv.ReadNextRecord())
                    {
                        List<string> fields = new List<string>(csv.FieldCount);
                        for (int i = 0; i < csv.FieldCount; i++)
                        {
                            try
                            {
                                string field = csv[i];
                                fields.Add(field.Trim('"'));
                            }
                            catch (MalformedCsvException ex)
                            {
                                // log, should not be possible anymore
                                throw;
                            }
                        }
                        allLineFields.Add(fields);
                    }
                }
            }
            return allLineFields;
        }


        static void Main(string[] args)
        {
            List<List<string>> allLineFields = GetAllLineFields(@"C:\Users\Saamari\Documents\Visual Studio 2015\Projects\CSV2\CSV2\data.csv");
            foreach (List<string> lineFields in allLineFields)
                Console.WriteLine(string.Join(",", lineFields.Select(s => string.Format("{0}", s))));
            Console.ReadLine();


        }
    }
}