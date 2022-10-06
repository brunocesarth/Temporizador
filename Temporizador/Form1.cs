using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Collections.Specialized;

namespace Temporizador
{
    public partial class Temporizador : Form
    {
        public static int tempo = Convert.ToInt32(ConfigurationManager.AppSettings["Tempo"]);

        public Temporizador()
        {
            InitializeComponent();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = tempo;
            progressBar1.Step = 1;
        }

        private void Temporizador_Load(object sender, EventArgs e)
        {
            label1.Text = "00:" + tempo;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tempo--;
            if (tempo < 10)
            {
                label1.Text = "00:0" + tempo;
            }
            else
            {
                label1.Text = "00:" + tempo;
            }

            if (tempo == 0)
            {
                timer1.Enabled = false;
                label1.Text = "00:00";
                AbrirProgramas();
            }
            progressBar1.PerformStep();
            ProcessBarUpdate();
        }

        public void AbrirProgramas()
        {
            var postos = ConfigurationManager.GetSection("Linha/Postos") as NameValueCollection;

            foreach (var key in postos.AllKeys)
            {
                string montagem = postos[key];
                Process.Start(montagem);
            }

            Application.Exit();
        }

        public void ProcessBarUpdate()
        {
            int percent = (int)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100);
            progressBar1.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular),
            Brushes.Black, new PointF(progressBar1.Width / 2 - 10, progressBar1.Height / 2 - 7));
            Thread.Sleep(999);
            progressBar1.Refresh();
        }
    }
}
