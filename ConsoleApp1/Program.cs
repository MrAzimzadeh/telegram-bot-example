using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static readonly string Token = "*";
    private static TelegramBotClient botClient;

    static async Task Main(string[] args)
    {
        botClient = new TelegramBotClient(Token);

        var me = await botClient.GetMe();
        Console.WriteLine($"Bot id: {me.Id}, Bot Name: {me.FirstName}");

        using var cts = new CancellationTokenSource();

        // Start receiving updates in a background task
        Task.Run(() =>
        {
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                (client, exception, arg3) => null,
                cancellationToken: cts.Token
            );
        });

        Console.WriteLine("Bot is running. Type 'exit' to stop.");

        // Keep the console application running
        while (true)
        {
            var input = Console.ReadLine();
            if (input?.ToLower() == "exit")
            {
                cts.Cancel();
                break;
            }
        }
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            // Log the received message
            Console.WriteLine($"Telegram message received: {messageText}");

            // Prompt for a reply in the console
            Console.Write("Enter your reply: ");
            string? replyText = Console.ReadLine();

           await botClient.SendMessage(chatId: chatId,
                text: replyText ?? "salam"
            );
        }
    }
}
// Send the reply back to Telegram