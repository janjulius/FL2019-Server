using System;

namespace FLServer
{
    class Program
    {

        static void Main(string[] args)
        {
            //Shared.General.Gamemode.Add("Normal", "Normal gamemode");
            //Shared.General.Map.Add("Tundra", "Ice cold", 0);
            //Shared.Characters.Characters.CreateCharacter("Default Char", "Description", "the Undertitle",
            //    100, 100, 100, 100, 100, 100, 100,
            //    DateTime.UtcNow, 1000, 1000);
            //Shared.Characters.Characters.CreateCharacter("Berend", "Centuries back, all the way back to the ice age, that’s the time Berend remembers as his childhood. Back then Berend was all alone, he has no clue where he came from really.. The legend goes he just woke up one day and instinctively felt responsible for the eradication of any non-yeti creature. Whether this is true is of course uncertain, as no one has ever been able to hold a decent conversation with the creature. There have been reports of people claiming they heard Berend talk, he has a deep and dark voice and his “talking” is suspiciously close to growling. During the centuries that Berend lived, he smashed up countless foes and he can now be considered an absolute MASTER at clobberin’. when a common human fails to avoid Berend’s club they will surely die within an instant. During Berend’s second century he met Geraldine, a female yeti that was slightly smaller in size than Berend.Geraldine absolutely loved throwing rocks, and she had a real talent for it! One day Berend and Geraldine were playing a prehistoric version of baseball with just two players and unevenly shaped rocks instead of the modern baseballs.When Berend’s club slightly slipped, the rock was launched right at Geraldine’s head, and she died to the slightly pointy stone upon impact. Berend was more than qualified to join the Thwamp roster based on his sheer hate for other creatures, but the loss of the love of his life by his own hands drove him absolutely insane and made him one of Thwamp’s scariest and deadliest assets.",
            //    "The terrifying yeti", 115, 65, 140, 40, 125, 175, 110, DateTime.Now, 3000, 500);
            //Shared.Characters.Characters.CreateCharacter("Gordo", "Gordo was the third of four children in his golem family, he absolutely despises his two older sisters but he got along really well with his little brother, Graha. During his early years Gordo always spent most of his time hanging out with Graha, always up to no good those two. Decades passed and young golems were supposed to develop a certain magical affinity around this point in their life.The young Gordo had a strong affinity for the elements, but his little brother wasn’t as lucky, years passed but the young Graha stayed incapable of doing anything magical.Not having any magical powers was very rare for golems and so Graha became an outcast. Not only Gordo’s sisters and parents made fun of Graha, golems from everywhere travelled to come make fun of him.But Gordo wouldn’t stand for that, every golem that came with evil intent was opposed by Gordo.Although Gordo at first couldn’t do much, he grew stronger every day from fending off bad golems for his brother and eventually not a single golem stood a chance against him anymore.By the time Gordo was too strong to be opposed his view of living being other than him and Graha had decayed to the point he would mercilessly murder any being that had the audacity to come within his eyesight. Gordo’s boundless magical power easily qualifies him to be part of the Thwamp roster.",
            //    "The tiny master of evil", 200, 75, 75, 60, 500, 75, 90, DateTime.Now, 3000, 500);
            //Shared.Characters.Characters.CreateCharacter("Grackle", "Grackle is a dragon from one of the latest generations, after all these years the amount of dragons kept decreasing to the point that the dragons had a lot of trouble finding dragons that weren’t part of the family. Grackle wasn’t born as a normal dragon thanks to these circumstances, he was born as a skinny dragon without wings. Grackle’s only dragon-like trait was that he could breathe fire. When Grackle was born, the doctor threw him out and told his parents they unfortunately had a miscarriage(which wasn’t a rare occurrence at this point in time for dragons).Grackle somehow manages to survive as an infant out in the wilds and grew to be a strong sub - dragon, when he got older he decided to go and look for his parents, to tell them that he grew strong and to ask them for forgiveness for being the way he is. When Grackle finally managed to find his parents he visited them, he told them that he was their son but they wouldn’t listen. They denied that he was their son and told him to leave immediately because they were revolted by his disgusting sub - dragon body.When Grackle stood strong and insisted that he was truly their offspring, they attacked him and chased him off into the horizon. Traumatized by this horrible experience, Grackle had nowhere to go to anymore. Grackle decided to join Thwamp’s roster because he saw this as the only place where they would accept him.",
            //    "The failed dragon", 100, 125, 125, 125, 75, 150, 90, DateTime.Now, 3000, 500);
            //Shared.Characters.Characters.CreateCharacter("Goatman", "First off, let’s clear up some confusion, Goatman is not actually a goat. Goatman gets very tense when you call him a goat, because he is a gazelle. “I’m not a goat! >:(“ -goatman 2019 Goatman was born as an average gezelle, just a gazelle galloping out in the savannah together with his fellow gazelles.For many years he lived like a normal gazelle, there were times where he nearly lost his life to a lion, and times when he lost one of his friends to the claws of a beast, but overall it was a rather peaceful gazelle - life.But then… the scientist came.Goatman was abducted by the scientist, for years they used him for special tests. Besides the fact that being locked up and used as a lab - animal was traumatizing for goatman, the side effects from the tests were excruciating. Every day goatman went through unbearable pain, until one day, Goatman was subject to a test that did the impossible, it gave him a human consciousness, the ability to talk and even his front legs mutated into arms. After this occurred, goatman went completely berserk and smashed up the entire facility murdering every single scientist inside. When he went back to go and see if anyone from his family still remained in his old home, he found out that his old home was wiped and there was now an industrial facility in its place. When Goatman found out that his home was demolished, he had nowhere to go to. He pondered where to go for a while but ultimately decided that he wanted to devote the rest of his life to Thwamp, where he was unfortunately nicknamed Goatman.",
            //    "The frustrated gazelle", 80, 150, 80, 150, 100, 100, 100, DateTime.Now, 3000, 500);
            //Shared.Characters.Characters.CreateCharacter("Sir Endur", "Faceless entities are a big question mark to the rest of the world, no one really knows where they come from, what they want or even what they really do. One day they started appearing, people from all over the world noticed them walking around the streets and slightly panicked. All the world leaders came together and decided they needed to contact these Faceless, try to communicate and find out what they wanted. Unfortunately this failed miserably, no one could figure out how to communicate with these Faceless because they didn’t in any way cooperate with the humans. The Faceless don’t ignore the fact that humans exist, they never walk into people or bother them in any other way, but they, for unknown reasons, refuse to communicate their intentions to the humans. Clearly, the people of earth wanted to rid themselves of these entities as there was no way to figure out whether they had bad intentions or not.But when they tried to attack one, they stood no chance.mortified by the things they decided to leave them be. Sir Endur is the only Faceless known to mankind that ever tried to communicate with the humans, the top linguists of the world have gathered to hear him out and try to figure out what exactly he is trying to say.Together they worked out what he wanted to tell them, they concluded he just wanted to share his name; Sir Endur wiglenks. No one knows how he ended up being part of the Thwamp roster.",
            //    "The unpredictable faceless", 100, 100, 100, 100, 75, 100, 100, DateTime.Now, 3000, 500);
            //Shared.Characters.Characters.CreateCharacter("Kaarl", "Lizarlops are a very rare species of immortal creatures that roam the steppes around the world. the lizarlops are cowardly creatures, they’d back off for anyone bigger than an insect. Luckily, they are also very fast, much like raptors they can reach a ridiculous speed in time of need and this comes in very handy every time they run away from yet another stranger. Kaarl is a lizarlops, a rather lonely one. One evening Kaarl was roaming the steppes and ran into a campfire with delicious spit - roasted chicken hanging above it.Kaarl was tempted by the delicious smell, as he hadn’t eaten anything other than a few worms for the past month.When he was about to push the chicken breast over and feast on it, he noticed a human sitting on a log close to the fire.Kaarl panicked, he shrieked out a loud “GREEFRGLARG”. But to Kaarl’s surprise, the human did nothing suspicious.He just sat there, making gestures trying to calm Kaarl down while letting out a comforting “Oooh, calm down boy”. Kaarl spent the rest of the night with this man and decided the man had no evil intentions. Years went by spent with this human fast, after so many years Kaarl finally felt safe, he finally wasn’t lonely anymore.Together they could defeat any foe, they could do anything as long as they were together as a team. Kaarl was betrayed.He was sold to Thwamp for a great sum of money, the human turned out to be a homeless man that was banned from the city for his many crimes.The years they spent together, the man was just trying to find a way back into the city so he could sell the creature and start a new life with the money he’d raise.",
            //    "The cowardly lizarlops", 80, 150, 75, 150, 50, 60, 120, DateTime.Now, 3000, 500);

            //Shared.Users.UserMethods.CreateNewUser("jan julius", "t");
            //Shared.Users.UserMethods.CreateNewUser("wesket", "t");
            //Shared.Users.UserMethods.CreateNewUser("thomaz", "t");
            //Shared.Users.UserMethods.CreateNewUser("kankerhond", "t");
            //
            //
            //var j = Shared.Users.UserMethods.GetUserByUsername("jan julius");
            //var w = Shared.Users.UserMethods.GetUserByUsername("wesket");
            //
            //Shared.Users.UserMethods.AddFriend(j, w);
            //Shared.Users.UserMethods.AddFriend(
            //    Shared.Users.UserMethods.GetUserByUsername("jan julius"),
            //    Shared.Users.UserMethods.GetUserByUsername("thomaz"));
            //
            //var c = Shared.Characters.Characters.GetCharacterByName("Default Char");
            //var s = Shared.Matches.Stats.Create(100, 100, 1, 1);
            //
            //var pj = Shared.Matches.Players.Create(j, c, s);
            //var sw = Shared.Matches.Stats.Create(2, 2, 2, 2);
            //var pw = Shared.Matches.Players.Create(w, c, sw);
            //
            //var tj = Shared.Matches.Teams.Create(pj);
            //var tw = Shared.Matches.Teams.Create(pw);
            //
            //
            //var match = Shared.Matches.Matches.SaveMatch(new System.Collections.Generic.List<Models.Team>()
            //{
            //    tj,tw
            //},
            //Shared.General.Map.GetMapById(1),
            //Shared.General.Gamemode.GetGamemodeById(1),
            //1,
            //100);
            //
            //Shared.Users.UserMethods.AddMatch(j, match);
            //Shared.Users.UserMethods.AddMatch(w, match);
            //
            //Shared.Users.UserMethods.GetMatches(j);

            LoginServer server = new LoginServer();
            server.Run();
            Console.ReadKey();
        }
    }
}
