using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {
        private List<IPlugin> _plugins = new List<IPlugin>();
        /// <summary>
        /// Initiates the Pluginmanager class and adds all plugins
        /// </summary>
        public PluginManager()
        {
            Add(new TestPlugin());
            Add(new LowerPlugin());
            Add(new TempPlugin());
            Add(new NaviPlugin());
            Add(new StaticFilePlugin());

            dynamicLoad();
        }
        /// <summary>
        /// Returns a list of all plugins. Never returns null.
        /// </summary>
        public IEnumerable<IPlugin> Plugins
        {
            get { return _plugins; }
        }
        /// <summary>
        /// Dynamically loads all plugins (.dll files) from specific directory
        /// </summary>
        public void dynamicLoad()
        {

            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory))
            {
                if (!file.EndsWith(".dll"))
                    continue;

                Assembly dll = Assembly.LoadFrom(Path.Combine(Environment.CurrentDirectory, file));
                Type[] pluginTypes = dll.GetTypes();

                foreach (Type type in pluginTypes)
                {
                    if(type.GetInterface("BIF.SWE1.Interfaces.IPlugin") == null)
                        continue;

                    Add(Activator.CreateInstance(type) as IPlugin);
                }

            }
        }
        /// <summary>
        /// Adds a new plugin. If the plugin was already added, nothing will happen.
        /// </summary>
        public void Add(IPlugin plugin)
        {
            if(!_plugins.Contains(plugin)) _plugins.Add(plugin);

        }
        /// <summary>
        /// Adds a new plugin by type name. If the plugin was already added, nothing will happen.
        /// Throws an exeption, when the type cannot be resoled or the type does not implement IPlugin.
        /// </summary>
        public void Add(string plugin)
        {
            Type type = Type.GetType(plugin);
            IPlugin str_plugin = (IPlugin)Activator.CreateInstance(type);

            Add(str_plugin);
        }
        /// <summary>
        /// Clears all plugins
        /// </summary>
        public void Clear()
        {
            _plugins.Clear();
        }
    }
}
