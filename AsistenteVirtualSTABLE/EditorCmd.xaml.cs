using AsistenteVirtualSTABLE.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace AsistenteVirtualSTABLE
{
    public partial class EditorCmd : Window
    {
        #region Variables
        private SQLiteConnection con;
        private SQLiteDataAdapter adap;
        private SQLiteCommand cmd;
        private SQLiteCommandBuilder cmdb;
        private DataSet ds;
        private ComandosSistema comandos;
        private ArrayList listaComando = new ArrayList();
        private bool celda_ingresada = false;
        private string tipo;
        #endregion
        #region Main
        public EditorCmd()
        {
            InitializeComponent();
            CargarCmdSociales();
            IdiomaEspañol();
        }
        #endregion
        #region Metodos internos
        private void IdiomaEspañol()
        {
            lblTitle.Content = "COMANDOS DEL SISTEMA";
            BtnSociales.Content = "SOCIALES";
            BtnCarpetas.Content = "CARPETAS";
            BtnAplicaciones.Content = "PROGRAMAS";
            BtnPaginasWebs.Content = "PAGINAS WEBS";
            BtnInternos.Content = "INTERNOS";
            BtnAyuda.Content = "AYUDA";
            BtnIO.Content = "BASE DE DATOS";
            BtnNuevoComando.Content = "NUEVO COMANDO";
            BtnEliminarComando.Content = "ELIMINAR";
            lblcomandoTitle.Content = "COMANDO+";
            lblaccionTitle.Content = "ACCIÓN+";
            lblRespuesta.Content = "RESPUESTA+";
            lblbdio.Content = "BASE DE DATOS";
            lbbdinfo.Content = "Importar: Reemplaza la actual base de datos por una nueva.\nExportar: Guarde la base de datos en su pc.\nNota: No cambiar el nombre a la base de datos.";
            BtnImportar.Content = "IMPORTAR";
            BtnExportar.Content = "EXPORTAR";
        }

        private void CargarCmdSociales()
        {
            tipo = "Sociales";
            try
            {
                con = new SQLiteConnection(Settings.Default.ConexionBD);
                con.Open();
                adap = new SQLiteDataAdapter("SELECT ID,Comando, Accion, Respuesta FROM CmdSociales", con);
                ds = new DataSet();
                adap.Fill(ds);
                DataGridP.ItemsSource = ds.Tables[0].DefaultView;
                con.Close();
            }
            catch
            {
                System.Windows.MessageBox.Show("Hay un error en la base de datos, en la tabla de CmdSociales, por favor verifique el error.");
            }
        }
        private void CargarCmdCarpetas()
        {
            tipo = "Carpetas";
            try
            {
                con = new SQLiteConnection(Settings.Default.ConexionBD);
                con.Open();
                adap = new SQLiteDataAdapter("SELECT Id, Comando, Accion, Respuesta FROM CmdCarpetas", con);
                ds = new DataSet();
                adap.Fill(ds);
                DataGridP.ItemsSource = ds.Tables[0].DefaultView;
                con.Close();
            }
            catch
            {

                System.Windows.MessageBox.Show("Hay un error en la base de datos, en la tabla de CmdCarpetas, por favor verifique el error.");
            }
        }
        private void CargarCmdAplicaciones()
        {
            tipo = "Aplicaciones";
            try
            {
                con = new SQLiteConnection(Settings.Default.ConexionBD);
                con.Open();
                adap = new SQLiteDataAdapter("SELECT Id, Comando, Accion, Respuesta FROM CmdAplicaciones", con);
                ds = new DataSet();
                adap.Fill(ds);
                DataGridP.ItemsSource = ds.Tables[0].DefaultView;
                con.Close();
            }
            catch
            {

                System.Windows.MessageBox.Show("Hay un error en la base de datos, en la tabla de CmdAplicaciones, por favor verifique el error.");
            }
        }
        private void CargarCmdPaginasWebs()
        {
            tipo = "Paginas Webs";
            try
            {
                con = new SQLiteConnection(Settings.Default.ConexionBD);
                con.Open();
                adap = new SQLiteDataAdapter("SELECT Id, Comando, Accion, Respuesta FROM CmdPaginasWebs", con);
                ds = new DataSet();
                adap.Fill(ds);
                DataGridP.ItemsSource = ds.Tables[0].DefaultView;
                con.Close();
            }
            catch
            {

                System.Windows.MessageBox.Show("Hay un error en la base de datos, en la tabla de CmdPaginasWebs, por favor verifique el error.");
            }
        }
        private void CargarCmdInternos()
        {
            tipo = "Internos";
            try
            {
                con = new SQLiteConnection(Settings.Default.ConexionBD);
                con.Open();
                adap = new SQLiteDataAdapter("SELECT ID, Comando, Accion, Sinonimo FROM CmdInternos", con);
                ds = new DataSet();
                adap.Fill(ds);
                DataGridP.ItemsSource = ds.Tables[0].DefaultView;
                con.Close();
            }
            catch
            {

                System.Windows.MessageBox.Show("Vaya, esto no debería pasar con los comandos internos, pero igual hay un error.");
            }
        }
        private void BorrarComandoBD()
        {

            List<DataRow> theRows = new List<DataRow>();
            for (int i = 0; i < DataGridP.SelectedItems.Count; ++i)
            {
                object o = DataGridP.SelectedItems[i];
                if (o != CollectionView.NewItemPlaceholder)
                {
                    DataRowView r = (DataRowView)o;
                    theRows.Add(r.Row);
                }
            }

            foreach (DataRow r in theRows)
            {
                int k = ds.Tables[0].Rows.IndexOf(r);
                ds.Tables[0].Rows[k].Delete();
            }

        }
        private void ActualizarTabla()
        {
            cmdb = new SQLiteCommandBuilder(adap);
            adap.Update(ds);
        }
        private void AgregarCmdBD()
        {
            try
            {
                if (tipo == "Sociales")
                {
                    con = new SQLiteConnection(Settings.Default.ConexionBD);
                    con.Open();
                    cmd = new SQLiteCommand(string.Format("INSERT INTO CmdSociales (Comando, Accion, Respuesta) values('{0}','{1}','{2}')", Tbx_Comando.Text, tbxAccion.Text, TbxRespuesta.Text), con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //ActualizarTabla();
                    CargarCmdSociales();
                    LimpiarTextBox();
                }
                else if (tipo == "Carpetas")
                {
                    con = new SQLiteConnection(Settings.Default.ConexionBD);
                    con.Open();
                    cmd = new SQLiteCommand(string.Format("INSERT INTO CmdCarpetas (Comando, Accion, Respuesta) values('{0}','{1}','{2}')", Tbx_Comando.Text, tbxAccion.Text, TbxRespuesta.Text), con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //ActualizarTabla();
                    CargarCmdCarpetas();
                    LimpiarTextBox();
                }
                else if (tipo == "Aplicaciones")
                {
                    con = new SQLiteConnection(Settings.Default.ConexionBD);
                    con.Open();
                    cmd = new SQLiteCommand(string.Format("INSERT INTO CmdAplicaciones (Comando, Accion, Respuesta) values('{0}','{1}','{2}')", Tbx_Comando.Text, tbxAccion.Text, TbxRespuesta.Text), con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //ActualizarTabla();
                    CargarCmdAplicaciones();
                    LimpiarTextBox();
                }
                else if (tipo.Equals("Paginas Webs"))
                {
                    con = new SQLiteConnection(Settings.Default.ConexionBD);
                    con.Open();
                    cmd = new SQLiteCommand(string.Format("INSERT INTO CmdPaginasWebs (Comando, Accion, Respuesta) values('{0}','{1}','{2}')", Tbx_Comando.Text, tbxAccion.Text, TbxRespuesta.Text), con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   // ActualizarTabla();
                    CargarCmdPaginasWebs();
                    LimpiarTextBox();
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Aquí hubo un problemón, posiblemente sea el código o no encuentro la base de datos.");
            }
        }

        private void LimpiarTextBox()
        {
            Tbx_Comando.Text = string.Empty;
            tbxAccion.Text = string.Empty;
            TbxRespuesta.Text = string.Empty;
        }

        private void OcultarBotones()
        {
            BtnNuevoComando.Visibility = Visibility.Hidden;
            BtnEliminarComando.Visibility = Visibility.Hidden;
        }

        private void MostrarBotones()
        {
            BtnNuevoComando.Visibility = Visibility.Visible;
            BtnEliminarComando.Visibility = Visibility.Visible;
        }

        private void Buscar_Archivos()
        {
            if (tipo == "Aplicaciones")
            {
                Microsoft.Win32.OpenFileDialog buscar_programa = new Microsoft.Win32.OpenFileDialog();
                buscar_programa.ShowDialog();
                tbxAccion.Text = buscar_programa.FileName.ToString();
            }
            else if (tipo == "Carpetas")
            {
                System.Windows.Forms.FolderBrowserDialog buscar_carpetas = new System.Windows.Forms.FolderBrowserDialog();
                buscar_carpetas.ShowDialog();
                tbxAccion.Text = buscar_carpetas.SelectedPath;
            }
        }
        #endregion
        #region Eventos de controles
        private void BtnSociales_Click(object sender, RoutedEventArgs e)
        {
            CargarCmdSociales();
            MostrarBotones();
            DataGridP.Columns[0].Visibility = Visibility.Hidden;
                lblRespuesta.Content = "RESPUESTA+";
        }
        private void BtnCarpetas_Click(object sender, RoutedEventArgs e)
        {
            CargarCmdCarpetas();
            MostrarBotones();
            DataGridP.Columns[0].Visibility = Visibility.Hidden;
                lblRespuesta.Content = "RESPUESTA+";
        }
        private void BtnAplicaciones_Click(object sender, RoutedEventArgs e)
        {
            CargarCmdAplicaciones();
            MostrarBotones();
            DataGridP.Columns[0].Visibility = Visibility.Hidden;
                lblRespuesta.Content = "RESPUESTA+";
        }
        private void BtnPaginasWebs_Click(object sender, RoutedEventArgs e)
        {
            CargarCmdPaginasWebs();
            MostrarBotones();
            DataGridP.Columns[0].Visibility = Visibility.Hidden;
                lblRespuesta.Content = "RESPUESTA+";
        }
        private void BtnInternos_Click(object sender, RoutedEventArgs e)
        {
            CargarCmdInternos();
            OcultarBotones();
            DataGridP.Columns[0].Visibility = Visibility.Hidden;
            DataGridP.Columns[1].IsReadOnly = true;
            DataGridP.Columns[2].IsReadOnly = true;
            lblRespuesta.Content = "SINONIMOS+";
        }
        private void BtnEliminarComando_Click(object sender, RoutedEventArgs e)
        {
            BorrarComandoBD();
            ActualizarTabla();
            DataGridP.Columns[0].Visibility = Visibility.Hidden;
        }
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            Buscar_Archivos();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridP.Columns[0].Visibility = Visibility.Hidden;
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet = System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet;
            Close();
        }
        private void BtnNuevoComando_Click(object sender, RoutedEventArgs e)
        {
            if (BtnNuevoComando.Content.Equals("NUEVO COMANDO"))
            {
                Tbx_Comando.Visibility = Visibility.Visible;
                tbxAccion.Visibility = Visibility.Visible;
                BtnBuscar.Visibility = Visibility.Visible;
                TbxRespuesta.Visibility = Visibility.Visible;
                BtnNuevoComando.Content = "AGREGAR";
            }
            else
            {
                if (Tbx_Comando.Text == string.Empty && tbxAccion.Text == string.Empty && TbxRespuesta.Text == string.Empty)
                {
                    System.Windows.MessageBox.Show("No se permiten comandos vacios", "Error de sistema", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    AgregarCmdBD();
                    DataGridP.Columns[0].Visibility = Visibility.Hidden;
                    Tbx_Comando.Visibility = Visibility.Hidden;
                    tbxAccion.Visibility = Visibility.Hidden;
                    BtnBuscar.Visibility = Visibility.Hidden;
                    TbxRespuesta.Visibility = Visibility.Hidden;
                    BtnNuevoComando.Content = "NUEVO COMANDO";
                }

            }
        }
        private void DataGridP_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            celda_ingresada = true;
        }
        private void DataGridP_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (celda_ingresada == true)
            {
                ActualizarTabla();
                if (tipo == "Sociales")
                {
                    CargarCmdSociales();
                }
                else if (tipo == "Carpetas")
                {
                    CargarCmdCarpetas();
                }
                else if (tipo == "Aplicaciones")
                {
                    CargarCmdAplicaciones();
                }
                else if (tipo == "Paginas Webs")
                {
                    CargarCmdPaginasWebs();
                }
                else
                {
                    CargarCmdInternos();
                    DataGridP.Columns[1].IsReadOnly = true;
                    DataGridP.Columns[2].IsReadOnly = true;
                }
                DataGridP.Columns[0].Visibility = Visibility.Hidden;
            }
            celda_ingresada = false;
        }
        private void BtnAyuda_Click(object sender, RoutedEventArgs e)
        {
            if (comandos == null)
            {
                comandos = new ComandosSistema() { WindowState = WindowState.Normal };
                comandos.Show();
                comandos.Closed += delegate (object a, EventArgs b)
                {
                    comandos = null;
                };
            }

        }
        private void BtnIO_Click(object sender, RoutedEventArgs e)
        {

            if (Settings.Default.ingresóIE == true)
            {
                rectangle_Exp.Visibility = Visibility.Visible;
                lblbdio.Visibility = Visibility.Visible;
                lbbdinfo.Visibility = Visibility.Visible;
                BtnExportar.Visibility = Visibility.Visible;
                BtnImportar.Visibility = Visibility.Visible;
                Settings.Default.ingresóIE = false;
                Settings.Default.Save();
            }
            else if (Settings.Default.ingresóIE == false)
            {
                rectangle_Exp.Visibility = Visibility.Hidden;
                lblbdio.Visibility = Visibility.Hidden;
                lbbdinfo.Visibility = Visibility.Hidden;
                BtnExportar.Visibility = Visibility.Hidden;
                BtnImportar.Visibility = Visibility.Hidden;
                Settings.Default.ingresóIE = true;
                Settings.Default.Save();
            }
        }
        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void BtnImportar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Base de datos de SQLite(.sqlite)| *.sqlite",
                    Title = "Cargar base de datos",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                };
                if (openFileDialog.ShowDialog().ToString().Equals("OK"))
                {
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    fileInfo.CopyTo("C:\\AVSara\\DataBase\\Data" + fileInfo.Extension, true);
                    System.Windows.MessageBox.Show("Base de datos importada con exito");
                    SystemSounds.Asterisk.Play();
                }
                openFileDialog.Dispose();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog export_bd = new SaveFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                    FileName = "Data",
                    Title = "Guardar base de datos",
                    Filter = "Base de datos de SQLite(.sqlite)|*.sqlite",
                    RestoreDirectory = true
                };
                if (export_bd.ShowDialog().ToString().Equals("OK"))
                {
                    FileInfo fileInfo = new FileInfo("C:\\AVSara\\DataBase\\Data.sqlite");
                    fileInfo.CopyTo(export_bd.FileName, true);
                    rectangle_Exp.Visibility = Visibility.Hidden;
                    lblbdio.Visibility = Visibility.Hidden;
                    lbbdinfo.Visibility = Visibility.Hidden;
                    BtnExportar.Visibility = Visibility.Hidden;
                    BtnImportar.Visibility = Visibility.Hidden;
                    SystemSounds.Asterisk.Play();
                    System.Windows.MessageBox.Show("La base de datos se ha exportado correctamente", "AV-Sara", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                export_bd.Dispose();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void BtnAvanzados_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("En algún momento habrá una característica por aquí.");
        }
        #endregion

    }
}
