using ComunicacaoWebSocketClient.Models;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.IO;

namespace ComunicacaoWebSocketClient.Connections;

internal class ListenServerClass
{
    public static async Task ListenToServerAsync(ClientWebSocket client)
    {
        byte[] buffer = new byte[2048 * 1024]; // Buffer grande para suportar mensagens extensas
        try
        {
            while (client.State == WebSocketState.Open)
            {
                var messageReceived = new ArraySegment<byte>(buffer);
                var result = await client.ReceiveAsync(messageReceived, CancellationToken.None);

                // Converte os bytes recebidos para uma string
                string serverMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

                if (serverMessage.StartsWith("JSON: "))
                {
                    // Trata mensagens JSON
                    ProcessJsonMessage(serverMessage.Replace("JSON: ", ""));
                }
                else if (serverMessage.StartsWith("Excel: "))
                {
                    // Exibe mensagens de texto simples
                    Console.WriteLine(serverMessage.Replace("Excel: ", ""));
                }
                else
                {
                    // Exibe mensagens de texto simples
                    Console.WriteLine(serverMessage);
                }
            }
        }
        catch (WebSocketException wex)
        {
            Console.WriteLine($"Ocorreu um erro no WebSocket ListenServerClass: {wex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro no ListenServerClass: {ex.Message}");
        }
    }

    // Método para processar mensagens JSON
    private static void ProcessJsonMessage(string jsonMessage)
    {
        try
        {
            List<Consumidor> consumidores = JsonSerializer.Deserialize<List<Consumidor>>(jsonMessage) ?? new List<Consumidor>();

            if (consumidores.Any())
            {
                StringBuilder escritaConsumidor = new StringBuilder();

                foreach (var consumidor in consumidores)
                {
                    escritaConsumidor.AppendLine($"\nDados do consumidor {consumidor.id_consumidor} recebidos:");
                    escritaConsumidor.AppendLine($"Nome: {consumidor.nm_consumidor}");
                    escritaConsumidor.AppendLine($"Documento: {consumidor.nr_documento}");
                    escritaConsumidor.AppendLine($"Tipo de Documento: {consumidor.id_tipo_documento}");
                    escritaConsumidor.AppendLine($"Email: {consumidor.ds_email}");
                    escritaConsumidor.AppendLine($"Celular: {consumidor.nr_celular}");
                    escritaConsumidor.AppendLine($"CRM: {consumidor.fl_crm}");
                    escritaConsumidor.AppendLine($"SMS: {consumidor.fl_sms}");
                    escritaConsumidor.AppendLine($"Email Marketing: {consumidor.fl_email}");
                    escritaConsumidor.AppendLine($"CEP: {consumidor.nr_cep}");
                    escritaConsumidor.AppendLine($"Endereço: {consumidor.ds_endereco}");
                    escritaConsumidor.AppendLine($"Bairro: {consumidor.ds_bairro}");
                    escritaConsumidor.AppendLine($"Cidade: {consumidor.nm_cidade}");
                    escritaConsumidor.AppendLine($"UF: {consumidor.sg_uf}");
                    escritaConsumidor.AppendLine($"Dia de Aniversário: {consumidor.nr_dia_aniversario}");
                    escritaConsumidor.AppendLine($"Mês de Aniversário: {consumidor.nr_mes_aniversario}");
                }

                // Exibe todas as informações do consumidor de uma vez só
                Console.WriteLine(escritaConsumidor.ToString());
            }
            else
            {
                Console.WriteLine("Não possui consumidores cadastrados");
            }
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Erro ao desserializar JSON: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao processar a mensagem JSON: {ex.Message}");
        }
    }

}
