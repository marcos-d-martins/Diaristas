using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace diaria
{
    [Activity(Label = "DIARISTAPRIME", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Conexao conexao = new Conexao();
        Button bLogin, btcli, btdiarista, bCad;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            
            bLogin = FindViewById<Button>(Resource.Id.btnChamaLogin);
            btcli = FindViewById<Button>(Resource.Id.bcli);
            btdiarista = FindViewById<Button>(Resource.Id.bdia);
            bCad = FindViewById<Button>(Resource.Id.btnCadastro);

            conexao.AbrirCon();

            escondeBotoes();

            bLogin.Click += BLogin_Click;
            btcli.Click += BtCliente_Click;
            btdiarista.Click += BtDiarista_Click;
            bCad.Click += BCad_Click;

        }

        private void BtDiarista_Click(object sender, EventArgs e)
        {
            
            var telaCadastroDiarista = new Intent(this, typeof(CadastroDiarista));
            StartActivity(telaCadastroDiarista);
            
        }

        private void BtCliente_Click(object sender, EventArgs e)
        {
            var telaCadastroCliente = new Intent(this, typeof(CadastroCliente));
            StartActivity(telaCadastroCliente);
           
        }

        private void BLogin_Click(object sender, EventArgs e)
        {
            var telaLogin = new Intent(this, typeof(Login));
            StartActivity(telaLogin);
           
        }

        private void BCad_Click(object sender, EventArgs e)
        {
            btcli.Enabled = true;
            btdiarista.Enabled = true;
          
        }
        private void escondeBotoes()
        {
            btcli.Enabled = false;
            btdiarista.Enabled = false;
        }

  
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}