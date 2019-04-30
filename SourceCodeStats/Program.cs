using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SourceCodeStats
{
    class Program
    {
        public static List<string> FileList = new List<string>();
        public static List<string> DirectoryList = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("In Path: " + @args[0]);

            //var files = Directory.GetFiles(@args[0], "*.cs");

            BuildDirectoryList(@args[0], "*.*");

            foreach (var file in FileList)
            {
                var fo = new FileInfo(file);

                if (fo.Extension == ".cs")
                {
                    Console.WriteLine("File: {0}", file);

                    GetStats.CountUsing(file);
                    GetStats.CountNamespaces(file);
                    GetStats.CountClasses(file);
                    GetStats.CountMethods(file);
                    GetStats.CountWords(file);
                    GetStats.CountComments(file);
                }
            }


            GetStats.ToCSV(GetStats.dictNamespaces, @args[1], "CountNamespaces.csv");
            GetStats.ToCSV(GetStats.dictClasses, @args[1], "CountClasses.csv");
            GetStats.ToCSV(GetStats.dictUsing, @args[1], "CountUsing.csv");
            GetStats.ToCSV(GetStats.dictMethods, @args[1], "CountMethods.csv");
            GetStats.ToCSV(GetStats.dictWords, @args[1], "CountWords.csv");
            GetStats.ToCSV(GetStats.dictComments, @args[1], "CountComments.csv");

        }

        public static void BuildDirectoryList(string dir, string _pattern)
        {
            try
            {

                var files = Directory.GetFiles(dir, _pattern);

                lock (FileList)
                {
                    FileList.AddRange(files);
                }

                // get the file attributes for file or directory
                var directories = Directory.GetDirectories(dir, _pattern);

                DirectoryList.AddRange(directories);

                    foreach (string directory in directories)
                    {
                        BuildDirectoryList(directory, _pattern);
                    }


            }
            catch (UnauthorizedAccessException UAEx)
            {
                throw UAEx;
                //Console.WriteLine(UAEx.Message);

                // Console.WriteLine(MetaDiver_Core.logging.HandleError(UAEx, "directorylist UnauthorizedAccessException", "BuildDirectoryList: " + dir));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




    }
}
