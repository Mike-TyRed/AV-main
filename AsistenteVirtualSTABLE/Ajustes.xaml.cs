using BibliotecaAsistente;
using System;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AsistenteVirtualSTABLE
{
    public partial class Ajustes : Window
    {
        #region Variables
        private SpeechSynthesizer sara = new SpeechSynthesizer();
        private Asistente ia = new Asistente();
        #endregion
        #region Main
        public Ajustes()
        {
            InitializeComponent();
            CargarDatos();
        }
        #endregion
        #region Metodos declarados
        private void OcultarControles()
        {
            tbx_NombreUsuario.Visibility = Visibility.Hidden;
            tbx_NombreAsistente.Visibility = Visibility.Hidden;
            cbx_Voces.Visibility = Visibility.Hidden;
            sld_Volumen.Visibility = Visibility.Hidden;
            sld_confidencia.Visibility = Visibility.Hidden;
            Sld_Opacidad.Visibility = Visibility.Hidden;
            lblinfovolumen.Visibility = Visibility.Hidden;
            lblnombreAI.Visibility = Visibility.Hidden;
            cbxOffMicro.Visibility = Visibility.Hidden;
            checkInicio.Visibility = Visibility.Hidden;
        }
        public void CargarDatos()
        {
            CargarVoces_tiempoespera();
            lblnombreAI.Content = "AV" + Properties.Settings.Default.NombreAsistente;
            lbl_info.Content = "   Por favor, verifique los ajustes \n     para una mejor interacción\n               con el asistente";
            tbx_NombreUsuario.Text = Properties.Settings.Default.NombreUsuario;
            tbx_NombreAsistente.Text = Properties.Settings.Default.NombreAsistente;
            cbx_Voces.Text = Properties.Settings.Default.VozDefault;
            sld_Volumen.Value = Properties.Settings.Default.VolumenIA;
            sld_confidencia.Value = Properties.Settings.Default.ConfidenciaUsuario;
            Sld_Opacidad.Value = Properties.Settings.Default.OpacidadVentana;
            cbxOffMicro.Text = Properties.Settings.Default.OffMicro.ToString();
            checkInicio.IsChecked = Properties.Settings.Default.InicioWindows;
           
        }

        private void CargarVoces_tiempoespera()
        {
            foreach (InstalledVoice voces in sara.GetInstalledVoices())
            {
                cbx_Voces.Items.Add(voces.VoiceInfo.Name);
            }
            cbxOffMicro.Items.Add("5");
            cbxOffMicro.Items.Add("10");
            cbxOffMicro.Items.Add("15");
            cbxOffMicro.Items.Add("30");
            cbxOffMicro.Items.Add("60");
        }
       

        private void Guardardatos()
        {
            Properties.Settings.Default.NombreUsuario = tbx_NombreUsuario.Text;
            Properties.Settings.Default.NombreAsistente = tbx_NombreAsistente.Text;
            Properties.Settings.Default.VozDefault = cbx_Voces.Text;
            Properties.Settings.Default.VolumenIA = (int)sld_Volumen.Value;
            Properties.Settings.Default.ConfidenciaUsuario = Math.Round(sld_confidencia.Value, 1);
            Properties.Settings.Default.OpacidadVentana = Math.Round(Sld_Opacidad.Value);
            Properties.Settings.Default.OffMicro = Convert.ToInt32(cbxOffMicro.Text);
            Properties.Settings.Default.InicioWindows = checkInicio.IsChecked.HasValue;
            Properties.Settings.Default.Save();
        }

        #endregion
        #region Eventos de Control
        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Guardardatos();
            System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet = System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet;
            Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void BtnNombreUsuario_Click(object sender, RoutedEventArgs e)
        {
                lbl_info.Content = "   Ingrese su nombre con el que \n" + "          " + "el asistente le hablará";
            
            OcultarControles();
            tbx_NombreUsuario.Visibility = Visibility.Visible;
        }
        private void BtnNombreAsistente_Click(object sender, RoutedEventArgs e)
        {
                lbl_info.Content = "  Ingrese el nombre del asistente";
            
            OcultarControles();
            tbx_NombreAsistente.Visibility = Visibility.Visible;
        }
        private void BtnVoces_Click(object sender, RoutedEventArgs e)
        {
                lbl_info.Content = "    Seleccione una voz instalada";
            
            OcultarControles();
            cbx_Voces.Visibility = Visibility.Visible;
        }
        private void BtnVolumen_Click(object sender, RoutedEventArgs e)
        {
            lbl_info.Content = "   Nivel de volumen del asistente";          
            OcultarControles();
            sld_Volumen.Visibility = Visibility.Visible;
            lblinfovolumen.Visibility = Visibility.Visible;
        }
        private void BtnConfidencia_Click(object sender, RoutedEventArgs e)
        {

            lbl_info.Content = "     Nivel de afirmación al hablar";

            OcultarControles();
            sld_confidencia.Visibility = Visibility.Visible;


        }
        private void BtnOpacidad_Click(object sender, RoutedEventArgs e)
        {

            lbl_info.Content = "       Opacidad de las ventanas";

            OcultarControles();
            Sld_Opacidad.Visibility = Visibility.Visible;
        }
        private void BtnOffMicro_Click(object sender, RoutedEventArgs e)
        {

            lbl_info.Content = "Tiempo de espera para desactivar \n" +
               "                  el micrófono";

            OcultarControles();
            cbxOffMicro.Visibility = Visibility.Visible;
        }
        private void BtnInicioWindows_Click(object sender, RoutedEventArgs e)
        {

            lbl_info.Content = "     Marque la casilla para iniciar\n        con su sistema operativo";
            OcultarControles();
            checkInicio.Visibility = Visibility.Visible;
        }
        private void Sld_Opacidad_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Opacity = Sld_Opacidad.Value;
        }
        private void Sld_Volumen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblinfovolumen.Content = Math.Round(sld_Volumen.Value);
        }
        private void Cbx_Voces_DropDownClosed(object sender, EventArgs e)
        {
            sara.SelectVoice(cbx_Voces.Text);
            sara.SpeakAsync("Voz seleccionada.");
        }

        #endregion
    }
}
