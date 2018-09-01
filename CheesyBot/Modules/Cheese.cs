using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CheesyBot.Modules
{
    [Group ("cheese")]
    public class Cheese : ModuleBase<SocketCommandContext>
    {
        const string path = "jokes.txt";

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

        [Command]
        public async Task TellAJokeAsync()
        {
            if (Jokes.Count == 0)
            {
                await ReplyAsync("No jokes found.");
                return;
            }

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
                await ReplyAsync($"The id {id} can't be negative.");
                return;
            }
            else if (id >= Jokes.Count)
            {
                await ReplyAsync($"Can't find a joke with that id {id}.");
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

        [Command("remove"), RequireOwner]
        public async Task RemoveAJokeAsync(params int[] ids)
        {
            ids = ids.OrderByDescending(x => x).ToArray();
            int removedJokes = 0;

            foreach (int id in ids)
            {
                if (id < 0)
                {
                    await ReplyAsync($"The id {id} can't be negative.");
                    continue;
                }
                else if (id >= Jokes.Count)
                {
                    await ReplyAsync($"Can't find a joke with that id {id}.");
                    continue;
                }

                Jokes.RemoveAt(id);
                removedJokes++;
            }

            if (removedJokes == 0)
                return;
            else
                SaveJokes();

            if (removedJokes == 1)
                await ReplyAsync("Joke removed!");
            else
                await ReplyAsync($"{removedJokes} jokes removed!");
        }

        [Command ("list")]
        public async Task ListAllJokesAsync()
        {
            if (Jokes.Count == 0)
            {
                await ReplyAsync("No jokes found.");
                return;
            }

            const string separator = ". ";
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Jokes.Count; i++)
            {
                if (builder.Length + i.ToString().Length + separator.Length + Jokes[i].Length >= 2000)
                {
                    await ReplyAsync(builder.ToString());
                    builder.Clear();
                }

                builder.Append(i);
                builder.Append(separator);
                builder.Append(Jokes[i]);

                if (i != Jokes.Count - 1)
                    builder.Append('\n');
            }

            await ReplyAsync(builder.ToString());
        }

        static List<string> LoadJokes()
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("The jokes file couldn't be found.");
                File.Create(path);
                return new List<string>();
            }

            return new List<string>(File.ReadAllLines(path));
        }

        static void SaveJokes()
        {
            File.WriteAllLines(path, Jokes);
        }
    }
}
