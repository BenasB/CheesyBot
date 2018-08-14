using System.IO;

namespace CheesyBot
{
    class Auth
    {
        public static string GetBotToken()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "/token.txt";
            var lines = File.ReadAllLines(path);

            return lines[0];
        }
    }
}
