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
    [Activity(Label = "IndexAdm")]
    public class IndexAdm : Activity
    {
        Conexao c = new Conexao();
        Button bVerRelatorio;
        TextView qtdTotalDiarias, qtdDiaristas, qtdClientes;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.indexAdm);
            bVerRelatorio = FindViewById<Button>(Resource.Id.btnRelatorio);
            qtdTotalDiarias = FindViewById<TextView>(Resource.Id.txtTotalDiarias);
            qtdDiaristas = FindViewById<TextView>(Resource.Id.txtTotalDiaristas);
            qtdClientes = FindViewById<TextView>(Resource.Id.txtTotalClientes);

            Relatorio();
        }

        private void Relatorio()
        {
            string consulta_DIARIAS;
            string consulta_qtd_diaristas;
            string consulta_qtd_clientes;
            try
            {
                c.AbrirCon();
                consulta_DIARIAS = "SELECT COUNT(idpreservico) FROM preservico";
                consulta_qtd_diaristas = "SELECT COUNT(iddiarista) FROM diarista";
                consulta_qtd_clientes = "SELECT COUNT(idcliente) FROM cliente";
                MySqlCommand comando;
                MySqlDataReader lerDiarias;
                MySqlDataReader lerQtdDiaristas;
                MySqlDataReader lerQtdClientes;

                comando = new MySqlCommand(consulta_DIARIAS,c.conn);
                comando = new MySqlCommand(consulta_qtd_diaristas,c.conn);
                comando = new MySqlCommand(consulta_qtd_clientes,c.conn);

                lerDiarias = comando.ExecuteReader();
                lerQtdDiaristas = comando.ExecuteReader();
                lerQtdClientes = comando.ExecuteReader();



            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}