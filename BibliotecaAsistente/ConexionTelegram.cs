using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BibliotecaAsistente
{
   public class ConexionTelegram : Gramaticas
    {
        private static readonly TelegramBotClient cliente = new TelegramBotClient("667898017:AAE7_Es7A2ObqXregTtqhSt-4CpjrHz4D04");
        public ConexionTelegram()
        {
            cliente.StartReceiving();
        }
        
        public void Escucha()
        {
            cliente.OnMessage += Cliente_OnMessage;
        }

        private void Cliente_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string resultado = EjecutarComandosBD(e.Message.Text.ToLower());

            if (resultado != "")
            {
                cliente.SendTextMessageAsync(e.Message.Chat.Id, resultado);
            }
            else
            {
                cliente.SendTextMessageAsync(e.Message.Chat.Id, "El comando no existe");
            }
        }
    }
}
