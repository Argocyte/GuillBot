using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using RealmeyeSharp;
using System.Linq;
using System.Collections.ObjectModel;

namespace GuillBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        #region Help
        [Command("help")]
        public async Task HelpAsync()
        {
            Generic(Color.Blue, "here's some help!",
                $"{Config.bot.cmdPrefix}Setup <true/false> <your guild name>",
                $"{Config.bot.cmdPrefix}Assign <your realm name>",
                $"{Config.bot.cmdPrefix}Player <player>",
                $"{Config.bot.cmdPrefix}Player Characters <player>",
                $"{Config.bot.cmdPrefix}Player description <player>",
                $"{Config.bot.cmdPrefix}Player pet <Player>",
                $"{Config.bot.cmdPrefix}Player Character <character number> <player>",
                $"{Config.bot.cmdPrefix}Guild <guild name>",
                $"{Config.bot.cmdPrefix}Guild Members <guild name>",
                $"{Config.bot.cmdPrefix}FindKey <key>",
                $"{Config.bot.cmdPrefix}FindClover",
                $"{Config.bot.cmdPrefix}FindBackpack",
                $"{Config.bot.cmdPrefix}Call <dungeon name> <server> <location> <true/false>",
                $"{Config.bot.cmdPrefix}Subscribe Daily <true/false> <Daily Message>",
                $"{Config.bot.cmdPrefix}UnSubscribe Daily",
                $"{Config.bot.cmdPrefix}Help",
                $"{Config.bot.cmdPrefix}Help <command>",
                $"All commands can be seen [here](http://www.notimplemented.com/)"
                );
        }
        [Command("help")]
        public async Task HelpAsync([Remainder]string command)
        {
            command = command.Split(" ")[0];
            command = command.ToLower();
            switch (command)
            {
                default:
                    await HelpAsync();
                    break;
                case "setup":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}Setup",
                    $"You must have administrative privilages to use this command.\n",
                    $"Usage is: {Config.bot.cmdPrefix}Setup <true/false> <your guild name>\n",
                    $"Must be run before any member can assign with the assign command. Set as true to generate the roles for the auto-assigning.\n"
                    );
                    break;
                case "assign":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}Assign",
                        $"Anyone with a role higher than the bot's (this includes the discord's creator) cannot use this command.\n",
                        $"Useage is: {Config.bot.cmdPrefix}Assign <in-game name>\n",
                        $"This command assigns the guild rank according to realmeye of a member. a unique code must be placed into their description with their profile set to public."
                        );
                    break;
                case "player":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}Player",
                        $"Displays player info that is public on Realmeye.",
                        $"Useage is: {Config.bot.cmdPrefix}Player <player name>"
                        );
                    break;
                case "guild":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}Guild",
                        $"Displays guild info that is public on Realmeye",
                        $"Useage is: {Config.bot.cmdPrefix}Guild <Guild Name>",
                        $"The guild name is case sensitive."
                        );
                    break;
                case "findkey":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}FindKey",
                        $"Finds the location of the key specified, and how much realm gold it costs.",
                        $"Useage is: {Config.bot.cmdPrefix}FindKey <key name (no spaces)>",
                        $""
                        );
                    break;
                case "findclover":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}FindClover",
                        $"Finds the location of a clover, and how much realm gold it costs.",
                        $"Useage is: {Config.bot.cmdPrefix}FindClover"
                        );
                    break;
                case "findbackpack":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}FindBackpack",
                        $"Finds the location of a backpack, and how much realm gold it is.",
                        $"Useage is: {Config.bot.cmdPrefix}FindBackpack"
                        );
                    break;
                case "call":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}Call",
                        $"Calls a run of the specified dungeon, at the location specified. also allows you to say if you need someone to buy a key, and where that key can be found and for what price.",
                        $"Useage is: {Config.bot.cmdPrefix}Call <Dungeon Name> <Server> <location> <need a key? true/false>",
                        $"Each entry must be one word. e.g USW3."
                        );
                    break;
                case "subscribe":
                    Generic(Color.Blue, $"here's some info on {Config.bot.cmdPrefix}Subscribe Daily",
                        $"Designed to be used to remind people of the daily login calendar. Requires @everyone privileges. Reminder is at ~2 hours before daily reset.",
                        $"Useage is: {Config.bot.cmdPrefix}Subscribe Daily <@everyone? true/false> <Daily Message>",
                        $"Use **{Config.bot.cmdPrefix}Unsubscribe Daily** to remove a channel from this list (Requires the permisions mannage messages.)"
                        );
                    break;
            }
        }
        #endregion

        #region Setup
        [Command("Setup")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetupAsync()
        {
            MissingParams("Set-up requires you to enter the guild this discord is for, and whether or not to make the roles for you.",
                "Setup <true/false> <guild name>",
                "where <true/false> is if you need the RotMG guild roles to be generated, and <guild name> is the name of your guild.");
        }
        [Command("Setup")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetupAsync(bool createRoles, [Remainder]string guild)
        {
            try
            {
                GuildInfo config = GuildConfig.gConfig.Find(x => x.DiscordID.Contains(Context.Guild.Id.ToString()));
                if (config.DiscordID != null) GuildConfig.gConfig.Remove(config);
                GuildConfig.Add(Context.Guild.Id.ToString(), guild);
                if (createRoles)
                {
                    await Context.Guild.CreateRoleAsync("Founder", null, Discord.Color.Gold);
                    await Context.Guild.CreateRoleAsync("Leader", null, Discord.Color.Gold);
                    await Context.Guild.CreateRoleAsync("Officer", null, Discord.Color.Orange);
                    await Context.Guild.CreateRoleAsync("Member", null, Discord.Color.Blue);
                    await Context.Guild.CreateRoleAsync("Initiate", null, Discord.Color.Green);
                    await Context.Guild.CreateRoleAsync("Guest", null, Discord.Color.DarkOrange);
                    Generic(Color.Blue,
                        "roles for this discord have been created!",
                        "The roles have been made but they don't do anything yet! Change their permissions to suit your needs.",
                        "Don't change the names, and don't put any roles above mine! This will mess up auto-assigning.");
                }
                else Generic(Color.Orange,
                    "you put false for creating roles.",
                    "If you already have the roles made, that is great! but if you haven't, my auto-assigning feature will not work.",
                    $"If you havent got the roles and want them, do {Config.bot.cmdPrefix}Setup true {guild}"
                    );
                Generic(Color.Green, "setup was successful!", $"This discord, {Context.Guild.Name}, will give anyone who assigns in the guild {guild}, their rank according to [this realmeye page](https://www.realmeye.com/guild/{SpaceDestroyer(guild)})", "(If the roles have been made.)");
            }
            catch (Exception e)
            {
                Error(e.ToString());
            }
        }
        #endregion

        #region Assign
        [Command("Assign")]
        public async Task AssignAsync()
        {
            MissingParams("Assigning requires you to enter your in-Game name", "assign <your in-game name>");
        }
        [Command("Assign")]
        public async Task AssignAsync(string ign)
        {
            try
            {
                User user = new User();
                Realm.GetUserSummary(ign, user);
                Realm.GetUserDescription(user);
                GuildInfo guild = GuildConfig.gConfig.Find(x => x.DiscordID.Contains(Context.Guild.Id.ToString()));
                if (guild.DiscordID == null)
                {
                    Error("This discord has not yet assigned itself a guild with !Setup.", "Please tell an admin.");
                }
                else if (user.Desc1.Contains(Context.User.Id.GetHashCode().ToString())
                    || user.Desc2.Contains(Context.User.Id.GetHashCode().ToString())
                    || user.Desc3.Contains(Context.User.Id.GetHashCode().ToString()))
                {
                    var _User = Context.User as IGuildUser;
                    await _User.ModifyAsync(p => p.Nickname = user.Name);
                    if (user.Guild == guild.GuildName)
                    {
                        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == user.GuildRank);
                        if (role != null)
                        {
                            await (Context.User as IGuildUser).AddRoleAsync(role);
                            Generic(Color.Green, $"you are a {user.GuildRank} of {guild.GuildName}!", $"I have assigned you the role: {user.GuildRank}, " +
                                $"and all of the permissions that come with it!");
                        }
                        else
                        {
                            Error("This discord has not generated or made compatible roles for auto-assigning.",
                                $"Please contact an admin to do **{Config.bot.cmdPrefix}setup true <Guild Name>** to resolve this.");
                        }

                    }
                    else
                    {
                        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "Guest");
                        if (role != null)
                        {
                            await (Context.User as IGuildUser).AddRoleAsync(role);
                            Generic(Color.Green, "you are not in this discords' guild.", "I have assigned you the role: Guest");
                        }
                        else
                        {
                            Error("This discord has not generated or made compatible roles for auto-assigning.",
                                $"Please contact an admin to do **{Config.bot.cmdPrefix}setup true <GuildName>** to resolve this.");
                        }
                    }
                }
                else
                {
                    Error($"Your realmeye description does not contain your unique code: **{Context.User.Id.GetHashCode().ToString()}**", "Please enter your unique code anywhere in your realmeye discription and try again later. Realmeye may take a minute or two to update so please be patient.");
                }
            }
            catch (Exception e)
            {
                Error(e.ToString());
            }
        }
        #endregion       

        #region Player
        [Command("Player")]
        public async Task PlayerAsync()
        {
            MissingParams(
                $"Looking up player data requires you to enter the name of a player.",
                $"Player <player name>"
                );
        }
        [Command("Player")]
        public async Task PlayerAsync(string player)
        {
            User user = new User();
            Realm.GetUserSummary(player, user);
            Generic(Color.Blue,
                $"this is the realmeye page for: **{player}**",
                $"Characters: {user.Chars}, Use the command: **{Config.bot.cmdPrefix}player characters <player>** for a summary of characters",
                $"Skins: {user.Skins}",
                $"Fame: {user.Fame}",
                $"Rank: {user.Rank}",
                $"Account Fame: {user.AccFame}",
                $"Guild: {StringFixer(user.Guild)}",
                $"GuildRank: {user.GuildRank}",
                $"Created: {user.Created}",
                $"Description: Use the command: **{Config.bot.cmdPrefix}Player Description <player>** to see description.",
                $"Pet: {user.PetName} Use the command: **{Config.bot.cmdPrefix}player pet <player>** to see pet stats."
                );
        }
        [Command("Player")]
        public async Task PlayerAsync(string selection, string player)
        {
            User user = new User();
            Realm.GetUserSummary(player, user);
            selection = selection.ToLower();
            switch (selection)
            {
                case "characters":
                    Realm.GetUserClasses(user);
                    ClassesListBuilder(
                        Color.Blue,
                        $"this is a summary for the characters of {player}",
                        user.Classes
                        );
                    break;
                case "description":
                    Realm.GetUserDescription(user);
                    Generic(Color.Blue,
                        $"this is the realmeye description of {player}",
                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━",
                        $"{user.Desc1}",
                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━",
                        $"{user.Desc2}",
                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━",
                        $"{user.Desc3}",
                        $"━━━━━━━━━━━━━━━━━━━━━━━━━━"
                        );
                    break;
                case "pet":
                    Realm.GetUserPetStats(user);
                    Generic(Color.Blue,
                        $"These are the stats for {player}'s\n**{StringFixer(user.PetName)}**!",
                        $"{user.Petstat1}: **{user.Petlvl1}**",
                        $"{user.Petstat2}: **{user.Petlvl2}**",
                        $"{user.Petstat3}: **{user.Petlvl3}**"
                        );
                    break;
                default:
                    Error($"{selection} is not a recognised sub-command of **{Config.bot.cmdPrefix}Player**", "Valid sub-commands are: characters, description, and pet.");
                    break;
            }
        }
        [Command("Player character")]
        public async Task PlayerAsync(int charnum, string player)
        {
            try
            {
                User user = new User();
                Realm.GetUserSummary(player, user);
                Realm.GetUserClasses(user);
                if (charnum > user.Classes.Count)
                {
                    Error($"character number was too high! {player} does not have that many characters.", $"Use a number this is {user.Classes.Count} or lower.");
                }
                else if (charnum < 1)
                {
                    Error($"Arrays may start at 0, but character numbers don't!", $"Use a number this is greater than 0.");

                }
                else
                {
                    Class @class = user.Classes[charnum - 1];
                    Generic(Color.Blue,
                        $"this is {player}'s {@class.ClassName}!",
                        $"Level: **{@class.Lvl}**┃Class Quests Completed: **{@class.CQC}**",
                        $"Fame: **{@class.Fame}**┃Stats: **{@class.Stats}**┃{Backpack(@class.Backpack)}",
                        $"__Equipment__:\n{StringFixer(@class.Eq1)}┃{StringFixer(@class.Eq2)}\n{StringFixer(@class.Eq3)}┃{StringFixer(@class.Eq4)}"
                        );
                }
            }
            catch (Exception e)
            {
                Error(e.ToString());
            }

        }
        private string Backpack(bool backpack)
        {
            if (backpack == true) return "Has Backpack";
            else return "No Backpack";
        }
        #endregion

        #region Guild
        [Command("Guild")]
        public async Task GuildAsync()
        {
            MissingParams(
                $"Looking up guild data requires you to enter the name of a guild.",
                $"Guild <Guild name>",
                $"The guild name is case sensitive."
                );
        }
        [Command("Guild")]
        public async Task GuildAsync([Remainder]string guildName)
        {
            Guild guild = new Guild();
            Realm.GetGuildSummary(guildName, guild);
            Generic(Color.Blue, $"this is the realmeye page for: **{guildName}**",
                $"Number of members: **{guild.MemberCount}**. Use the command **{Config.bot.cmdPrefix}Guild Members <guild name>** to view members.",
                $"Total number of characters: **{guild.Chars}**",
                $"Total fame: **{guild.Fame}**",
                $"Most active on: **{guild.MostActiveOn}**",
                $"━━━━━━━━━━━━━━━━━━━━━━━━━━",
                $"{guild.Desc1}",
                $"━━━━━━━━━━━━━━━━━━━━━━━━━━",
                $"{guild.Desc2}",
                $"━━━━━━━━━━━━━━━━━━━━━━━━━━",
                $"{guild.Desc3}",
                $"━━━━━━━━━━━━━━━━━━━━━━━━━━"
                );
        }
        [Command("Guild Members")]
        public async Task GuildMembersAsync([Remainder]string guildName)
        {
            try
            {
                Guild guild = new Guild();
                Realm.GetGuildSummary(guildName, guild);
                await ReplyAsync($"Fame: {guild.Fame}  Member count: {guild.MemberCount} {guild.Name}");
                Realm.GetGuildMembers(guild);
                MembersListBuilder(Color.Blue, $"Members of the guild {guildName}", guild.Members);
            }
            catch (Exception e)
            {
                Error(e.ToString());
            }

        }
        #endregion

        #region Find
        [Command("FindKey")]
        public async Task FindKeyAsync()
        {
            MissingParams(
                $"Finding a key requires you to enter the type of key you want to find.",
                $"FindKey <dungeon name>",
                "you dont need to be exact with dungeon names, for example lh and lost halls are both valid."
                );
        }
        [Command("FindKey")]
        public async Task FindKeyAsync([Remainder]string key)
        {
            string findings = Find.FindKey(key.ToLower());
            if (findings != "could not find key")
            {
                Generic(Color.Blue,
                    $"I found a {key} key for you!",
                    $"The location and price is: **{findings} gold.**"
                    );
            }
            else
            {
                Error($"I could not find a {key} key, I'm sorry!", $"Try again later and there might be one availiable.");
            }
        }

        [Command("FindClover")]
        public async Task FindCloverAsync()
        {
            string findings = Find.FindClover();
            if (findings != "could not find clover")
            {
                Generic(Color.Blue,
                    $"I found a clover for you!",
                    $"The location and price is: **{findings} gold.**"
                    );
            }
            else
            {
                Error($"I could not find a clover, I'm sorry!", $"Try again later and there might be one availiable.");
            }
        }
        [Command("FindBackpack")]
        public async Task FindBackpackAsync()
        {
            string findings = Find.FindClover();
            if (findings != "could not find backpack")
            {
                Generic(Color.Blue,
                    $"I found a backpack for you!",
                    $"The location and price is: **{findings} gold.**"
                    );
            }
            else
            {
                Error($"I could not find a backpack, I'm sorry!", $"Try again later and there might be one availiable.");
            }
        }
        #endregion

        #region Call
        [Command("Call")]
        public async Task CallAsync()
        {
            MissingParams("The command Call requires you to add a dungeon and a location, and wether or not you need a key.",
                "Call <Dungeon name> <Server> <Location> <need a key? True/False>",
                "Dungeon name must be one word (e.g cdepths), Server must also be one word (e.g USW), location must also be one word (e.g GHall), and if you need a key put true, and if you dont put false.");
        }
        [Command("Call")]
        public async Task CallAsync(string dungeon, string server, string location, string needKey)
        {
            await Context.Message.DeleteAsync();
            bool key = false;
            if (needKey.ToLower() == "true")
            {
                key = true;
            }
            Call(dungeon, server, location, key);

        }
        #endregion

        #region Subscribe
        [Command("Subscribe Daily")]
        public async Task SubscribeAsync()
        {
            MissingParams("Subscribe daily requires you to enter if you want the bot to @everyone for the subscription, and what you want the daily reminder to say.",
                "Subscribe Daily <true/false> <message>", "");
        }
        [Command("Subscribe Daily")]
        [RequireUserPermission(GuildPermission.MentionEveryone)]
        public async Task SubscribeAsync(bool atEveryone, [Remainder]string message)
        {
            SubscribedChannel channelToCheck = DailySubscription.subscribedChannels.Find(x => x.ChannelID.Contains(Context.Channel.Id.ToString()));
            if (channelToCheck.ChannelID != null)
            {
                DailySubscription.subscribedChannels.Remove(channelToCheck);
            }
            DailySubscription.Add(Context.Channel.Id.ToString(), atEveryone, message);
            Generic(Color.Green, "this channel is now subscribed to daily reminders!", 
                $"Your reminder {WillWillNot(atEveryone)} @everyone",
                $"Your reminder reads: {message}");
        }
        [Command("UnSubscribe Daily")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task UnSubscribeAsync()
        {
            SubscribedChannel channelToCheck = DailySubscription.subscribedChannels.Find(x => x.ChannelID.Contains(Context.Channel.Id.ToString()));
            if (channelToCheck.ChannelID != null)
            {
                DailySubscription.subscribedChannels.Remove(channelToCheck);
                Generic(Color.Blue, "you have been unsubscribed from daily reminders!", 
                    "Sad to see you go, but you can always subscribe again!");
            }
            else
            {
                Generic(Color.Orange, "this channel is not subscribed!",
                    $"Use the command {Config.bot.cmdPrefix}Subscribe to subscribe a channel");
            }
        }
        private string WillWillNot(bool Bool)
        {
            if (Bool) return "will";
            else return "will not";
        }
        #endregion

        #region Parsing Strings
        private string StringFixer(string str)
        {
            if (str==null)
            {
                str = "";
            }
            str = str.Replace("&apos;", "'");
            str = str.Replace("&nbsp;", " ");
            return str;
        }
        private string SpaceDestroyer(string str)
        {
            str = str.Replace(" ", "%20");
            return str;
        }
        #endregion

        #region EmbedBuilders
        private async void Call(string dungeon, string server, string location, bool needKey)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Blue);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"{Context.User.Username} is hosting a {dungeon}!");
            embed.WithDescription($"@everyone The location is: {server}, {location}.\n" +
                $"Please react with :crossed_swords: if you are coming!");
            if (needKey)
            {
                string description = embed.Description;
                description += $"\n{Context.User.Username} needs a key for this dungeon. This key can be found at: **{Find.FindKey(dungeon.ToLower())} gold.**";
                embed.WithDescription(description);
            }
            await ReplyAsync("@everyone", false, embed);
        }
        private async void Error(string errorDescription)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Red);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, I encountered an error!");
            embed.WithDescription($"{errorDescription}\n" +
                $"I don't have a solution at this time!");
            await ReplyAsync("", false, embed);
        }
        private async void Error(string errorDescription, string errorSolution)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Red);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, I encountered an error!");
            embed.WithDescription($"{errorDescription}\n" +
                $"{errorSolution}");
            await ReplyAsync("", false, embed);
        }
        private async void MissingParams(string description, string cmd)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Orange);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, this command requires a few parameters!");
            embed.WithDescription($"{description}\n" +
                $"For example: {Config.bot.cmdPrefix}{cmd}\n" +
                $"Try again!");
            await ReplyAsync("", false, embed);
        }
        private async void MissingParams(string description, string cmd, string extra)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Orange);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, this command requires a few parameters!");
            embed.WithDescription($"{description}\n" +
            $"For example: {Config.bot.cmdPrefix}{cmd}\n" +
            $"{extra}\n" +
            $"Try again!");
            await ReplyAsync("", false, embed);
        }
        private async void ClassesListBuilder(Color color, string titleText, ObservableCollection<Class> ts)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");

            string description = "";
            for (int i = 0; i < ts.Count; i++)
            {
                if (i != ts.Count<Class>() - 1)
                {
                    description += $"Character number: **{i + 1}**┃**{ts[i].Stats}**  {ts[i].ClassName}\nBackpack? {ts[i].Backpack}┃Base Fame: **{ts[i].Fame}**\n━━━━━━━━━━━━━━━━\n";
                }
                else
                {
                    description += $"Character number: **{i + 1}**┃**{ts[i].Stats}**  {ts[i].ClassName}\nBackpack? {ts[i].Backpack}┃Base Fame: **{ts[i].Fame}**\n━━━━━━━━━━━━━━━━\nFor more detail use the command: \n**!Player character <character no.> <player>**";
                }
            }
            embed.WithDescription(description);
            await ReplyAsync("", false, embed);
        }
        private async void MembersListBuilder(Color color, string titleText, ObservableCollection<Member> ts)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");

            string description = "";
            for (int i = 0; i < ts.Count; i++)
            {
                if (i!=ts.Count-1)
                {
                    description += $"{ts[i].Name}┃GR:{ts[i].GuildRank}┃F:{ts[i].Fame}┃R:{ts[i].Rank}┃C:{ts[i].Chars}\n";
                }
                else
                {
                    description += $"{ts[i].Name}┃GR:{ts[i].GuildRank}┃F:{ts[i].Fame}┃R:{ts[i].Rank}┃C:{ts[i].Chars}";
                }
            }
            embed.WithDescription(description);
            await ReplyAsync("", false, embed);
        }
        #region Generic
        private async void Generic(Color color, string titleText)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                  $"{desc2}\n" +
                  $"{desc3}\n" +
                  $"{desc4}\n" +
                  $"{desc5}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                 $"{desc2}\n" +
                 $"{desc3}\n" +
                 $"{desc4}\n" +
                 $"{desc5}\n" +
                 $"{desc6}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11, string desc12)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}\n" +
                $"{desc12}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11, string desc12, string desc13)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}\n" +
                $"{desc12}\n" +
                $"{desc13}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11, string desc12, string desc13, string desc14)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}\n" +
                $"{desc12}\n" +
                $"{desc13}\n" +
                $"{desc14}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11, string desc12, string desc13, string desc14, string desc15)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}\n" +
                $"{desc12}\n" +
                $"{desc13}\n" +
                $"{desc14}\n" +
                $"{desc15}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11, string desc12, string desc13, string desc14, string desc15, string desc16)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription($"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}\n" +
                $"{desc12}\n" +
                $"{desc13}\n" +
                $"{desc14}\n" +
                $"{desc15}\n" +
                $"{desc16}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11, string desc12, string desc13, string desc14, string desc15, string desc16, string desc17)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription(
                $"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}\n" +
                $"{desc12}\n" +
                $"{desc13}\n" +
                $"{desc14}\n" +
                $"{desc15}\n" +
                $"{desc16}\n" +
                $"{desc17}");
            await ReplyAsync("", false, embed);
        }
        private async void Generic(Color color, string titleText, string desc1, string desc2, string desc3, string desc4, string desc5, string desc6, string desc7, string desc8, string desc9, string desc10, string desc11, string desc12, string desc13, string desc14, string desc15, string desc16, string desc17, string desc18)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(color);
            embed.WithFooter("©Argocyte");
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://static.tvtropes.org/pmwiki/pub/images/guill.png");
            embed.WithTitle($"Hi {Context.User.Username}, {titleText}");
            embed.WithDescription(
                $"{desc1}\n" +
                $"{desc2}\n" +
                $"{desc3}\n" +
                $"{desc4}\n" +
                $"{desc5}\n" +
                $"{desc6}\n" +
                $"{desc7}\n" +
                $"{desc8}\n" +
                $"{desc9}\n" +
                $"{desc10}\n" +
                $"{desc11}\n" +
                $"{desc12}\n" +
                $"{desc13}\n" +
                $"{desc14}\n" +
                $"{desc15}\n" +
                $"{desc16}\n" +
                $"{desc17}\n" +
                $"{desc18}");
            await ReplyAsync("", false, embed);
        }

        #endregion
        #endregion
    }
}
