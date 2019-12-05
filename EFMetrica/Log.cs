using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EFMetrica
{
    public class Log
    {
        public static void Salvar(string Texto)
        {
            string path = System.IO.Directory.GetCurrentDirectory();

            string dir = path + "/log";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }


            string FileName = System.IO.Path.Combine(dir, DateTime.Now.ToString("yyy-MM-dd-hh-mm-ss"));
            //File.Create(FileName + ".log");

            FileName = FileName + Texto;

            if (!File.Exists(FileName))
                File.Create(FileName)
                    .Close();

            File.AppendAllText(FileName, "Executado às (" + DateTime.Now.ToShortDateString() + ")\r\n");
        }
    }
}
