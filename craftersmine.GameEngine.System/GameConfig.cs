using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Represents Game configuration file that contains Key-Value pairs of data
    /// </summary>
    public sealed class GameConfig
    {
        private Dictionary<string, string> cfg = new Dictionary<string, string>();
        private string ConfigFilePath { get; set; }
        private string cfgAppDataPath { get; set; }
        private string cfgFileName { get; set; }

        /// <summary>
        /// Gets true if config file is just created now, else false
        /// </summary>
        public bool IsCreated { get; internal set; }

        /// <summary>
        /// Creates new <see cref="GameConfig"/> instance with config <paramref name="path"/> and name of config file
        /// </summary>
        /// <param name="path">Path to config file (<see cref="GameApplication.AppDataGameRoot"/> + /<paramref name="path"/>)</param>
        /// <param name="configFile">Config file name without extention</param>
        public GameConfig(string path, string configFile)
        {
            cfgAppDataPath = path;
            cfgFileName = configFile;
            LoadConfig();
        }

        /// <summary>
        /// Loads or reloads config from disk
        /// </summary>
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

        /// <summary>
        /// Saves config to disk
        /// </summary>
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

        /// <summary>
        /// Sets <see cref="string"/> with <paramref name="value"/> at <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <param name="value">Config entry value</param>
        public void SetString(string key, string value)
        {
            if (cfg.ContainsKey(key))
                cfg[key] = value;
            else cfg.Add(key, value);
        }

        /// <summary>
        /// Sets <see cref="bool"/> with <paramref name="value"/> at <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <param name="value">Config entry value</param>
        public void SetBool(string key, bool value)
        {
            SetString(key, value.ToString());
        }

        /// <summary>
        /// Sets <see cref="int"/> with <paramref name="value"/> at <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <param name="value">Config entry value</param>
        public void SetInt(string key, int value)
        {
            SetString(key, value.ToString());
        }

        /// <summary>
        /// Sets <see cref="long"/> with <paramref name="value"/> at <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <param name="value">Config entry value</param>
        public void SetLong(string key, long value)
        {
            SetString(key, value.ToString());
        }

        /// <summary>
        /// Gets <see cref="string"/> value from entry with <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <returns></returns>
        public string GetString(string key)
        {
            if (cfg.TryGetValue(key, out string val))
                return val;
            else throw new KeyNotFoundException("Config key \"" + key + "\" not found in file!");
        }

        /// <summary>
        /// Gets <see cref="int"/> value from entry with <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets <see cref="long"/> value from entry with <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets <see cref="bool"/> value from entry with <paramref name="key"/>
        /// </summary>
        /// <param name="key">Config entry key</param>
        /// <returns></returns>
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
