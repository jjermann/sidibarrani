using System;
using System.Linq;
using SidiBarraniCommon.Model;
using SidiBarraniServer;
using SidiBarraniClient;
using System.Collections.Generic;
using System.Threading;

namespace SidiBaraniConsole
{
    public class Program
    {
        static void Main()
        {
            var sidiBarraniServer = new SidiBarraniServerImplementation();

            // The following lines should be replaced by actual web service calls
            var client1 = new SidiBarraniClientImplementation
            {
                SidiBarraniServerApi = sidiBarraniServer
            };
            var client2 = new SidiBarraniClientImplementation
            {
                SidiBarraniServerApi = sidiBarraniServer
            };
            var client3 = new SidiBarraniClientImplementation
            {
                SidiBarraniServerApi = sidiBarraniServer
            };
            var client4 = new SidiBarraniClientImplementation
            {
                SidiBarraniServerApi = sidiBarraniServer
            };

            var rules = new Rules
            {
                EndScore = 100
            };
            client1.OpenGame(rules, "Game1", "TeamA1", "TeamA2");
            client2.OpenGame(null, "Game2", "TeamB1", "TeamB2");
            client3.RefreshOpenGames();
            client4.RefreshOpenGames();
            client1.ConnectToGame(client1.OpenGameList.First(), "Player1");
            client2.ConnectToGame(client2.OpenGameList.First(), "Player2");
            client3.ConnectToGame(client3.OpenGameList.First(), "Player3");
            client4.ConnectToGame(client4.OpenGameList.First(), "Player4");
            client1.StartGame();
            Thread.Sleep(500);
            var clientList = new List<SidiBarraniClientImplementation> {client1, client2, client3, client4};

            var random = new Random();
            var actionCount = 0;
            while (actionCount < 10)
            {
                var oldActionCount = actionCount;
                foreach (var client in clientList)
                {
                    var validActions = client.GetValidActions();
                    if (validActions.Any())
                    {
                        var randomAction = validActions[random.Next(validActions.Count-1)];
                        if (client.ProcessAction(randomAction))
                        {
                            actionCount++;
                            Thread.Sleep(500);
                        }
                    }
                }
                if (oldActionCount == actionCount)
                {
                    break;
                }
            }
        }
    }
}