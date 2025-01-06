# Documentação

## Introdução

O projeto **ComunicacoWevSocketClient---Client_side_CMD** é um cliente desenvolvido em C# para linha de comando (CMD) que utiliza comunicação em tempo real via WebSocket. Este projeto exemplifica como criar uma aplicação cliente leve e eficiente para interagir com um servidor WebSocket, enviando e recebendo mensagens de forma bidirecional.

---

## Objetivos

- **Comunicação em Tempo Real:** Demonstrar a implementação de um cliente WebSocket capaz de enviar e receber mensagens em tempo real.
- **Simplicidade e Eficiência:** Criar uma aplicação cliente minimalista que pode ser executada diretamente no terminal.
- **Integração Cliente-Servidor:** Estabelecer um fluxo de comunicação consistente com servidores WebSocket.

---

## Configuração do Ambiente

### Requisitos Pré-Instalação
- **SDK .NET**: Versão 5.0 ou superior.
- **Editor de Texto/IDE**: Visual Studio ou Visual Studio Code.
- **Servidor WebSocket**: Deve estar configurado e em execução para testar o cliente.

### Passos para Configuração
1. **Clonar o Repositório**
   ```bash
   git clone https://github.com/Magno-programmer/ComunicacoWevSocketClient---Client_side_CMD.git
   ```
2. **Navegar para o Diretório do Projeto**
   ```bash
   cd ComunicacoWevSocketClient---Client_side_CMD
   ```
3. **Restaurar Dependências**
   ```bash
   dotnet restore
   ```
4. **Compilar o Projeto**
   ```bash
   dotnet build
   ```
5. **Executar o Cliente**
   ```bash
   dotnet run
   ```

---

## Estrutura do Projeto

### Diretórios Principais

- **`Services/`**: Inclui a lógica de comunicação WebSocket.
- **`Utils/`**: Contém funções auxiliares para manipulação de mensagens e tratamento de exceções.

### Arquivos Chave
- **`Program.cs`**: Arquivo principal que inicializa a aplicação e gerencia o fluxo de execução.
- **`WebSocketClientService.cs`**: Classe responsável pela implementação da lógica de comunicação com o servidor WebSocket.

---

## Funcionalidades

### Estabelecimento de Conexão
- Conecta-se a um servidor WebSocket utilizando a classe `ClientWebSocket`.
- Garante a reconexão automática em caso de falha temporária.

### Envio de Mensagens
- Permite que o usuário envie mensagens ao servidor digitando comandos no terminal.
- Suporte para envio de mensagens em diferentes formatos, como texto simples ou JSON.

### Recebimento de Mensagens
- Recebe mensagens do servidor e exibe no terminal.
- Atualiza dinamicamente o fluxo de mensagens recebidas.

### Tratamento de Erros
- Captura exceções relacionadas à conexão ou mensagens malformadas.
- Exibe mensagens de erro claras para facilitar o diagnóstico.

### Logs de Atividades
- Gera logs detalhados para monitorar o status da conexão e o fluxo de mensagens enviadas e recebidas.

---

## Exemplos de Uso

1. **Conectar ao Servidor**:
   - Ao executar o programa, o cliente tenta estabelecer uma conexão com o servidor configurado.

2. **Enviar Mensagem ao Servidor**:
   - No terminal, digite a mensagem que deseja enviar ao servidor e pressione Enter.

3. **Receber Mensagens do Servidor**:
   - As mensagens enviadas pelo servidor serão exibidas automaticamente no terminal.

4. **Erro de Conexão**:
   - Caso o servidor não esteja acessível, uma mensagem de erro será exibida no terminal.

---

## Possíveis Melhorias

1. **Autenticação**:
   - Adicionar suporte para autenticação baseada em token (ex.: JWT).

2. **Interface Interativa**:
   - Criar uma interface gráfica para melhorar a experiência do usuário.

3. **Tratamento Avançado de Erros**:
   - Implementar mensagens de erro mais detalhadas para problemas complexos.

4. **Testes Automatizados**:
   - Criar testes unitários para validar o comportamento do cliente.

---

## Conclusão

O projeto **ComunicacoWevSocketClient---Client_side_CMD** demonstra como criar um cliente leve e eficiente para comunicação em tempo real via WebSocket. Ele é uma base sólida para soluções mais complexas e pode ser facilmente adaptado para diversas aplicações que requerem interação em tempo real.

**Agradecimentos:** Este projeto foi desenvolvido para explorar a implementação de comunicação cliente-servidor utilizando WebSocket em aplicações de linha de comando.  

