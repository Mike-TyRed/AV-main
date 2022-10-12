using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaAsistente
{
    public class Variables
    {
        public ArrayList listaComandos = new ArrayList();
        public string[] comandos;
        public string[] confirmarcmd;
        public string[] multiejecutar;
        public string[] multirespuesta;
        public string palabraresultcmd;
        public SQLiteConnection con;
        public SQLiteCommand cmd;
        public SQLiteDataReader readerS;
        public DateTime fecha;
        public Random random = new Random();
        public bool cmdConfirmado;
        public string palabraresultante;
        public string cadenaBD;
        public string textToSpeak;
        public SerialPort serial_escucha;
    }
}
