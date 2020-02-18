using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace GuillBot
{
    class DailySubscription
    {
        private const string configFolder = "Resources";
        private const string configFile = "DailySubscriptions.json";
        public static List<SubscribedChannel> subscribedChannels;

        static DailySubscription()
        {
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            if (!File.Exists(configFolder + "/" + configFile))
            {
                subscribedChannels = new List<SubscribedChannel>();
                string json = JsonConvert.SerializeObject(subscribedChannels, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                subscribedChannels = JsonConvert.DeserializeObject<List<SubscribedChannel>>(json);
            }
        }
        public static void Add(string channelID, bool atEveryone, string message)
        {
            SubscribedChannel subscribedChannel = new SubscribedChannel
            {
                ChannelID = channelID,
                AtEveryone = atEveryone,
                Message = message
            };
            subscribedChannels.Add(subscribedChannel);
            Update();
        }
        static void Update()
        {
            string json = JsonConvert.SerializeObject(subscribedChannels, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + configFile, json);
        }
    }
    public struct SubscribedChannel
    {
        public string ChannelID;
        public bool AtEveryone;
        public string Message;
    }
}
