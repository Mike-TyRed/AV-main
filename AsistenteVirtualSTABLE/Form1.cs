using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AsistenteVirtualSTABLE
{
    public partial class Form1 : Form
    {
        public string Si = @"C:\Users\PRIDE RACOON\Documents\GitHub\AV\AsistenteVirtualSTABLE\Resources\Si.mp4";
        public string Aso = @"C:\Users\PRIDE RACOON\Documents\GitHub\AV\AsistenteVirtualSTABLE\Resources\A sus ordenes.mp4";
        public string Eqpa = @"C:\Users\PRIDE RACOON\Documents\GitHub\AV\AsistenteVirtualSTABLE\Resources\En que puedo ayudarle.mp4";
        public string Ei = @"C:\Users\PRIDE RACOON\Documents\GitHub\AV\AsistenteVirtualSTABLE\Resources\Esperando instrucciones.mp4";
        public string Inmovil = @"C:\Users\PRIDE RACOON\Documents\GitHub\AV\AsistenteVirtualSTABLE\Resources\Inmovil.mp4";
        public string Diga = @"C:\Users\PRIDE RACOON\Documents\GitHub\AV\AsistenteVirtualSTABLE\Resources\Diga.mp4";
        public int c;
        public Form1()
        {
            InitializeComponent();
        }
        public void GetNum(int i)
        {
            timer1.Start();
            c = i;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (c)
            {
                case 0:
                    //timer1.Interval = 1000;
                    interaccion();
                    break;
                case 1:
                    axWindowsMediaPlayer1.URL = Si;
                    timer1.Interval = 1000;
                    c = 0;
                    break;
                case 2:
                    axWindowsMediaPlayer1.URL = Aso;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    timer1.Interval = 1000;
                    c = 0;
                    break;
                case 3:
                    axWindowsMediaPlayer1.URL = Eqpa;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    timer1.Interval = 1000;
                    c = 0;
                    break;
                case 4:
                    axWindowsMediaPlayer1.URL = Diga;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    timer1.Interval = 1000;
                    c = 0;
                    break;
                case 5:
                    axWindowsMediaPlayer1.URL = Ei;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    timer1.Interval = 2000;
                    c = 0;
                    break;
            }
        }
        public void interaccion()
        {
            axWindowsMediaPlayer1.URL = Inmovil;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            timer1.Stop();
        }
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
        }
    }
}
