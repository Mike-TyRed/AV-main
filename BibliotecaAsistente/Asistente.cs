using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Data.SQLite;
using System.Windows.Threading;
using System.Media;
using System.Windows.Forms;

namespace BibliotecaAsistente
{
    public class Asistente
    {
        public string NombreAsistente;
        public string NombreUsuario;
        public string _Speech;
        public string speech;
        public double _Confidencia;
        public double _PosiciónX;
        public double _PosiciónY;
        public SpeechSynthesizer habla_asistente;
        public SpeechRecognitionEngine reconocer_voz_usuario;
        public SQLiteConnection con;
        public SQLiteCommand cmd;
        public SQLiteDataReader reader;
        public bool habilitarReconocimiento;
        public bool cmdConfirmado;
        public DispatcherTimer tiempo_desactivar_microfono;
        public DispatcherTimer liberarRAM;
        public DispatcherTimer verificarpuertoconectado;
        public DispatcherTimer verificarpuertodesconectado;
        public RecognitionResult recogResult;
        public int tiempo;
        public SoundPlayer on_mic = new SoundPlayer(@"Assets\\Speech_On.wav");
        public SoundPlayer off_mic = new SoundPlayer(@"Assets\\Speech_Off.wav");
        public Asistente()
        {
            tiempo = 0;
            habla_asistente = new SpeechSynthesizer();
            reconocer_voz_usuario = new SpeechRecognitionEngine();
            tiempo_desactivar_microfono = new DispatcherTimer() {Interval = new TimeSpan(0,0,1)};
            liberarRAM = new DispatcherTimer() { Interval = new TimeSpan(0,0,0,1)};
            verificarpuertoconectado = new DispatcherTimer() { Interval = new TimeSpan (0,0,0,0,100)};
            verificarpuertodesconectado = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };
        }
    }
}
