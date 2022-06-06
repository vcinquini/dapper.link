using System.IO;

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
        /// load configuration file
        /// </summary>
        /// <param name="filename">file name</param>
        void Load(string filename);

        /// <summary>
        /// Load a xml string
        /// </summary>
        /// <param name="xmlString">xml string</param>
        void LoadXml(string xmlString);
        
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
}
