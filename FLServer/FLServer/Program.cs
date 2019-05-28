using System;

namespace FLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Shared.General.Gamemode.Add("Normal", "Normal gamemode");
            Shared.General.Map.Add("Tundra", "Ice cold", 0);
            Shared.Characters.Characters.CreateCharacter("Default Char", "Description", "the Undertitle",
                100, 100, 100, 100, 100, 100, 100,
                DateTime.UtcNow, 1000, 1000);
            
            Shared.Users.UserMethods.CreateNewUser("jan julius", "t");
            Shared.Users.UserMethods.CreateNewUser("wesket", "t");
            Shared.Users.UserMethods.CreateNewUser("thomaz", "t");
            Shared.Users.UserMethods.CreateNewUser("kankerhond", "t");
            
            
            var j = Shared.Users.UserMethods.GetUserByUsername("jan julius");
            var w = Shared.Users.UserMethods.GetUserByUsername("wesket");
            
            Shared.Users.UserMethods.AddFriend(j, w);
            Shared.Users.UserMethods.AddFriend(
                Shared.Users.UserMethods.GetUserByUsername("jan julius"),
                Shared.Users.UserMethods.GetUserByUsername("thomaz"));

            var c = Shared.Characters.Characters.GetCharacterByName("Default Char");
            var s = Shared.Matches.Stats.Create(100, 100, 1, 1);

            var pj = Shared.Matches.Players.Create(j, c, s);
            var sw = Shared.Matches.Stats.Create(2, 2, 2, 2);
            var pw = Shared.Matches.Players.Create(w, c, sw);

            var tj = Shared.Matches.Teams.Create(pj);
            var tw = Shared.Matches.Teams.Create(pw);


            var match = Shared.Matches.Matches.SaveMatch(new System.Collections.Generic.List<Models.Team>()
            {
                tj,tw
            },
            Shared.General.Map.GetMapById(1),
            Shared.General.Gamemode.GetGamemodeById(1),
            1,
            100);

            Shared.Users.UserMethods.AddMatch(j, match);
            Shared.Users.UserMethods.AddMatch(w, match);

            Shared.Users.UserMethods.GetMatches(j);

            LoginServer server = new LoginServer();
            server.Run();
            Console.ReadKey();
        }
    }
}
