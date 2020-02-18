using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace GuillBot
{
    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;

        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
            _client.Log += Log;
            _client.UserJoined += UserJoined;
            //_client.UserVoiceStateUpdated += UserVoiceStateUpdated;
            
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            //DailyReminder();
            await Task.Delay(-1);
        }
        
        private async Task UserJoined(SocketGuildUser user)
        {
            await user.SendMessageAsync($"Hi {user.Username}, welcome to {user.Guild.Name}! Please add the code **{user.Id.GetHashCode()}** to your realmeye description, then do {Config.bot.cmdPrefix}assign [ign] in #verify. " +
            $"It may take a while for realmeye to update.");
        }

        private async Task Log(LogMessage arg)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString().ToString()+": "+arg.Message);
        }

        private async void DailyReminder()
        {
            bool doneForToday = false;

            TimeSpan time1 = new TimeSpan(22, 0, 0);
            TimeSpan time2 = new TimeSpan(23, 0, 0);
            while (true)
            {
                if (!doneForToday && DateTime.Now.TimeOfDay >= time1 && DateTime.Now.TimeOfDay <= time2 && _client.ConnectionState == ConnectionState.Connected)
                {
                    Task.Delay(5000).Wait();
                    foreach (SubscribedChannel SC in DailySubscription.subscribedChannels)
                    {
                        if (SC.AtEveryone == true)
                        {
                            await ((ISocketMessageChannel)_client.GetChannel(Convert.ToUInt64(SC.ChannelID))).SendMessageAsync($"@everyone {SC.Message}");
                        }
                        else
                        {
                            await ((ISocketMessageChannel)_client.GetChannel(Convert.ToUInt64(SC.ChannelID))).SendMessageAsync(
                                $"{SC.Message}");
                        }
                    }
                    doneForToday = true;
                }
                if (DateTime.Now.TimeOfDay > time2)
                {
                    doneForToday = false;
                }
            }
        }
    }
}
