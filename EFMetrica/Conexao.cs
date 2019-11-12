using Npgsql;
using System;
using System.Data;

namespace EFMetrica
{
    public class Conexao
    {
        public static string strConnEng = "Host=172.21.25.22;Database=eng;Username=eng;Password=agesune1";
        public static string strConnSonar = "Host=172.23.18.44;Database=sonar;Username=report;Password=WDe49ToNelol";
        public static NpgsqlConnection ConnEng { get; set; }
        public static NpgsqlConnection ConnSonar { get; set; }
        public static void ConectaEng()
        {
            try
            {
                ConnEng = new NpgsqlConnection(strConnEng);
                ConnEng.Open();
                Console.WriteLine("Conectado na Base Eng");
            }
            catch (NpgsqlException ex)
            {
                string error = ex.Message;
            }
        }

        public static void ConectaSonar()
        {
            try
            {
                ConnSonar = new NpgsqlConnection(strConnSonar);
                ConnSonar.Open();
                Console.WriteLine("Conectado na Base Sonar");
            }
            catch (NpgsqlException ex)
            {
                string error = ex.Message;
            }
        }

        public static void DesconectaEng()
        {

            if (ConnEng.State == ConnectionState.Open)
            {
                ConnEng.Close();
            }
        }

        public static void DesconectaSonar()
        {
            if (ConnSonar.State == ConnectionState.Open)
            {
                ConnSonar.Close();
            }
        }
    }

}

