using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Speech.Recognition;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace BibliotecaAsistente
{
    public class Gramaticas : Variables
    {
        [STAThread]
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MciSendStringA(string lpstrCommand, string buffer, Int32 bufferSize, IntPtr hwndCallback);
        public Gramaticas()
        {
        }
        public string SinonimoCmd(string _speech) //to lower == abrir bandeja
        {
            cmdConfirmado = false;
            try
            {
                con = new SQLiteConnection("Data Source = |DataDirectory|\\DataBase\\Data.sqlite; Version = 3"); //conection stablished
                con.Open();

                cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Sinonimo LIKE '%" + _speech + "%'", con); //abrir bandeja => no lo encuentra
                readerS = cmd.ExecuteReader();

                if (readerS.Read())
                {
                    string[] array = readerS["Sinonimo"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < array.Length; i++)
                    {
                        string result = array[i].ToLower();

                        if (_speech.ToLower() == result)
                        {
                            cmdConfirmado = true;
                        }
                    }
                    if (cmdConfirmado)
                    {
                        _speech = readerS["Comando"].ToString();
                    }
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Comando LIKE '%" + _speech + "%'",con); //abrir bandeja => si encuentra
                readerS = cmd.ExecuteReader();

                if (readerS.Read())
                {
                    if (_speech == readerS["Comando"].ToString().ToLower())
                    {
                        _speech = readerS["Comando"].ToString();
                    }
                }

                con.Close();
            }
            catch (FormatException ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }

            return EjecutarComandoBDInternos(_speech);
        }
        public string EjecutarComandosBD(string _speech)
        {
            cmdConfirmado = false;
            palabraresultcmd = string.Empty;
            con = new SQLiteConnection("Data Source = |DataDirectory|\\DataBase\\Data.sqlite; Version = 3");
            con.Open();


            //CmdSociales
            cmd = new SQLiteCommand("SELECT * FROM CmdSociales WHERE Comando LIKE '%" + _speech + "%'", con);
            readerS = cmd.ExecuteReader(CommandBehavior.SingleResult);
            if (readerS.Read())
            {
                confirmarcmd = readerS["Comando"].ToString().Split(new char[] { '+' });
                for (int i = 0; i < confirmarcmd.Length; i++)
                {
                    string b = confirmarcmd[i].ToLower();
                    if (_speech.ToLower() == b)
                    {
                        cmdConfirmado = true;
                    }
                }
                if (cmdConfirmado)
                {
                    multiejecutar = readerS["Accion"].ToString().Split(new char[] { '+' }); ;
                    if (multiejecutar[0] != string.Empty)
                    {
                        for (int i = 0; i < multiejecutar.Length; i++)
                        {
                            string filename = multiejecutar[i];
                            Process.Start(filename);
                        }
                    }
                    multirespuesta = readerS["Respuesta"].ToString().Split(new char[] { '+' });
                    palabraresultcmd = multirespuesta[random.Next(0, multirespuesta.Count())];
                }
            }

            //CmdCarpetas
            cmd = new SQLiteCommand("SELECT * FROM CmdCarpetas WHERE Comando LIKE '%" + _speech + "%'", con);
            readerS = cmd.ExecuteReader(CommandBehavior.SingleResult);
            if (readerS.Read())
            {
                confirmarcmd = readerS["Comando"].ToString().Split(new char[] { '+' });
                for (int i = 0; i < confirmarcmd.Length; i++)
                {
                    string b = confirmarcmd[i].ToLower();
                    if (_speech.ToLower() == b)
                    {
                        cmdConfirmado = true;
                    }
                }
                if (cmdConfirmado)
                {
                    multiejecutar = readerS["Accion"].ToString().Split(new char[] { '+' }); ;
                    if (multiejecutar[0] != string.Empty)
                    {
                        for (int i = 0; i < multiejecutar.Length; i++)
                        {
                            string filename = multiejecutar[i];
                            Process.Start(filename);
                        }
                    }
                    multirespuesta = readerS["Respuesta"].ToString().Split(new char[] { '+' });
                    palabraresultcmd = multirespuesta[random.Next(0, multirespuesta.Count())];
                }
            }

            //Aplicaciones
            cmd = new SQLiteCommand("SELECT * FROM CmdAplicaciones WHERE Comando LIKE '%" + _speech + "%'", con);
            readerS = cmd.ExecuteReader(CommandBehavior.SingleResult);
            if (readerS.Read())
            {
                confirmarcmd = readerS["Comando"].ToString().Split(new char[] { '+' });
                for (int i = 0; i < confirmarcmd.Length; i++)
                {
                    string b = confirmarcmd[i].ToLower();
                    if (_speech.ToLower() == b)
                    {
                        cmdConfirmado = true;
                    }
                }
                if (cmdConfirmado)
                {
                    multiejecutar = readerS["Accion"].ToString().Split(new char[] { '+' }); ;
                    if (multiejecutar[0] != string.Empty)
                    {
                        for (int i = 0; i < multiejecutar.Length; i++)
                        {
                            string filename = multiejecutar[i];
                            Process.Start(filename);
                        }
                    }
                    multirespuesta = readerS["Respuesta"].ToString().Split(new char[] { '+' });
                    palabraresultcmd = multirespuesta[random.Next(0, multirespuesta.Count())];
                }
            }

            //Paginas Webs
            cmd = new SQLiteCommand("SELECT * FROM CmdPaginasWebs WHERE Comando LIKE '%" + _speech + "%'", con);
            readerS = cmd.ExecuteReader(CommandBehavior.SingleResult);
            if (readerS.Read())
            {
                confirmarcmd = readerS["Comando"].ToString().Split(new char[] { '+' });
                for (int i = 0; i < confirmarcmd.Length; i++)
                {
                    string b = confirmarcmd[i].ToLower();
                    if (_speech.ToLower() == b)
                    {
                        cmdConfirmado = true;
                    }
                }
                if (cmdConfirmado)
                {
                    multiejecutar = readerS["Accion"].ToString().Split(new char[] { '+' }); ;
                    if (multiejecutar[0] != string.Empty)
                    {
                        for (int i = 0; i < multiejecutar.Length; i++)
                        {
                            
                            string filename = multiejecutar[i];
                            Process.Start(filename);
                        }
                    }
                    multirespuesta = readerS["Respuesta"].ToString().Split(new char[] { '+' });
                    palabraresultcmd = multirespuesta[random.Next(0, multirespuesta.Count())];
                }
            }

            //Arduino
            cmd = new SQLiteCommand("SELECT * FROM CmdPuertoSerial WHERE Comando LIKE '%" + _speech + "%'", con);
            readerS = cmd.ExecuteReader(CommandBehavior.SingleResult);
            if (readerS.Read())
            {
                confirmarcmd = readerS["Comando"].ToString().Split(new char[] { '+' });
                for (int i = 0; i < confirmarcmd.Length; i++)
                {
                    string b = confirmarcmd[i].ToLower();
                    if (_speech.ToLower() == b)
                    {
                        cmdConfirmado = true;
                    }
                }

                serial_escucha = new SerialPort(readerS["PuertoCOM"].ToString()) //C0M16 Y COM1
                {
                    BaudRate = Int32.Parse("9600"),
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None,
                    PortName = readerS["PuertoCOM"].ToString()
                };
                if (cmdConfirmado)
                {
                    multiejecutar = readerS["Accion"].ToString().Split(new char[] { '+' }); ;
                    if (multiejecutar[0] != string.Empty)
                    {
                        for (int i = 0; i < multiejecutar.Length; i++)
                        {
                            string filename = multiejecutar[i];
                            try
                            {
                                Console.WriteLine(readerS["PuertoCOM"].ToString());
                                serial_escucha.Open();
                                serial_escucha.DiscardOutBuffer();
                                serial_escucha.Write(readerS["Accion"].ToString());
                                serial_escucha.Close();
                            }
                            catch
                            {

                                return palabraresultcmd = "No se encuentra un puerto disponible";
                            }
                        }
                    }
                    multirespuesta = readerS["Respuesta"].ToString().Split(new char[] { '+' });
                    palabraresultcmd = multirespuesta[random.Next(0, multirespuesta.Count())];
                }
            }

            con.Close();
            return palabraresultcmd;
        }

        [STAThread]
        public string EjecutarComandoBDInternos(string speech)
        {
            palabraresultante = string.Empty;
            con = new SQLiteConnection("Data Source = |DataDirectory|\\DataBase\\Data.sqlite; Version = 3");
            con.Open();

            cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Comando LIKE '%" + speech + "%'", con);
            readerS = cmd.ExecuteReader();
            if (readerS.Read())
            {
                if (speech == "Abrir configuración")
                {
                    palabraresultante = "Abriendo configuraciones del sistema";
                }
                else
                {
                    if (speech == "¿Qué hora es?")
                    {
                        fecha = DateTime.Now;
                        string hora = fecha.ToString("hh:mm tt");
                        palabraresultante = hora;
                    }
                    else
                    {
                        if (speech == "Leer esto")
                        {
                            Clipboard.Clear();
                            SendKeys.SendWait("^(c)");
                            palabraresultante = Clipboard.GetText();
                        }
                        else
                        {
                            if (speech == "Agregar comando")
                            {
                                palabraresultante = "Abriendo el editor de comandos";
                            }
                            else
                            {
                                if (speech == "Cerrar asistente")
                                {
                                    palabraresultante = "Hasta pronto ";
                                }
                                else
                                {
                                    if (speech == "Cerrar sesión")
                                    {
                                        Process.Start("shutdown.exe", "-l");
                                    }
                                    else
                                    {
                                        if (speech == "Minimizar el asistente")
                                        {
                                            palabraresultante = "Minimizado";
                                        }
                                        else if (speech == "Mostrar asistente")
                                        {
                                            palabraresultante = "Mostrado";
                                        }
                                        else if (speech == "Ver comandos")
                                        {
                                            palabraresultante = "Mostrando todos los comandos";
                                        }
                                        else if (speech == "Acción cerrar")
                                        {
                                            SendKeys.SendWait("%{F4}");
                                            palabraresultante = "Cerrado";
                                        }
                                        else if (speech == "Acción copiar")
                                        {
                                            SendKeys.SendWait("^(c)");
                                            palabraresultante = "Copiado";
                                        }
                                        else if (speech == "Acción cortar")
                                        {
                                            SendKeys.SendWait("^(x)");
                                            palabraresultante = "Cortado";
                                        }
                                        else if (speech == "Acción pegar")
                                        {
                                            SendKeys.SendWait("^(v)");
                                            palabraresultante = "Pegado";
                                        }
                                        else if (speech == "Acción deshacer")
                                        {
                                            SendKeys.SendWait("^(z)");
                                            palabraresultante = "listo";
                                        }
                                        else if (speech == "Acción guardar")
                                        {
                                            SendKeys.SendWait("^(g)");
                                            palabraresultante = "guardado";
                                        }
                                        else if (speech == "Acción no guardar")
                                        {
                                            SendKeys.SendWait("%(n)");
                                            palabraresultante = "listo";
                                        }
                                        else if (speech == "¿Qué día es?")
                                        {
                                            fecha = DateTime.Now;
                                            string dia = fecha.ToString("dddd");
                                            palabraresultante = dia;
                                        }
                                        else if (speech == "¿Qué fecha es hoy?")
                                        {
                                            fecha = DateTime.Now;
                                            string dia_actual, numeroactual, mes, año;

                                            dia_actual = fecha.ToString("dddd");
                                            numeroactual = fecha.ToString("dd");
                                            mes = fecha.ToString("MMMM");
                                            año = fecha.ToString("yyyy");

                                            palabraresultante = dia_actual + " " + numeroactual + " de " + mes + " de " + año;
                                        }
                                        else if (speech == "Abrir bandeja")
                                        {
                                            MciSendStringA("set CDAudio door open", "", 127, IntPtr.Zero);
                                            palabraresultante = "Bandeja abierta";
                                        }
                                        else if (speech == "Cerrar bandeja")
                                        {
                                            MciSendStringA("set CDAudio door open", "", 127, IntPtr.Zero);
                                            palabraresultante = "Bandeja cerrada";
                                        }
                                        else if (speech == "Importa preferencias")
                                        {
                                            palabraresultante = "Preferencias importadas con éxito.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return palabraresultante;
        }
        public string Calc(RecognitionResult recoResult)
        {
            string resultadocalc = string.Empty;
            if (recoResult.Semantics.ContainsKey("num1") && recoResult.Semantics.ContainsKey("num2") && recoResult.Semantics.ContainsKey("operator"))
            {
                try
                {
                    if (recoResult.Semantics["num1"].Value.ToString() != string.Empty && recoResult.Semantics["operator"].Value.ToString() != string.Empty && recoResult.Semantics["num2"].Value.ToString() != string.Empty)
                    {
                        int operation1 = Convert.ToInt32(recoResult.Semantics["num1"].Value.ToString());
                        int operation2 = Convert.ToInt32(recoResult.Semantics["num2"].Value.ToString());
                        char operatorc = Convert.ToChar(recoResult.Semantics["operator"].Value.ToString());
                        resultadocalc = Calcular(operation1, operatorc, operation2);
                    }
                }
                catch (Exception ex)
                {

                    System.Windows.MessageBox.Show(ex.Message + " error de cálculo");
                }
            }
            return resultadocalc;
        }
        public string Calcular(int op1, char operation, int op2)
        {
            int result = 0;
            String operationStr = "";
            textToSpeak = string.Empty;

            /*Float Div Variables*/
            float num1 = op1;
            float num2 = op2;
            float resultdiv;
            if (operation == '/' && op2 == 0)
            {
                textToSpeak = op1.ToString() + " entre cero no está definido.";
            }
            else
            {
                switch (operation)
                {
                    case '+':
                        result = op1 + op2;
                        operationStr = " mas ";
                        textToSpeak = op1.ToString() + operationStr + op2.ToString() + " es " + result.ToString();
                        break;
                    case '-':
                        result = op1 - op2;
                        operationStr = " menos ";
                        textToSpeak = op1.ToString() + operationStr + op2.ToString() + " es " + result.ToString();
                        break;
                    case '*':
                        result = op1 * op2;
                        operationStr = " por ";
                        textToSpeak = op1.ToString() + operationStr + op2.ToString() + " es " + result.ToString();
                        break;
                    case '/':
                        resultdiv = num1 / num2;
                        operationStr = " entre ";
                        textToSpeak = op1.ToString() + operationStr + op2.ToString() + " es " + Math.Round(resultdiv, 2).ToString();
                        break;
                }
            }
            return textToSpeak;
        }
        public string Busc(RecognitionResult recoResult, string speech)
        {
            string frasespeech = string.Empty;
            if (recoResult.Semantics.ContainsKey("Buscador1"))
            {
                frasespeech = BuscadorWeb(speech, recoResult.Semantics["Buscador1"].Value.ToString());
            }
            return frasespeech;
        }
        public string BuscadorWeb(string _speech, string explorador)
        {
            //buscar la historia de méxico en google
            string frasefinal = _speech.Remove(0, 9);
            string frase = string.Empty;
            int inicio = frasefinal.IndexOf(explorador);
            frasefinal = frasefinal.Remove(inicio - 3, explorador.Length + 3);
            if (explorador == "Google")
            {
                Process.Start("https://www.google.com.mx/search?q=" + frasefinal.Trim());
                frase = "Buscando " + frasefinal.Trim() + " en Google";
            }
            else if (explorador == "YouTube")
            {
                Process.Start("https://www.youtube.com/results?search_query=" + frasefinal.Trim());
                frase = "Buscando " + frasefinal.Trim() + " en Youtube";
            }
            else
            {
                Process.Start("https://es.wikipedia.org/wiki/" + frasefinal.Trim());
                frase = "Buscando " + frasefinal.Trim() + " en Wikipedia";
            }

            return frase;
        }
    }
}
