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
    [Activity(Label = "PerfilCliente")]
    public class PerfilCliente : Activity
    {
        Conexao c = new Conexao();
        string idCliente;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            c.AbrirCon();
            idCliente = Intent.GetStringExtra("idCliente");
                        
        }

        private void dadosCliente()
        {
            string sql;
            try
            {
                MySqlCommand cmd;
                MySqlDataReader lerCliente;
                sql = "SELECT idcliente,nome,email,cpf,telefone,complemento FROM cliente WHERE idcliente = @id";
                cmd = new MySqlCommand(sql,c.conn);
                cmd.Parameters.AddWithValue("@id",idCliente);
                lerCliente = cmd.ExecuteReader();

                if ( lerCliente.HasRows ) {
                    while ( lerCliente.Read() ) {

                    }
                }

            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context,"erro ao carregar os dados:"+e,ToastLength.Long).Show();
            }
        }
    }
}