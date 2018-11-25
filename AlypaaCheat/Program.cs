using Fiddler;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AlypaaCheat
{
    class Program
    {
        static Game[] games;
        static int gameIndex = 0;
        static void Main(string[] args)
        {
            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
            var settingsBuilder = new FiddlerCoreStartupSettingsBuilder();
            settingsBuilder.ListenOnPort(8889).RegisterAsSystemProxy().DecryptSSL();
            FiddlerApplication.Startup(settingsBuilder.Build());

            while (true)
            {
                var key = Console.ReadKey(true);
                if(key.Key == ConsoleKey.LeftArrow)
                {
                    if(gameIndex > 0)
                    {
                        ShowGame(gameIndex - 1);
                    }
                }
                else if(key.Key == ConsoleKey.RightArrow)
                {
                    if(gameIndex < games.Length - 1)
                    {
                        ShowGame(gameIndex + 1);
                    }
                }
                else if(key.Key == ConsoleKey.X && key.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    break;
                }
            }

            FiddlerApplication.AfterSessionComplete -= FiddlerApplication_AfterSessionComplete;

            if (FiddlerApplication.IsStarted())
            {
                FiddlerApplication.Shutdown();
            }
        }
        static void ShowGame(int index)
        {
            Console.Clear();
            gameIndex = index;
            var game = games[gameIndex];
            var maxLength = game.Questions.Max(q => q.Text.Length);
            foreach(var question in game.Questions)
            {
                var answer = question.Options[question.CorrectIndex];
                Console.Write(question.Text);
                Console.CursorLeft = maxLength + 2;
                Console.WriteLine(answer);
            }
        }
        private static void FiddlerApplication_AfterSessionComplete(Session oSession)
        {
            if (oSession.fullUrl.Contains("alypaa.com/api/quiz/questions/"))
            {
                var body = oSession.GetResponseBodyAsString();
                games = JsonConvert.DeserializeObject<Game[]>(body);
                ShowGame(0);
            }
        }
    }
    class Game
    {
        public Question[] Questions { get; set; }
    }
    class Question
    {
        [JsonProperty("answer")]
        public string[] Options { get; set; }
        [JsonProperty("correct")]
        public int CorrectIndex { get; set; }
        [JsonProperty("question")]
        public string Text { get; set; }
    }
}
