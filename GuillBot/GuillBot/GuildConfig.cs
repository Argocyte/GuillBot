using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace GuillBot
{
    class GuildConfig
    {
        private const string configFolder = "Resources";
        private const string configFile = "Guilds.json";
        public static List<GuildInfo> gConfig;

        static GuildConfig()
        {
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            if (!File.Exists(configFolder + "/" + configFile))
            {
                gConfig = new List<GuildInfo>();
                string json = JsonConvert.SerializeObject(gConfig, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                gConfig = JsonConvert.DeserializeObject<List<GuildInfo>>(json);
            }
        }
        public static void Add(string discordID, string guildName)
        {
            GuildInfo guild = new GuildInfo
            {
                DiscordID = discordID,
                GuildName = guildName
            };
            gConfig.Add(guild);
            Update();
        }
        static void Update()
        {
            string json = JsonConvert.SerializeObject(gConfig, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + configFile, json);
        }
    }
    public struct GuildInfo
    {
        public string DiscordID;
        public string GuildName;
    }
}
