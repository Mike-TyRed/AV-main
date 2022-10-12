using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using BibliotecaAsistente;

namespace AsistenteVirtualSTABLE
{
    public partial class ControlSerial : Window
    {
        #region Variables
        private string strBufferOut;
        SerialPort SpPuertos;
        SQLiteConnection conection;
        DataSet ds;
        SQLiteDataAdapter adap;
        SQLiteCommandBuilder cmdb;
        Asistente ia = new Asistente();
        #endregion
        #region Metodo Main
        public ControlSerial()
        {
            InitializeComponent();
            ia.verificarpuertoconectado.Tick += Verificarpuertos_Tick;
            ia.verificarpuertodesconectado.Tick += Verificarpuertodesconectado_Tick;
            ia.verificarpuertoconectado.Start();
            Refresh_ports();
            MostrarDatos();
            Ocultar_Campos();
            IdiomaEspañol();
        }
        #endregion
        #region Metodos
        void Refresh_ports()
        {
            string[] Puertos_Disponibles = SerialPort.GetPortNames();
            cb_ports.Items.Clear();

            foreach (string puerto_simple in Puertos_Disponibles)
            {
                cb_ports.Items.Add(puerto_simple);
            }
            if (cb_ports.Items.Count > 0)
            {
                cb_ports.SelectedIndex = 0;
                Btn_conect.IsEnabled = true;
                ia.verificarpuertoconectado.Stop();
                ia.verificarpuertodesconectado.Start();
            }
            else
            {
                cb_ports.Text = "         ";
                strBufferOut = string.Empty;
                Btn_conect.IsEnabled = false;
                Btn_send_Data.IsEnabled = false;
                ia.verificarpuertodesconectado.Stop();
                ia.verificarpuertoconectado.Start();
            }
           
        }
        void Ocultar_Campos()
        {
            txt_puerto.Visibility = Visibility.Hidden;
            txt_comando.Visibility = Visibility.Hidden;
            txt_acción.Visibility = Visibility.Hidden;
            txt_respuesta.Visibility = Visibility.Hidden;
            rect_fondo_add.Visibility = Visibility.Hidden;
        }
        void Mostrar_Campos()
        {
            txt_puerto.Visibility = Visibility.Visible;
            txt_comando.Visibility = Visibility.Visible;
            txt_acción.Visibility = Visibility.Visible;
            txt_respuesta.Visibility = Visibility.Visible;
            rect_fondo_add.Visibility = Visibility.Visible;
        }
        void Agregar_Comandos_BD()
        {
            try
            {
                if (txt_puerto.Text != string.Empty && txt_comando.Text != string.Empty && txt_acción.Text != string.Empty)
                {
                    conection = new SQLiteConnection(Properties.Settings.Default.ConexionBD);
                    conection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(string.Format("INSERT INTO CmdPuertoSerial (PuertoCOM, Comando, Accion, Respuesta) values('{0}','{1}','{2}','{3}')", txt_puerto.Text, txt_comando.Text, txt_acción.Text, txt_respuesta.Text), conection);
                    cmd.ExecuteNonQuery();
                    txt_comando.Text = string.Empty;
                    txt_acción.Text = string.Empty;
                    txt_respuesta.Text = string.Empty;
                }
                else
                {
                    System.Windows.MessageBox.Show("No se puede agregar comandos vacíos o incompletos", "Error AVSARA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Ha ocurrido un problema", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        void MostrarDatos()
        {
            try
            {
                conection = new SQLiteConnection("Data Source = |DataDirectory|\\DataBase\\Data.sqlite; Version = 3");
                conection.Open();

                adap = new SQLiteDataAdapter("SELECT ID,PuertoCOM,Comando, Accion, Respuesta FROM CmdPuertoSerial", conection);
                ds = new DataSet();
                adap.Fill(ds);
                DatagridBD.ItemsSource = ds.Tables[0].DefaultView;
                conection.Close();
            }
            catch
            {
                System.Windows.MessageBox.Show("Se ha encontrado un error en la base de datos", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                conection.Close();
            }
        }
        void OcultarColumnas()
        {
            DatagridBD.Columns[0].Visibility = Visibility.Hidden;
        }
        void IdiomaEspañol()
        {
            lblTitle.Content = "CONTROLADOR DE PUERTO SERIAL";
            Btn_conect.Content = "Conectar";
            Btn_send_Data.Content = "Enviar Dato";
            cb_ports.ToolTip = "Puerto COM";
            tbx_BaudRate.ToolTip = "Baudio";
            txt_date.ToolTip = "Escribe un dato de prueba para enviar al Arduino";
            lblPuertoCOM.Content = "PUERTO";
            lblcomando.Content = "COMANDO+";
            lblaccion.Content = "ACCIÓN+";
            lblRespuesta.Content = "RESPUESTA+";
            Btn_add_comandos.Content = "Nuevo Comando";
            Btn_delet_cmd.Content = "Eliminar";
        }
        #endregion
        #region Eventos Internos
        private void Btn_conect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Btn_conect.Content.Equals("Conectar"))
                {
                    SpPuertos = new SerialPort(cb_ports.Items[0].ToString())
                    {
                        BaudRate = Int32.Parse(tbx_BaudRate.Text),
                        DataBits = 8,
                        Parity = Parity.None,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        PortName = cb_ports.Text
                    };


                    try
                    {
                        SpPuertos.Open();
                        Btn_conect.Content = "Desconectar";
                        Btn_conect.ToolTip = "Conectado";
                        Btn_send_Data.IsEnabled = true;
                    }
                    catch (FormatException ex)
                    {

                        System.Windows.MessageBox.Show(ex.Message);
                    }
                }
                else if (Btn_conect.Content.Equals("Desconectar"))
                {
                    SpPuertos.Close();
                    Btn_conect.Content = "Conectar";
                    Btn_conect.ToolTip = "Sin conexión";
                    Btn_send_Data.IsEnabled = false;
                }
            }
            catch (FormatException ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void Btn_send_Data_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SpPuertos.DiscardOutBuffer();
                strBufferOut = txt_date.Text;
                SpPuertos.Write(strBufferOut);
            }
            catch (FormatException ex)
            {

                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void Btn_add_comandos_Click(object sender, RoutedEventArgs e)
        {
            if (Btn_add_comandos.Content.Equals("Nuevo Comando"))
            {
                Mostrar_Campos();
                Btn_add_comandos.Content = "Agregar";
            }
            else if (Btn_add_comandos.Content.Equals("Agregar"))
            {
                Agregar_Comandos_BD();
                cmdb = new SQLiteCommandBuilder(adap);
                adap.Update(ds);
                MostrarDatos();
                OcultarColumnas();
                Ocultar_Campos();
                Btn_add_comandos.Content = "Nuevo Comando";
            }
        }
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            strBufferOut = string.Empty;
            Btn_conect.IsEnabled = false;
            Btn_send_Data.IsEnabled = false;
            OcultarColumnas();
        }
        private void DatagridBD_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Btn_delet_cmd.Visibility = Visibility.Visible;
        }
        private void Btn_delet_cmd_Click(object sender, RoutedEventArgs e)
        {
            List<DataRow> theRows = new List<DataRow>();
            for (int i = 0; i < DatagridBD.SelectedItems.Count; ++i)
            {
                Object o = DatagridBD.SelectedItems[i];
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

            cmdb = new SQLiteCommandBuilder(adap);
            adap.Update(ds);

            OcultarColumnas();
        }
        private void Verificarpuertodesconectado_Tick(object sender, EventArgs e)
        {
            Refresh_ports();
        }
        private void Verificarpuertos_Tick(object sender, EventArgs e)
        {
            Refresh_ports();
        }
        private void DatagridBD_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DatagridBD.UnselectAll();
            Btn_delet_cmd.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
