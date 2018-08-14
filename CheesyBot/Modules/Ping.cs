using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace CheesyBot.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command ("ping")]
        public async Task PingAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Pong!")
                .WithColor(Color.Green);

            await ReplyAsync("", false, builder.Build());
        }
    }
}
