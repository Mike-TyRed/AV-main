using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Collections;
using BibliotecaAsistente.Properties;

namespace BibliotecaAsistente
{
    public class CargarGramaticas : Variables
    {
        public CargarGramaticas()
        {

        }

        public void SeleccionarComandos()
        {
            try
            {
                con = new SQLiteConnection(Settings.Default.ConexionBD);
                con.Open();
                cmd = new SQLiteCommand("SELECT Comando FROM CmdSociales", con);
                readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    comandos = readerS["Comando"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < comandos.Length; i++)
                    {
                        listaComandos.Add(comandos[i]);
                    }
                }

                cmd = new SQLiteCommand("SELECT Comando FROM CmdCarpetas", con);
                readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    comandos = readerS["Comando"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < comandos.Length; i++)
                    {
                        listaComandos.Add(comandos[i]);
                    }
                }

                cmd = new SQLiteCommand("SELECT Comando FROM CmdAplicaciones", con);
                readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    comandos = readerS["Comando"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < comandos.Length; i++)
                    {
                        listaComandos.Add(comandos[i]);
                    }
                }

                cmd = new SQLiteCommand("SELECT Comando FROM CmdPaginasWebs", con);
                readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    comandos = readerS["Comando"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < comandos.Length; i++)
                    {
                        listaComandos.Add(comandos[i]);
                    }
                }

                cmd = new SQLiteCommand("SELECT Comando FROM CmdInternos", con);
                readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    comandos = readerS["Comando"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < comandos.Length; i++)
                    {
                        listaComandos.Add(comandos[i]);
                    }

                }

                cmd = new SQLiteCommand("SELECT Sinonimo FROM CmdInternos", con);
                readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    comandos = readerS["Sinonimo"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < comandos.Length; i++)
                    {
                            listaComandos.Add(comandos[i]);
                    }

                }

                cmd = new SQLiteCommand("SELECT Comando FROM CmdPuertoSerial", con);
                readerS = cmd.ExecuteReader();
                while (readerS.Read())
                {
                    comandos = readerS["Comando"].ToString().Split(new char[] { '+' });
                    for (int i = 0; i < comandos.Length; i++)
                    {
                        listaComandos.Add(comandos[i]);
                    }

                }

                con.Close();
            }
            catch
            {

                MessageBox.Show("Aquí hay un error. Puede ser que no exista la base de datos");
            }
        }
        public Choices CargarGramáticasBD()
        {
            Choices comandosBD = new Choices(listaComandos.ToArray(typeof(string)) as string[]);
            return comandosBD;
        }
        public Choices ComandosInternos()
        {
            return new Choices(new string[] { "Activar el micrófono", "Desactivar el micrófono", "Desactivar micrófono" });
        }
        public Grammar CargarGramáticasWebs()
        {
            Choices buscadores = new Choices(new string[] { "Google", "YouTube", "Wikipedia" });
            Choices option = new Choices(new string[] {"la","el","las","los","un","una","unos","unas"});
            GrammarBuilder frase1 = new GrammarBuilder("Buscar");
            frase1.Append(option);
            frase1.AppendDictation();
            frase1.Append("en");
            frase1.Append(new SemanticResultKey("Buscador1", buscadores));

            Grammar grammarWebs = new Grammar(frase1);

            return grammarWebs;
        }
        public GrammarBuilder CargarGramáticasCalc()
        {
            GrammarBuilder[] array = new GrammarBuilder[] { null, null };
            array[0] = new GrammarBuilder(new Choices("salir"));
            array[1] = new GrammarBuilder();
            array[1].Append("cuánto es",0,1);
            string[] numberString = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete","ocho","nueve", "diez","once","doce",
                                      "trece","catorce","quince","dieciseis","diecisiete","dieciocho","diecinueve","veinte","veinte y uno",
                                      "veinte y dos", "veinte y tres", "veinte y cuatro", "veinte y cinco", "veinte y seis", "veinte y siete",
                                      "veinte y ocho ", "veinte y nueve","treinta","treinta y uno","treinta y dos","treinta y tres","treinta y cuatro",
                                      "treinta y cinco","treinta y seis","treinta y siete","treinta y ocho","treinta y nueve","cuarenta"};

            Choices choices = new Choices();
            for (int i = 0; i < numberString.Length; i++)
            {
                choices.Add(new SemanticResultValue(numberString[i], i));
            }

            array[1].Append(new SemanticResultKey("num1", (GrammarBuilder)choices));

            string[] operatorString = { "mas", "menos", "por", "multiplicado por", "entre", "dividido entre" };
            Choices operatorChoices = new Choices();
            operatorChoices.Add(new GrammarBuilder[] { new SemanticResultValue("mas", "+") });
            operatorChoices.Add(new GrammarBuilder[] { new SemanticResultValue("menos", "-") });
            operatorChoices.Add(new GrammarBuilder[] { new SemanticResultValue("por", "*") });
            operatorChoices.Add(new GrammarBuilder[] { new SemanticResultValue("multiplicado por", "*") });
            operatorChoices.Add(new GrammarBuilder[] { new SemanticResultValue("entre", "/") });
            operatorChoices.Add(new GrammarBuilder[] { new SemanticResultValue("dividido entre", "/") });

            array[1].Append(new SemanticResultKey("operator", (GrammarBuilder)operatorChoices));
            array[1].Append(new SemanticResultKey("num2", (GrammarBuilder)choices));

            return new GrammarBuilder(new Choices(array));
        }
    }
}
