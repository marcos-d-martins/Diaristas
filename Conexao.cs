using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace diaria
{
    class Conexao
    {
        public string conec = "server=85.10.205.173;port=3306;database=diarista;uid=desenvoldiarista; pwd=sucesso999;charset=utf8";
        public MySqlConnection conn = null;

        public void AbrirCon()
        {
            try
            {
                // variável declarada com valor nulo na linha 18. Será criado um objeto dessa variável. Objeto do tipo MySQLConnection
                conn = new MySqlConnection(conec);
                conn.Open();
                //Toast.MakeText(Application.Context, "Conectado:", ToastLength.Long).Show();
                //Console.WriteLine("Conectou");
            }
            catch (Exception ee)
            {
                Toast.MakeText(Application.Context, "erro na conexão:" + ee, ToastLength.Long).Show();
                //Console.WriteLine(ee.ToString());
            }
        }

        public void FecharCon()
        {
            try
            {
                conn = new MySqlConnection(conec);
                conn.Close();
            }
            catch (Exception ee)
            {
                Toast.MakeText(Application.Context, "erro ao fechar a conexão:" + ee, ToastLength.Long).Show();
            }
        }
    }
}