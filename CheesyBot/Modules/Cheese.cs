using Discord.Commands;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CheesyBot.Modules
{
    [Group ("cheese")]
    public class Cheese : ModuleBase<SocketCommandContext>
    {
        static readonly string path;

        static List<string> jokes;
        static List<string> Jokes
        {
            get
            {
                if (jokes == null)
                    jokes = LoadJokes();
                return jokes;
            }
        }

        static Cheese()
        {
            path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "/jokes.txt";
        }

        [Command]
        public async Task TellAJokeAsync()
        {
            Random rnd = new Random();
            int randomIndex = rnd.Next(Jokes.Count);
            string joke = Jokes[randomIndex];

            await ReplyAsync(joke);
        }

        [Command]
        public async Task TellAJokeAsync(int id)
        {
            if (id < 0)
            {
                await ReplyAsync("The id can't be negative");
                return;
            }
            else if (id >= Jokes.Count)
            {
                await ReplyAsync("Can't find a joke with that id");
                return;
            }

            string joke = Jokes[id];

            await ReplyAsync(joke);
        }

        [Command ("add"), RequireOwner]
        public async Task AddAJokeAsync(params string[] newJokes)
        {
            foreach (string joke in newJokes)
                Jokes.Add(joke);

            SaveJokes();
            if (newJokes.Length == 1)
                await ReplyAsync("Joke added!");
            else
                await ReplyAsync("Jokes added!");
        }

        [Command ("list")]
        public async Task ListAllJokesAsync()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Jokes.Count; i++)
            {
                if (builder.Length + 3 + Jokes[i].Length >= 2000)
                {
                    await ReplyAsync(builder.ToString());
                    builder.Clear();
                }

                builder.Append(i);
                builder.Append(". ");
                builder.Append(Jokes[i]);

                if (i != Jokes.Count - 1)
                    builder.Append('\n');
            }

            await ReplyAsync(builder.ToString());
        }

        static List<string> LoadJokes()
        {         
            return new List<string>(File.ReadAllLines(path));
        }

        static void SaveJokes()
        {
            File.WriteAllLines(path, Jokes);
        }
    }
}
