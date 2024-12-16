using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

class Program
{
    static async Task Main(string[] args)
    {
        string token = "7584674777:AAE4xg02qNIP8Z_8XJrnNNqWx47CZ2MI81I";
        var botClient = new TelegramBotClient(token);

        var botInfo = await botClient.GetMeAsync();
        Console.WriteLine($"{botInfo.Username} бот иштеп жатат");

        using var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token
        );
        
        Console.WriteLine("Каалаган кнопканы басыныз, программаны токтотуу учун");
        Console.ReadKey();
        cts.Cancel();
    }

    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message || message.Text is not { } messageText)
        {
            return;
        }

        var chatId = message.Chat.Id;
        Console.WriteLine($"Кабар келди: {messageText},  чат: {chatId}");

        await botClient.SendTextMessageAsync(chatId: chatId, text: $"Сиз жаздыныз: {messageText}",
            cancellationToken: cancellationToken);
    }

    static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка чыкты: {exception.Message}");
        return Task.CompletedTask;
    }
}