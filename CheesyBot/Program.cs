namespace CheesyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordBot bot = new DiscordBot();
            bot.RunBotAsync().GetAwaiter().GetResult();
        }
    }
}
