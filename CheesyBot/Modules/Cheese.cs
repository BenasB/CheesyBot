using Discord.Commands;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CheesyBot.Modules
{
    [Group ("cheese")]
    public class Cheese : ModuleBase<SocketCommandContext>
    {
        static string[] jokes;
        static string[] Jokes
        {
            get
            {
                if (jokes == null)
                    jokes = LoadJokes();
                return jokes;
            }
        }

        [Command]
        public async Task TellAJokeAsync()
        {
            Random rnd = new Random();
            int randomIndex = rnd.Next(Jokes.Length);
            string joke = Jokes[randomIndex];

            await ReplyAsync(joke);
        }

        [Command ("list")]
        public async Task ListAllJokesAsync()
        {
            StringBuilder list = new StringBuilder();
            for (int i = 0; i < Jokes.Length; i++)
            {
                list.Append(i+1);
                list.Append(". ");
                list.Append(Jokes[i]);

                if (i != Jokes.Length - 1)
                    list.Append('\n');
            }

            await ReplyAsync(list.ToString());
        }

        static string[] LoadJokes()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "/jokes.txt";
            return File.ReadAllLines(path);
        }
    }
}
