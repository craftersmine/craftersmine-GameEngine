using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace craftersmine.GameEngine.System
{
    public sealed class GameConfig
    {
        private Dictionary<string, string> cfg = new Dictionary<string, string>();
        private string ConfigFilePath { get; set; }
        private string cfgAppDataPath { get; set; }
        private string cfgFileName { get; set; }

        public bool IsCreated { get; internal set; }

        public GameConfig(string path, string configFile)
        {
            cfgAppDataPath = path;
            cfgFileName = configFile;
            LoadConfig();
        }

        public void LoadConfig()
        {
            GameApplication.Log(Utils.LogEntryType.Info, "Loading game configuration... " + Path.Combine(cfgAppDataPath, cfgFileName + ".cfg"));
            ConfigFilePath = Path.Combine(GameApplication.AppDataGameRoot, cfgAppDataPath, cfgFileName + ".cfg");
            if (File.Exists(ConfigFilePath))
            {
                IsCreated = false;
                string[] file = File.ReadAllLines(ConfigFilePath);
                foreach (var ln in file)
                {
                    string[] kvp = ln.Split('=');
                    cfg.Add(kvp[0], kvp[1]);
                }
            }
            else IsCreated = true;
        }

        public void SaveConfig()
        {
            GameApplication.Log(Utils.LogEntryType.Info, "Saving game configuration... " + Path.Combine(cfgAppDataPath, cfgFileName + ".cfg"));
            List<string> lines = new List<string>();
            foreach (var cfgentry in cfg)
            {
                string ln = string.Join("=", cfgentry.Key, cfgentry.Value);
            }
            File.WriteAllLines(ConfigFilePath, lines);
        }

        public void SetString(string key, string value)
        {
            if (cfg.ContainsKey(key))
                cfg[key] = value;
            else cfg.Add(key, value);
        }

        public void SetBool(string key, bool value)
        {
            SetString(key, value.ToString());
        }

        public void SetInt(string key, int value)
        {
            SetString(key, value.ToString());
        }

        public void SetLong(string key, long value)
        {
            SetString(key, value.ToString());
        }

        public string GetString(string key)
        {
            if (cfg.TryGetValue(key, out string val))
                return val;
            else throw new KeyNotFoundException("Config key \"" + key + "\" not found in file!");
        }

        public int GetInt(string key)
        {
            if (cfg.TryGetValue(key, out string val))
            {
                if (int.TryParse(val, out int intVal))
                    return intVal;
                else throw new InvalidDataException("Config key \"" + key + "\" has invalid type, expected value is numeric value!");
            }
            else throw new KeyNotFoundException("Config key \"" + key + "\" not found in file!");
        }

        public long GetLong(string key)
        {
            if (cfg.TryGetValue(key, out string val))
            {
                if (long.TryParse(val, out long intVal))
                    return intVal;
                else throw new InvalidDataException("Config key \"" + key + "\" has invalid type, expected value is long numeric value!");
            }
            else throw new KeyNotFoundException("Config key \"" + key + "\" not found in file!");
        }

        public bool GetBool(string key)
        {
            if (cfg.TryGetValue(key, out string val))
            {
                if (bool.TryParse(val, out bool boolVal))
                    return boolVal;
                else throw new InvalidDataException("Config key \"" + key + "\" has invalid type, expected value is \"true\" or \"false\"!");
            }
            else throw new KeyNotFoundException("Config key \"" + key + "\" not found in file!");
        }
    }
}
