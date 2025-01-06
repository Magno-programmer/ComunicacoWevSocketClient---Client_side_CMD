using System;
using System.Data;
using System.Net.WebSockets;
using System.Text;
using System.Xml.Linq;
using static ComunicacaoWebSocketClient.Connections.ListenServerClass;

try
{

    Console.WriteLine($"Connecting with web socket...");
    await Task.Delay(1000);


    //Criando o WebSocket do cliente
    using (ClientWebSocket webSocket = new ClientWebSocket())
    {
        byte[] buffer = new byte[1024];
        bool keepingloop = true;
        string response;
        string login;
        string senha;
        
        //Testando a conexão
        await webSocket.ConnectAsync(new Uri("ws://localhost:5139/"), CancellationToken.None);

        Console.WriteLine($"Connection successful");

        bool authenticated = false;

        do
        {
            // Solicita login e senha do usuário
            Console.Write("Digite o login: ");
            login = Console.ReadLine()!;
            Console.Write("Digite a senha: ");
            senha = Console.ReadLine()!;

            // Envia credenciais para o servidor no formato "login:senha"
            string credentials = $"{login}:{senha}";
            byte[] buffer2 = Encoding.UTF8.GetBytes(credentials);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer2), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("Credenciais enviadas.");

            // Aguarda resposta do servidor
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            response = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Console.WriteLine("Aguardando validação...");
            await Task.Delay(3000);
            if (response.StartsWith("Successful:"))
            {
                Console.WriteLine("Resposta do servidor: " + response.Replace("Successful: ", ""));
                authenticated = true; // Login bem-sucedido
                await Task.Delay(2000);
            }
            else
            {
                Console.WriteLine("Resposta do servidor: " + response.Replace("Failure: ", ""));
                Console.WriteLine();
                await Task.Delay(2000);
            }
        } while (!authenticated && webSocket.State == WebSocketState.Open);
        
        if (authenticated)
        {
            do
            {
                Console.Clear();
                EscolhaDoUsuario(login);
                byte[] opcaoEscolhida;
                string escolha = Console.ReadLine()!;
                Console.Clear();

                /*
                 
                 \/\/\/\/\/\/\/\/\/========================== Opções de escolha de menu ===============================\/\/\/\/\/\/\/\/\/
                 
                 */

                if (!escolha.All(char.IsDigit) || escolha == "")
                {

                    await Contagem(3, "Invalid Option", login);
                    Console.Clear();
                    continue;

                }
                else if (int.Parse(escolha) > 10 && int.Parse(escolha) < 1)
                {
                    await Contagem(3, "Invalid Option", login);
                    Console.Clear();
                    continue;

                }/*
                   vvvvvvvv----------------------------- Baixar o primeiro excel encontrado pelo seridor----------------------------vvvvv
                   */
                else if (int.Parse(escolha) == 1)
                {
                    opcaoEscolhida = Encoding.UTF8.GetBytes("Download_do_excel");
                }
                else if (int.Parse(escolha) == 2)
                {
                    opcaoEscolhida = Encoding.UTF8.GetBytes("Tamanho_do_excel");
                }
                else if(int.Parse(escolha) == 3)
                {
                    opcaoEscolhida = Encoding.UTF8.GetBytes("Pesquisa_dos_exceis_disponiveis");
                } /*
                   vvvvvvvv----------------------------- Baixar excel específico ---------------------------------------------------vvvvv
                   */
                else if(int.Parse(escolha) == 4)
                {
                    Console.Write("Digite o nome do arquivo Excel (não coloque o .xlsx): ");
                    string nomeExcel = Console.ReadLine()!;
                    opcaoEscolhida = Encoding.UTF8.GetBytes($"Baixar_Excel_especifico: {nomeExcel}");
                }
                else if (int.Parse(escolha) == 5)
                {
                    Console.Write("Digite o nome do arquivo Excel (não coloque o .xlsx): ");
                    string nomeExcel = Console.ReadLine()!;
                    opcaoEscolhida = Encoding.UTF8.GetBytes($"Tamanho_do_Excel_especifico: {nomeExcel}");
                }
                else if (int.Parse(escolha) == 6)
                {
                    Console.Write("Digite o nome do arquivo Excel (não coloque o .xlsx): ");
                    string nomeExcel = Console.ReadLine()!;
                    opcaoEscolhida = Encoding.UTF8.GetBytes($"Baixar_dataset_especifico: {nomeExcel}");
                }/*
                   vvvvvvvv----------------------------- Enviar excel para o servidor ----------------------------------------------vvvvv
                   */
                else if (int.Parse(escolha) == 7)
                {
                    await EnviarExcelParaServidor(webSocket);
                    continue;
                }/*
                   vvvvvvvv----------------------------- Receber cadastros realizados do banco -------------------------------------vvvvv
                   */
                else if (int.Parse(escolha) == 8) 
                {
                    opcaoEscolhida = Encoding.UTF8.GetBytes("Listar_todos_consumidoresPostgres:");
                }
                else if (int.Parse(escolha) == 9) 
                {
                    opcaoEscolhida = Encoding.UTF8.GetBytes("Listar_todos_consumidoresMySQL:");
                }
                else
                {
                    opcaoEscolhida = Encoding.UTF8.GetBytes("Listar_todos_consumidoresSQLite:");
                }


                /*
                 
                 /\/\/\/\/\/\/\/\/\========================== Opções de escolha de menu ===============================/\/\/\/\/\/\/\/\/\
                 
                 */

                //Send option of talk
                await webSocket.SendAsync(opcaoEscolhida, WebSocketMessageType.Text, true, CancellationToken.None);

                Task listen = ListenToServerAsync(webSocket);

                Console.WriteLine("Digite 'back' para voltar ao menu ou 'exit' para desconectar");

                while (webSocket.State == WebSocketState.Open)
                {
                    string messageTest = Console.ReadLine()!;

                    //Fecha a conexão se o tipo de mensagem for igual a mensagem de fechamento
                    if (messageTest == "back")
                    {
                        Console.WriteLine("Voltando ao menu...");
                        await Task.Delay(2000);
                        break;
                    }
                    Task dialogo = UserDialog(webSocket, messageTest);
                }

            } while (keepingloop && webSocket.State == WebSocketState.Open);
        }
    }

}
catch (Exception ex) 
{
    Console.WriteLine($"Ocorreu um erro na classe Program: {ex.Message}");
}

async Task EnviarExcelParaServidor(ClientWebSocket webSocket)
{
    Console.Write("Digite o nome do arquivo Excel (não coloque o .xlsx): ");
    string name = Console.ReadLine()!;
    Console.Write("Digite o caminho completo, considerando o arquivo Excel: ");
    string filePath = Console.ReadLine()!;

    if (File.Exists(filePath))
    {
        try
        {
            byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
            string base64Image = Convert.ToBase64String(imageBytes);
            Console.WriteLine("Enviando arquivo Excel para o servidor...");
            await UserDialog(webSocket, $"Enviar_excel_para_o_servidor {name}.xlsx:{base64Image}");
            Console.WriteLine("Arquivo Excel enviado com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler/enviar o arquivo: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Arquivo não encontrado. Verifique o caminho e tente novamente.");
    }
}


async Task Contagem(int seg, string mensagem, string nome)
{
    for (int i = seg; i > 0; i--)
    {
        Console.Clear();
        EscolhaDoUsuario(nome);
        Console.WriteLine($"{mensagem}. Retornando ao menu em: {i} second(s)...");
        await Task.Delay(1000);  // Espera 1 segundo para cada contagem
    }
}

void EscolhaDoUsuario(string nome)
{
    Console.WriteLine("Menu de Opções: " +
        "\n1 - Baixar Excel" +
        "\n2 - Ver informações do Excel" +
        "\n3 - Listar todos os Excels disponíveis no servidor" +
        "\n4 - Baixar Excel específico" +
        "\n5 - Ver tamanho de um Excel específico" +
        "\n6 - Baixar DataSet específico" +
        "\n7 - Enviar Excel para o servidor" +
        "\n8 - Listar todos os registros do banco Postgres" +
        "\n9 - Listar todos os registros do banco MySQL" +
        "\n10 - Listar todos os registros do banco SQLite" +
        $"\nBem vindo {nome}. Escolha uma das opções: ");

}

async Task UserDialog(ClientWebSocket webSocket, string messageTest)
{
    if (messageTest == "exit")
    {
        Console.WriteLine("Connection closed");

        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "CloseSent", CancellationToken.None);
    }
    else
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(messageTest);
        await webSocket.SendAsync(messageBytes, WebSocketMessageType.Text, true, CancellationToken.None);
    }

}