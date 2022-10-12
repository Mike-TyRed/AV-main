#region Usings
using AsistenteVirtualSTABLE.Properties;
using BibliotecaAsistente;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endregion

namespace AsistenteVirtualSTABLE
{
    public partial class InterfazPrincipal : Window
    {
        #region Variables
        EditorCmd editorCmd;
        ControlSerial serial;
        Ajustes ajustes;
        ComandosSistema csis;
        Asistente ia = new Asistente();
        Gramaticas gramaticas = new Gramaticas();
        CargarGramaticas loadgramaticas = new CargarGramaticas();
        ConexionTelegram establecerConexion = new ConexionTelegram();
        Form1 Waifu;
        public int i = 0;
        #endregion
        #region Main
        public InterfazPrincipal()
        {
            InitializeComponent();
            CargarDatosIniciales();
            establecerConexion.Escucha();
            Interactuar(i);
        }
        #endregion
        #region Métodos
        Form1 q = new Form1();
        public void Interactuar(int i)
        {
            q.GetNum(i);
            q.Show();
        }
        void CargarDatosIniciales()
        {
            Btn_NomAsisstant.Content = Settings.Default.NombreAsistente;
            ia.habla_asistente.SelectVoice(Settings.Default.VozDefault);
            ia.habla_asistente.Volume = Settings.Default.VolumenIA;
            VistaOffStroke();
            DesactivarMicrófono();
            Top = Settings.Default.PosicionVentanaX;
            Left = Settings.Default.PosicionVentanaY;
            IdiomaEspañol();
            CargarDatosGramaticas();
            Process.GetCurrentProcess().MaxWorkingSet = Process.GetCurrentProcess().MinWorkingSet;
        }
        void CargarDatosGramaticas()
        {
            loadgramaticas.SeleccionarComandos();
            ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(loadgramaticas.ComandosInternos())); //todo esto tiene la información de los comandos internos
            ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(loadgramaticas.CargarGramáticasCalc())); //Carga las gramaticas de la calculadora
            ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(new Choices(loadgramaticas.CargarGramáticasBD()))); //Carga las gramaticas del usuario
            ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(new Choices(Settings.Default.NombreAsistente)));// Esto se eliminará en un futuro, pero carga el nombre del asistente
            ia.reconocer_voz_usuario.LoadGrammarAsync(loadgramaticas.CargarGramáticasWebs());//hasta aquí carga todas las gramáticas
            ia.reconocer_voz_usuario.RequestRecognizerUpdate();
            ia.reconocer_voz_usuario.AudioLevelUpdated += Reconocer_voz_usuario_AudioLevelUpdated;
            ia.reconocer_voz_usuario.SpeechRecognized += Reconocer_voz_usuario_SpeechRecognized;
            ia.habla_asistente.SpeakStarted += Habla_asistente_SpeakStarted;
            ia.habla_asistente.SpeakCompleted += Habla_asistente_SpeakCompleted;
            ia.reconocer_voz_usuario.SetInputToDefaultAudioDevice();
            ia.reconocer_voz_usuario.RecognizeAsync(RecognizeMode.Multiple);
            ia.tiempo_desactivar_microfono.Tick += Tiempo_desactivar_microfono_Tick;
            ia.liberarRAM.Tick += LiberarRAM_Tick;
            ia.liberarRAM.Start();
        }
        private void LiberarRAM_Tick(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().MaxWorkingSet = Process.GetCurrentProcess().MinWorkingSet;
        }
        void GuardarPosiciónVentana()
        {
            ia._PosiciónX = Top;
            ia._PosiciónY = Left;
            Settings.Default.PosicionVentanaX = ia._PosiciónX;
            Settings.Default.PosicionVentanaY = ia._PosiciónY;
            Settings.Default.Save();
        }
        void DesactivarMicrófono()
        {
            Settings.Default.OnOffMicro = false;
            Settings.Default.Save();
        }
        void VistaOnStroke()
        {
            rec_onoff.Stroke = System.Windows.Media.Brushes.Green;
        }
        void VistaOffStroke()
        {
            rec_onoff.Stroke = System.Windows.Media.Brushes.Red;
        }
        void Activo(string speech)
        {
            Random rnd = new Random();
            if (speech.Equals("Activar el micrófono"))
            {
                Settings.Default.OnOffMicro = true;
                Settings.Default.Save();
                VistaOnStroke();
            }
            else if (speech.Equals(Settings.Default.NombreAsistente))
            {
                int numran = rnd.Next(1, 5);
                ia.on_mic.Play();
                switch (numran)
                {
                    case 1:
                        i = 1;
                        Interactuar(i);
                        ia.habla_asistente.SpeakAsync("Si " + Properties.Settings.Default.NombreUsuario + ". . . .");
                        break;
                    case 2:
                        i = 2;
                        Interactuar(i);
                        ia.habla_asistente.SpeakAsync("A sus órdenes " + Properties.Settings.Default.NombreUsuario + ". . . .");
                        break;
                    case 3:
                        i = 3;
                        Interactuar(i);
                        ia.habla_asistente.SpeakAsync("En qué puedo ayudarle " + Properties.Settings.Default.NombreUsuario + ". . . .");
                        break;
                    case 4:
                        i = 4;
                        Interactuar(i);
                        ia.habla_asistente.SpeakAsync("Diga " + Properties.Settings.Default.NombreUsuario + ". . . .");
                        break;
                    case 5:
                        i = 5;
                        Interactuar(i);
                        ia.habla_asistente.SpeakAsync("Esperando instrucciones " + Properties.Settings.Default.NombreUsuario + ". . . .");
                        break;
                }
                Settings.Default.OnOffMicro = true;
                Settings.Default.Save();
                VistaOnStroke();
                lbl_pantalla.Content = "Escuchando instrucciones...";

                //Actualizar Gramaticas
                loadgramaticas.SeleccionarComandos();
                ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(loadgramaticas.ComandosInternos())); //todo esto tiene la información de los comandos internos
                ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(loadgramaticas.CargarGramáticasCalc())); //Carga las gramaticas de la calculadora
                ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(new Choices(loadgramaticas.CargarGramáticasBD()))); //Carga las gramaticas del usuario
                ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(new Choices(Settings.Default.NombreAsistente)));// Esto se eliminará en un futuro, pero carga el nombre del asistente
                ia.reconocer_voz_usuario.LoadGrammarAsync(loadgramaticas.CargarGramáticasWebs());//hasta aquí carga todas las gramáticas
                
            }
            else if (speech == "Desactivar el micrófono" || speech.Equals("Desactivar micrófono"))
            {
                DesactivarMicrófono();
                VistaOffStroke();
                lbl_pantalla.Content = "Para activar el micrófono di \"" + Settings.Default.NombreAsistente + "\"";
                ia.tiempo_desactivar_microfono.Stop();
                ia.tiempo = 0;
                ia.off_mic.Play();
            }

        }
        private void IdiomaEspañol()
        {
            Btn_NomAsisstant.ToolTip = "Ajustes";
            BtnAlarma.ToolTip = "Alarmas";
            BtnComando.ToolTip = "Comandos";
            BtnRecordatorio.ToolTip = "Matemáticas";
            BtnSerial_Port.ToolTip = "Puerto Serial";
            lbl_pantalla.Content = "Para activar el micrófono di \"" + Settings.Default.NombreAsistente + "\"";
        }

        #endregion
        #region Eventos Internos
        private void Reconocer_voz_usuario_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            pb_Audio.Value = e.AudioLevel;
        }
        private void Habla_asistente_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            ia.habilitarReconocimiento = false;
            VistaOffStroke();
            ia.tiempo = 0;
        }
        private void Habla_asistente_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            ia.habilitarReconocimiento = true;
            VistaOnStroke();
            lbl_pantalla.Content = "Escuchando instrucciones...";
            ia.tiempo = 0;
            ia.tiempo_desactivar_microfono.Start();
        }
        private void Tiempo_desactivar_microfono_Tick(object sender, EventArgs e)
        {
            ia.tiempo++;
            if (ia.tiempo.Equals(Settings.Default.OffMicro))
            {
                ia.off_mic.Play();
                DesactivarMicrófono();
                VistaOffStroke();
                ia.tiempo_desactivar_microfono.Stop();
                ia.tiempo = 0;
            }
        }
        private void Reconocer_voz_usuario_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            ia.recogResult = e.Result;
            ia.speech = e.Result.Text;

            Activo(ia.speech);

            if (Settings.Default.OnOffMicro == true)
            {
                if (e.Result.Confidence >= Settings.Default.ConfidenciaUsuario)
                {
                    if (ia.habilitarReconocimiento == true)
                    {
                        string ejecutarcmdBD = gramaticas.EjecutarComandosBD(ia.speech); // Esto es igual a e.Result.Text (ia.speech)
                        string ejecutarcmdInternos = gramaticas.SinonimoCmd(ia.speech);
                        string calculadora = gramaticas.Calc(ia.recogResult);
                        string buscador = gramaticas.Busc(ia.recogResult, ia.speech);
                        if (ejecutarcmdBD != string.Empty)
                        {
                            ia.habla_asistente.SpeakAsync(ejecutarcmdBD + "          .");
                            lbl_pantalla.Content = ejecutarcmdBD;
                        }
                        if (ejecutarcmdInternos != string.Empty)
                        {
                            if (ejecutarcmdInternos == "Abriendo configuraciones del sistema")
                            {
                                if (ajustes == null)
                                {
                                    ajustes = new Ajustes();
                                    ajustes.Show();
                                    ajustes.Closed += delegate (object a, EventArgs b)
                                    {
                                        ajustes = null;
                                    };
                                }
                            }
                            else if (ejecutarcmdInternos == "Abriendo el editor de comandos")
                            {
                                EditorCmd cmd = new EditorCmd();
                                cmd.Show();
                            }
                            else if (ejecutarcmdInternos == "Hasta pronto")
                            {
                                ia.habla_asistente.Speak(ejecutarcmdInternos + Properties.Settings.Default.NombreUsuario);
                                Application.Current.Shutdown();
                            }
                            else if (ejecutarcmdInternos == "Minimizado")
                            {
                                Visibility = Visibility.Hidden;
                            }
                            else if (ejecutarcmdInternos == "Mostrado")
                            {
                                Visibility = Visibility.Visible;
                            }
                            else if (ejecutarcmdInternos == "Mostrando todos los comandos")
                            {
                                csis = new ComandosSistema();
                                csis.Show();

                            }
                            ia.habla_asistente.SpeakAsync(ejecutarcmdInternos + "         .");
                            lbl_pantalla.Content = ejecutarcmdInternos;
                        }
                        if (calculadora != string.Empty)
                        {
                            ia.habla_asistente.SpeakAsync(calculadora + "         .");
                            lbl_pantalla.Content = calculadora;
                        }
                        if (buscador != string.Empty)
                        {
                            ia.habla_asistente.SpeakAsync(buscador + "          .");
                            lbl_pantalla.Content = buscador;
                        }
                    }
                }
            }
        }
        #endregion
        #region Controles WPF, Eventos WPF
        private void Btn_NomAsisstant_Click(object sender, RoutedEventArgs e)
        {
            if (ajustes == null)
            {
                ajustes = new Ajustes();
                ajustes.Show();
                ajustes.Closed += delegate (object a, EventArgs b)
                {
                    ajustes = null;
                };
            }
        }
        private void BtnComando_Click(object sender, RoutedEventArgs e)
        {
            if (editorCmd == null)
            {
                editorCmd = new EditorCmd();
                editorCmd.Show();
                editorCmd.Closed += delegate (object a, EventArgs b)
                {
                    editorCmd = null;
                };
            }
        }
        private void BtnSerial_Port_Click(object sender, RoutedEventArgs e)
        {
            if (serial == null)
            {
                serial = new ControlSerial();
                serial.Show();
                serial.Closed += delegate (object a, EventArgs b)
                {
                    serial = null;
                };
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            ia.habla_asistente.SelectVoice(Settings.Default.VozDefault);
            ia.habla_asistente.Volume = Settings.Default.VolumenIA;
            Btn_NomAsisstant.Content = Settings.Default.NombreAsistente;
            ia.reconocer_voz_usuario.LoadGrammarAsync(new Grammar(new Choices(Settings.Default.NombreAsistente)));
            Process.GetCurrentProcess().MaxWorkingSet = Process.GetCurrentProcess().MinWorkingSet;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            GuardarPosiciónVentana();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
            e.Cancel = false;
        }
        private void BtnAlarma_Click(object sender, RoutedEventArgs e)
        {
                MessageBox.Show("Parece que el programador no tiene ideas...");
        } 
        private void BtnRecordatorio_Click(object sender, RoutedEventArgs e)
        {
                MessageBox.Show("Hey, si tienes alguna idea pasala al desarrollador :)");
        }

        #endregion
    }
}
