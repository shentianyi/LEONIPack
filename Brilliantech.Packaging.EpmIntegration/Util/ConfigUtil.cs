using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;

namespace Brilliantech.Packaging.EpmIntegration.Util
{
    public class ConfigUtil
    {
        private IConfig config;
        private IConfigSource source;
        public ConfigUtil()
        {
        }

        public ConfigUtil(string node)
        {
            config = source.Configs[node];
        }

        public ConfigUtil(string node, string filename)
        {
            source = new IniConfigSource(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));
            config = source.Configs[node];
        }

        /// <summary>
        /// update the node
        /// </summary>
        /// <param name="node">name of node</param>
        public void Update(string node, string filename)
        {
            config = source.Configs[node];
            if (config == null)
            {
                source.AddConfig(node);
                config = source.Configs[node];
            }
            RemoveKeys();
        }

        /// <summary>
        /// get value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return config.Get(key);
        }

        /// <summary>
        /// get all values of node
        /// </summary>
        /// <returns></returns>
        public string[] GetAllNodeValue()
        {
            return config.GetValues();
        }

        public string[] GetAllNodeKey()
        {
            return config.GetKeys();
        }


        /// <summary>
        /// set key and value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            config.Set(key, value);
        }

        /// <summary>
        /// save the changes
        /// </summary>
        public void Save()
        {
            source.Save();
        }

        /// <summary>
        /// remove all keys
        /// </summary>
        private void RemoveKeys()
        {
            string[] keys = config.GetKeys();
            foreach (string key in keys)
            {
                config.Remove(key);
            }
        }
    }
}
