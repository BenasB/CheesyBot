using System;
using System.IO;

namespace CheesyBot
{
    class Auth
    {
        const string path = "token.txt";

        public static string GetBotToken()
        {            
            if (!File.Exists(path))
            {
                Console.WriteLine("The token file couldn't be found.");
                File.Create(path);
                return null;
            }

            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
            {
                Console.WriteLine("The token could not be found.");
                return null;
            }

            return lines[0];
        }
    }
}
