using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace diaria
{
    [Activity(Label = "Inicio")]
    public class Inicio : Activity
    {
        Conexao c = new Conexao();
        Button btnLogin, btnCadastro;
        ImageView img;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.inicio);
            btnLogin = FindViewById<Button>(Resource.Id.login);
            btnCadastro = FindViewById<Button>(Resource.Id.cadastro);
            img = FindViewById<ImageView>(Resource.Id.imageView1);
            //img.SetImageResource(Resource.Drawable.diaristaExemplo);
        }
        
        private void Click_telaLogin(object sender, AdapterView.ItemClickEventArgs e)
        {
            var telaLogin = new Intent(this, typeof(Login));
            StartActivity(telaLogin);
        }
        private void Click_telaCadastro(object sender, EventArgs ev)
        {
            var telaCadastro = new Intent(this, typeof(CadastroCliente) );
            StartActivity(telaCadastro);
        }
    }
}