using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.SE.Omapi;
using Android.Views;
using Android.Widget;
using Java.Sql;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace diaria
{
    [Activity(Label = "GravaVaga")]
    public class GravaVaga : Activity
    {
        Conexao c;
        ImageView img;
        EditText DescricaoComodos, Horario;
        Android.Widget.CalendarView dataServico;
        Button bLancaVaga;
        TextView tData;

        int anoClicado,mesClicado,diaClicado;

        string idcliente;
        //Spinner tam;
        //List<string> listidcom = new List<string>();
        //List<string> listdesccom = new List<string>();
        //RadioGroup rbServicos;
        //RadioButton rbCadaServico;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.gravaVaga);

            img = FindViewById<ImageView>(Resource.Id.imageView1);
            img.SetImageResource(Resource.Drawable.topo);

            idcliente = Intent.GetStringExtra("idCliente");
            //rbServicos = FindViewById<RadioGroup>(Resource.Id.servicos);
            //rbCadaServico = FindViewById<RadioButton>(Resource.Id.radioButton);
            c = new Conexao();
            DescricaoComodos = FindViewById<EditText>(Resource.Id.edDescComodos);
            dataServico = FindViewById<Android.Widget.CalendarView>(Resource.Id.cDia);
            tData = FindViewById<TextView>(Resource.Id.txtData);
            Horario = FindViewById<EditText>(Resource.Id.edHorario);
            bLancaVaga = FindViewById<Button>(Resource.Id.btLancaVaga);
            //tam = FindViewById<Spinner>(Resource.Id.comodos);

            dataServico.DateChange += MudarData;
            bLancaVaga.Click +=  GerarVaga;
        }

        private void MudarData(object sender, CalendarView.DateChangeEventArgs e)
        {
            int dia = e.DayOfMonth;
            int mes = e.Month + 1;
            int ano = e.Year;
            tData.Text = "Data desejada: " + dia + " / " + mes + " / " + ano;

            anoClicado = ano;
            mesClicado = mes;
            diaClicado = dia;
        }

        private void GerarVaga(object sender, EventArgs e)
        {
            string sql;
            c.AbrirCon();
            try
            {
                string dataFormatada = anoClicado +"/"+ mesClicado +"/"+ diaClicado;
                /*
                DateTime data = new DateTime(dataServico.Date);
                string dataFormatada = data.ToString("YYYY mm dd");
                */

                Toast.MakeText(Application.Context,dataFormatada,ToastLength.Short).Show();

                MySqlCommand cmd;
                sql = "INSERT INTO preservico VALUES(NULL,@id_cl,1,@descricao,@data,1,@hora)";
                cmd = new MySqlCommand(sql, c.conn);
                cmd.Parameters.AddWithValue("@id_cl",idcliente);
                cmd.Parameters.AddWithValue("@descricao",DescricaoComodos.Text);
                //cmd.Parameters.AddWithValue("@data",dataFormatada);
                cmd.Parameters.AddWithValue("@hora",Horario.Text);

                if ( cmd.ExecuteNonQuery() > 0 )
                {
                    Toast.MakeText(Application.Context, "vaga gerada!",ToastLength.Short).Show();
                    var redirecionaVerVagas = new Intent(this, typeof(VerVagasDoCliente) );
                    redirecionaVerVagas.PutExtra("idcliente",idcliente);
                    StartActivity(redirecionaVerVagas);
                }
                c.FecharCon();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, "Erro ao gerar a vaga:" + ex, ToastLength.Long).Show();


                c.AbrirCon();

                MySqlCommand comando;
                string insere;
                insere = "INSERT INTO registro_de_erros VALUES(NULL,@erro,'GravaVaga-gravar_a_vaga')";
                comando = new MySqlCommand(insere, c.conn);
                comando.Parameters.AddWithValue("@erro", ex);
                comando.ExecuteNonQuery();
            }
        }
    }
}
