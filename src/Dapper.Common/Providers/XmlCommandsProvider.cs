using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Dapper.XmlResolves;

namespace Dapper
{
    /// <summary>
    /// xml configuration provider
    /// </summary>
    public interface IXmlCommandsProvider
    {
        /// <summary>
        /// Parse dynamic sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        string Build<T>(string id, T parameter) where T : class;
        /// <summary>
        /// Parse sql
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string Build(string id);
        /// <summary>
        /// Load configuration file
        /// </summary>
        /// <param name="filename">filename</param>
        void Load(string filename);
        /// <summary>
        /// Load all matching files from the specified path
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="pattern">File wildcard</param>
        void Load(string path, string pattern);
        /// <summary>
        /// Load all matching files from the specified path
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="pattern">File wildcard</param>
        /// <param name="options">Find options</param>
        void Load(string path, string pattern, SearchOption options);
        /// <summary>
        /// Load configuration all xml from assembly
        /// </summary>
        /// <param name="assembly">assembly</param>
        void Load(System.Reflection.Assembly assembly);
        /// <summary>
        /// Load configuration from assembly
        /// </summary>
        /// <param name="assembly">assembly</param>
        /// <param name="pattern">Regular match</param>
        void Load(System.Reflection.Assembly assembly, string pattern);
    }
    /// <summary>
    /// xml configuration provider
    /// </summary>
    public class XmlCommandsProvider : IXmlCommandsProvider
    {
        private readonly Dictionary<string, CommandNode> _commands
            = new Dictionary<string, CommandNode>();

        private Dictionary<string, string> ResolveVariables(XmlElement element)
        {
            var variables = new Dictionary<string, string>();
            var elements = element.Cast<XmlNode>()
                .Where(a => a.Name == "var");
            foreach (XmlElement item in elements)
            {
                if (item.Name == "var")
                {
                    var id = item.GetAttribute("id");
                    var value = string.IsNullOrEmpty(item.InnerXml)
                         ? item.GetAttribute("value") : item.InnerXml;
                    variables.Add(id, value);
                }
            }
            return variables;
        }

        private string ReplaceVariable(Dictionary<string, string> variables, string text)
        {
            var matches = Regex.Matches(text, @"\${(?<key>.*?)}");
            foreach (Match item in matches)
            {
                var key = item.Groups["key"].Value;
                if (variables.ContainsKey(key))
                {
                    var value = variables[key];
                    text = text.Replace("${" + key + "}", value);
                }
            }
            return Regex.Replace(text, @"\s+", " ").Trim(' ');
        }

        private CommandNode ResolveCommand(XmlElement element)
        {
            var cmd = new CommandNode();
            foreach (XmlNode item in element.ChildNodes)
            {
                if (item.Name == "var" || item.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                if (item.NodeType == XmlNodeType.Text)
                {
                    cmd.Nodes.Add(new TextNode
                    {
                        Value = item.Value
                    });
                }
                else if (item.NodeType == XmlNodeType.Element && item.Name == "where")
                {
                    var whereNode = new WhereNode();
                    foreach (XmlNode iitem in item.ChildNodes)
                    {
                        if (iitem.NodeType == XmlNodeType.Text)
                        {
                            whereNode.Nodes.Add(new TextNode
                            {
                                Value = iitem.Value
                            });
                        }
                        else if (iitem.NodeType == XmlNodeType.Element && iitem.Name == "if")
                        {
                            var test = iitem.Attributes["test"].Value;
                            var value = string.IsNullOrEmpty(iitem.InnerText) ?
                                (iitem.Attributes["value"]?.Value ?? string.Empty) : iitem.InnerText;
                            whereNode.Nodes.Add(new IfNode
                            {
                                Test = test,
                                Value = value
                            });
                        }
                    }
                    cmd.Nodes.Add(whereNode);
                }
                else if (item.NodeType == XmlNodeType.Element && item.Name == "if")
                {
                    var test = item.Attributes["test"].Value;
                    var value = string.IsNullOrEmpty(item.InnerText) ?
                             (item.Attributes["value"]?.Value ?? string.Empty) : item.InnerText;
                    cmd.Nodes.Add(new IfNode
                    {
                        Test = test,
                        Value = value
                    });
                }
            }
            return cmd;
        }

        public string Build<T>(string id, T parameter) where T : class
        {
            if (!_commands.ContainsKey(id))
            {
                return null;
            }
            var cmd = _commands[id];
            return cmd.Resolve(cmd, parameter);
        }

        public string Build(string id)
        {
            return Build(id, (object)null);
        }
        private void Resolve(XmlDocument document)
        {
            if (document.DocumentElement.Name != "commands")
            {
                return;
            }
            lock (this)
            {
                var @namespace = document.DocumentElement
                    .GetAttribute("namespace") ?? string.Empty;
                // resolve global variables
                var globalVariables = ResolveVariables(document.DocumentElement);
                // get command node
                var elements = document.DocumentElement
                    .Cast<XmlNode>()
                    .Where(a => a.Name != "var" && a is XmlElement);
                foreach (XmlElement item in elements)
                {
                    var id = item.GetAttribute("id");
                    id = string.IsNullOrEmpty(@namespace) ? $"{id}" : $"{@namespace}.{id}";
                    // resolve local variables
                    var localVariables = ResolveVariables(item);
                    //Merge local and global variables, local variables can override global variables
                    var variables = new Dictionary<string, string>(globalVariables);
                    foreach (var ariable in localVariables)
                    {
                        if (variables.ContainsKey(ariable.Key))
                        {
                            variables[ariable.Key] = ariable.Value;
                        }
                        else
                        {
                            variables.Add(ariable.Key, ariable.Value);
                        }
                    }
                    //replace variable
                    var xml = ReplaceVariable(variables, item.OuterXml);
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    // Parse the command by variable
                    var cmd = ResolveCommand(doc.DocumentElement);
                    if (_commands.ContainsKey(id))
                    {
                        _commands[id] = cmd;
                    }
                    else
                    {
                        _commands.Add(id, cmd);
                    }
                }
            }
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        /// <param name="filename"></param>
        public void Load(string filename)
        {
            var document = new XmlDocument();
            document.Load(filename);
            Resolve(document);
        }

        /// <summary>
        /// Load xml from the stream
        /// </summary>
        /// <param name="stream"></param>
        public void Load(Stream stream)
        {
            XmlDocument document = new XmlDocument();
            document.Load(stream);
            Resolve(document);
        }

        /// <summary>
        /// Load all matching files from the specified path
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="pattern">wildcard</param>
        public void Load(string path, string pattern)
        {
            var files = System.IO.Directory.GetFiles(path, pattern);
            foreach (var item in files)
            {
                Load(item);
            }
        }

        /// <summary>
        /// Load all matching files from the specified path
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="pattern">wildcard</param>
        /// <param name="options">Find options</param>
        public void Load(string path, string pattern, SearchOption options)
        {
            var files = System.IO.Directory.GetFiles(path, pattern, options);
            foreach (var item in files)
            {
                Load(item);
            }
        }

        /// <summary>
        /// Load files ending in .xml from embedded resources
        /// </summary>
        /// <param name="assembly">assembly</param>
        public void Load(System.Reflection.Assembly assembly)
        {
            var filenames = assembly.GetManifestResourceNames();
            foreach (var item in filenames)
            {
                if (!item.EndsWith(".xml", System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                Load(assembly.GetManifestResourceStream(item));
            }

        }

        /// <summary>
        /// Load configuration from embedded resource
        /// </summary>
        /// <param name="assembly">assembly</param>
        /// <param name="pattern">match pattern</param>
        public void Load(System.Reflection.Assembly assembly, string pattern)
        {
            var filenames = assembly.GetManifestResourceNames();
            foreach (var item in filenames)
            {
                if (!Regex.IsMatch(item, pattern))
                {
                    continue;
                }
                Load(assembly.GetManifestResourceStream(item));
            }
        }
    }
}