using AsistenteVirtualSTABLE.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AsistenteVirtualSTABLE
{
    public partial class ComandosSistema : Window
    {
        #region Variables
        TreeViewItem alarma = new TreeViewItem();
        TreeViewItem acciones = new TreeViewItem();
        TreeViewItem buscador = new TreeViewItem();
        TreeViewItem calculadora = new TreeViewItem();
        TreeViewItem clima = new TreeViewItem();
        TreeViewItem sistema = new TreeViewItem();
        TreeViewItem cmds = new TreeViewItem();
        TreeViewItem cmdc = new TreeViewItem();
        TreeViewItem cmda = new TreeViewItem();
        TreeViewItem cmdp = new TreeViewItem();
        TreeViewItem cmdps = new TreeViewItem();
        List<string> alarmacmd = new List<string>();
        List<string> accionescmd = new List<string>();
        List<string> buscadorcmd = new List<string>();
        List<string> calculadoracmd = new List<string>();
        List<string> climacmd = new List<string>();
        List<string> sistemacmd = new List<string>();
        List<string> cmdscmd = new List<string>();
        List<string> cmdccmd = new List<string>();
        List<string> cmdacmd = new List<string>();
        List<string> cmdpcmd = new List<string>();
        List<string> cmdpscmd = new List<string>();
        string[] array;
        string comando;
        bool flag_general;

        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader reader;
        #endregion
        #region Main
        public ComandosSistema()
        {
            InitializeComponent();
            PosicionInicial();
            CargarListaComandos();
            CargarItemsTV();
            LoadComandos();
        }
        #endregion
        #region Metodos
        void CargarItemsTV()
        {
            tvComandos.Items.Add(alarma);
            tvComandos.Items.Add(acciones);
            tvComandos.Items.Add(buscador);
            tvComandos.Items.Add(calculadora);
            tvComandos.Items.Add(clima);
            tvComandos.Items.Add(sistema);
            tvComandos.Items.Add(cmds);
            tvComandos.Items.Add(cmdc);
            tvComandos.Items.Add(cmda);
            tvComandos.Items.Add(cmdp);
            tvComandos.Items.Add(cmdps);
        }
        void AddComandos()
        {
            alarmacmd.Add("configurar alarma a las [hora]");
            buscadorcmd.Add("buscar [...]");
            buscadorcmd.Add("buscar [...] en [...]");
            buscadorcmd.Add("buscar en [...] [...]");
            calculadoracmd.Add("[núm] [operador] [núm]");
            calculadoracmd.Add("cuanto es [núm] [operador] [núm]");
        }
        void LoadComandos()
        {
            try
            {
                con = new SQLiteConnection(Settings.Default.ConexionBD);
                con.Open();

                cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Tipo LIKE 'Alarma'", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    alarmacmd.Add(reader["Comando"].ToString());
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Tipo LIKE 'Sistema'", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sistemacmd.Add(reader["Comando"].ToString());
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Tipo LIKE 'Clima'", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    climacmd.Add(reader["Comando"].ToString());
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Tipo LIKE 'Atajos'", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    accionescmd.Add(reader["Comando"].ToString());
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdSociales", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmdscmd.Add(reader["Comando"].ToString().Split(new char[] { '+' })[0]);
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdCarpetas", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmdccmd.Add(reader["Comando"].ToString().Split(new char[] { '+' })[0]);
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdAplicaciones", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmdacmd.Add(reader["Comando"].ToString().Split(new char[] { '+' })[0]);
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdPaginasWebs", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmdpcmd.Add(reader["Comando"].ToString().Split(new char[] { '+' })[0]);
                }

                cmd = new SQLiteCommand("SELECT * FROM CmdPuertoSerial", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmdpscmd.Add(reader["Comando"].ToString().Split(new char[] { '+' })[0]);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            con.Close();
            AddComandos();
            alarma.ItemsSource = alarmacmd;
            sistema.ItemsSource = sistemacmd;
            clima.ItemsSource = climacmd;
            acciones.ItemsSource = accionescmd;
            buscador.ItemsSource = buscadorcmd;
            calculadora.ItemsSource = calculadoracmd;
            cmds.ItemsSource = cmdscmd;
            cmdc.ItemsSource = cmdccmd;
            cmda.ItemsSource = cmdacmd;
            cmdp.ItemsSource = cmdpcmd;
            cmdps.ItemsSource = cmdpscmd;
        }
        void PosicionInicial()
        {
            Rect workArea = SystemParameters.WorkArea;
            Left = workArea.Right - 280.0;
            Top = workArea.Bottom / 2.0 - 235.0;
        }
        void CargarListaComandos()
        {
            alarma.Header = "Alarma";
            acciones.Header = "Acciones";
            buscador.Header = "Buscador";
            calculadora.Header = "Calculadora";
            clima.Header = "Clima";
            sistema.Header = "Sistema";
            cmds.Header = "Comandos Sociales";
            cmdc.Header = "Comandos Carpetas";
            cmda.Header = "Comandos Aplicaciones";
            cmdp.Header = "Comandos Páginas Webs";
            cmdps.Header = "Comandos Puerto Serial";
        }
        #endregion
        #region Metodos Internos
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void TvComandos_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = sender as TreeView;
            bool flag = tvComandos.SelectedItem is TreeViewItem;
            if (flag)
            {
                TreeViewItem treeViewItem = tvComandos.SelectedItem as TreeViewItem;
                flag_general = treeViewItem.Header.ToString() == "Alarma";
                if (flag_general)
                {
                    TbxInfo.Text = "Configure la alarma";
                }
                else
                {
                    flag_general = treeViewItem.Header.ToString() == "Acciones";
                    if (flag_general)
                    {
                        TbxInfo.Text = "Realice las acciones comunes del teclado";
                    }
                    else
                    {
                        flag_general = treeViewItem.Header.ToString() == "Buscador";
                        if (flag_general)
                        {
                            TbxInfo.Text = "Realice una búsqueda en las páginas webs más conocidas. \nRequiere de una conexión a internet.";
                        }
                        else
                        {
                            flag_general = treeViewItem.Header.ToString() == "Calculadora";
                            if (flag_general)
                            {
                                TbxInfo.Text = "Realice las operaciones matemáticas básicas.";
                            }
                            else
                            {
                                flag_general = treeViewItem.Header.ToString() == "Clima";
                                if (flag_general)
                                {
                                    TbxInfo.Text = "Sepa cuál es el clima para hoy y mañana. \nEsto requiere conexión a internet.";
                                }
                                else
                                {
                                    flag_general = treeViewItem.Header.ToString() == "Sistema";
                                    if (flag_general)
                                    {
                                        TbxInfo.Text = "Controle el sistema operativo Windows por voz.";
                                    }
                                    else
                                    {
                                        flag_general = treeViewItem.Header.ToString() == "Comandos Sociales";
                                        if (flag_general)
                                        {
                                            TbxInfo.Text = "Permite que el asistente responda a las frases anteriormente guardadas.";
                                        }
                                        else
                                        {
                                            flag_general = treeViewItem.Header.ToString() == "Comandos Carpetas";
                                            if (flag_general)
                                            {
                                                TbxInfo.Text = "Le permite abrir sus carpetas guardadas.";
                                            }
                                            else
                                            {
                                                flag_general = treeViewItem.Header.ToString() == "Comandos Aplicaciones";
                                                if (flag_general)
                                                {
                                                    TbxInfo.Text = "Le permite abrir sus aplicaciones guardadas.";
                                                }

                                                else
                                                {
                                                    flag_general = treeViewItem.Header.ToString() == "Comandos Páginas Webs";
                                                    if (flag_general)
                                                    {
                                                        TbxInfo.Text = "Le permite abrir sus páginas webs guardadas.";
                                                    }
                                                    else
                                                    {
                                                        flag_general = treeViewItem.Header.ToString() == "Comandos Puerto Serial";
                                                        if (flag_general)
                                                        {
                                                            TbxInfo.Text = "Le permite controlar un Arduino por voz.";
                                                        }
                                                        else
                                                        {
                                                            TbxInfo.Text = string.Empty;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (treeView.SelectedItem is string)
                {
                    flag = false;
                    TbxInfo.Text = "Sinónimos:\n";
                    con = new SQLiteConnection(Settings.Default.ConexionBD);
                    con.Open();
                    try
                    {

                        if (!flag)
                        {
                            cmd = new SQLiteCommand("SELECT * FROM CmdSociales WHERE Comando LIKE '" + treeView.SelectedItem.ToString() + "%'", con);
                            reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            while (reader.Read())
                            {
                                array = reader["Comando"].ToString().Split(new char[] { '+' });

                                for (int i = 0; i < array.Length; i++)
                                {
                                    comando = array[i];
                                    TbxInfo.AppendText(comando + "\n");
                                }
                            }
                        }
                        if (!flag)
                        {
                            cmd = new SQLiteCommand("SELECT * FROM CmdCarpetas WHERE Comando LIKE '" + treeView.SelectedItem.ToString() + "%'", con);
                            reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            while (reader.Read())
                            {
                                array = reader["Comando"].ToString().Split(new char[] { '+' });

                                for (int i = 0; i < array.Length; i++)
                                {
                                    comando = array[i];
                                    TbxInfo.AppendText(comando + "\n");
                                }
                            }
                        }

                        if (!flag)
                        {
                            cmd = new SQLiteCommand("SELECT * FROM CmdAplicaciones WHERE Comando LIKE '" + treeView.SelectedItem.ToString() + "%'", con);
                            reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            while (reader.Read())
                            {
                                array = reader["Comando"].ToString().Split(new char[] { '+' });

                                for (int i = 0; i < array.Length; i++)
                                {
                                    comando = array[i];
                                    TbxInfo.AppendText(comando + "\n");
                                }
                            }
                        }
                        if (!flag)
                        {
                            cmd = new SQLiteCommand("SELECT * FROM CmdPaginasWebs WHERE Comando LIKE '" + treeView.SelectedItem.ToString() + "%'", con);
                            reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            while (reader.Read())
                            {
                                array = reader["Comando"].ToString().Split(new char[] { '+' });

                                for (int i = 0; i < array.Length; i++)
                                {
                                    comando = array[i];
                                    TbxInfo.AppendText(comando + "\n");
                                }
                            }
                        }

                        if (!flag)
                        {
                            cmd = new SQLiteCommand("SELECT * FROM CmdPuertoSerial WHERE Comando LIKE '" + treeView.SelectedItem.ToString() + "%'", con);
                            reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            while (reader.Read())
                            {
                                array = reader["Comando"].ToString().Split(new char[] { '+' });

                                for (int i = 0; i < array.Length; i++)
                                {
                                    comando = array[i];
                                    TbxInfo.AppendText(comando + "\n");
                                }
                            }
                        }

                        if (!flag)
                        {
                            cmd = new SQLiteCommand("SELECT * FROM CmdInternos WHERE Comando LIKE '" + treeView.SelectedItem.ToString() + "%'", con);
                            reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                            while (reader.Read())
                            {
                                array = reader["Sinonimo"].ToString().Split(new char[] { '+' });

                                for (int i = 0; i < array.Length; i++)
                                {
                                    comando = array[i];
                                    TbxInfo.AppendText(comando + "\n");
                                }
                            }
                        }


                        con.Close();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion
    }
}
