using System;
using System.IO;

namespace ProjectMetricAPI
{
    public class Log
    {
        public static void Salvar()
        {
            string path = System.IO.Directory.GetCurrentDirectory();


            string FileName = System.IO.Path.Combine(path, DateTime.Now.ToString("yyy-MM-dd-hh-mm-ss"));
            //File.Create(FileName + ".log");

            if (!File.Exists(FileName))
                File.Create(FileName)
                    .Close();

            File.AppendAllText(FileName, "Executado às (" + DateTime.Now.ToShortDateString() + ")\r\n");
        }
    }
}
