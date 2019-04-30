using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SourceCodeStats
{
    public class GetStats
    {
        public static Dictionary<string, int> dictUsing = new Dictionary<string, int>();
        public static Dictionary<string, int> dictNamespaces = new Dictionary<string, int>();
        public static Dictionary<string, int> dictClasses = new Dictionary<string, int>();
        public static Dictionary<string, int> dictMethods = new Dictionary<string, int>();
        public static Dictionary<string, int> dictWords = new Dictionary<string, int>();
        public static Dictionary<string, int> dictComments = new Dictionary<string, int>();

        public static void CountUsing(string filename)
        {
            var lines = File.ReadAllLines(@filename);

            try
            {
                foreach (var line in lines)
                {
                    if (line.Contains("using"))
                    {
                        var cleanLine = line.Replace(";", "");

                        var items = cleanLine.Split(new char[] { ' ' }).ToList();

                        var nKey = 1;

                        if (items.Count > 1 )
                        {
                            var bResult = dictUsing.TryGetValue(items[1], out nKey);

                            if (bResult)
                            {
                                dictUsing[items[1]] = nKey + 1;
                            }
                            else
                            {
                                dictUsing.Add(items[1], 1);
                            }
                        }
                    }

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("ex: {0}", ex.InnerException);
                throw ex;
            }

        }

        public static void CountNamespaces(string filename)
        {
            var lines = File.ReadAllLines(@filename);

            foreach (var line in lines)
            {
                if (line.Contains("namespace"))
                {
                    var cleanLine = line.Replace(";", "");

                    string hit = cleanLine.Split(new string[] { "namespace" }, StringSplitOptions.None).Last();

                    var nKey = 1;

                    var bResult = dictNamespaces.TryGetValue(hit, out nKey);

                    if (bResult)
                    {
                        dictNamespaces[hit] = nKey + 1;
                    }
                    else
                    {
                        dictNamespaces.Add(hit, 1);
                    }
                }

            }
            
        }

        public static void CountClasses(string filename)
        {
            var lines = File.ReadAllLines(@filename);

            //var dictUsing = new Dictionary<string, int>();

            //Console.WriteLine(filename + ": Classes");

            foreach (var line in lines)
            {
                if (line.Contains("class "))
                {
                    var cleanLine = line.Replace(";", "");

                    string hit = cleanLine.Split(new string[] { "class" }, StringSplitOptions.None).Last();

                    var nKey = 1;

                    var bResult = dictClasses.TryGetValue(hit, out nKey);

                    if (bResult)
                    {
                            dictClasses[hit] = nKey + 1;
                    }
                    else
                    {
                        dictClasses.Add(hit, 1);
                    }    

                }

            }

        }

        public static void CountComments(string filename)
        {
            var lines = File.ReadAllLines(@filename);

            foreach (var line in lines)
            {
                if (line.Contains("// ") || line.Contains("///"))
                {
                    var cleanLine = line.Replace(";", "");

                    string hit = (cleanLine.Split(new string[] { "//", "///" }, StringSplitOptions.None).Last()).Replace("/","");

                    var nKey = 1;

                    var bResult = dictComments.TryGetValue(hit, out nKey);

                    if (bResult)
                    {
                        dictComments[hit] = nKey + 1;
                    }
                    else
                    {
                        dictComments.Add(hit, 1);
                    }

                }

            }

            // Console.WriteLine("Count {0}", dictUsing.Count);
        }

        public static void CountWords(string filename)
        {
            var lines = File.ReadAllLines(@filename);

            foreach (var line in lines)
            {
                var cleanLine = line.Replace(";", "");

                var punctuation = cleanLine.Where(Char.IsPunctuation).Distinct().ToArray();
                var words = cleanLine.Split().Select(x => x.Trim(punctuation));

                foreach (var hit in words)
                {
                    if (!string.IsNullOrEmpty(hit))
                    {
                        var nKey = 1;

                        var bResult = dictWords.TryGetValue(hit.ToString(), out nKey);

                        if (bResult)
                        {
                            //Console.WriteLine("Found:   {0}; {1}", hit, nKey);
                            dictWords[hit.ToString()] = nKey + 1;
                        }
                        else
                        {
                            dictWords.Add(hit.ToString(), 1);
                        }
                    }
                }

            }

        }


        public static void CountMethods(string filename)
        {
            var lines = File.ReadAllLines(@filename);

            string[] str = { "void","public", "private", "protected", "static", "final", "native", "synchronized", "abstract", "transient" };

            foreach (var line in lines)
            {
                var cleanLine = line.Replace(";", "");

                var check = (from strF in str
                            where cleanLine.Contains(strF)
                            select strF).ToList();

                if (check.Count > 0)
                {
                    string hitKey = cleanLine.Split(str, StringSplitOptions.None).First();
                    string hit = cleanLine.Split(str , StringSplitOptions.None).Last();

                    var nKey = 1;

                    var bResult = dictMethods.TryGetValue(hit, out nKey);

                    if (bResult)
                    {
                        //Console.WriteLine("Found:   {0}; {1}", hit, nKey);
                        dictMethods[hit] = nKey + 1;
                    }
                    else
                    {
                        dictMethods.Add(hit, 1);
                    }

                }

            }

            // Console.WriteLine("Count {0}", dictUsing.Count);
        }

        public static void ToCSV(Dictionary<string, int> dict, string outPath, string outFile)
        {
            var outFullPath = @Path.Combine(outPath, outFile);

            string csv = "Item; Count\r\n";
            csv += String.Join(
                Environment.NewLine,
                dict.OrderByDescending(x => x.Value).Distinct().Select(d => d.Key + ";" + d.Value + ";"));

            System.IO.File.WriteAllText(outFullPath, csv);
        }

    }
}
