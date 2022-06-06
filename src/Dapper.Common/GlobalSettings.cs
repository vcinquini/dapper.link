using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper
{
    /// <summary>
    /// Global Settings
    /// </summary>
    public static class GlobalSettings
    {
        /// <summary>
        /// Database metadata provider
        /// </summary>
        public static IDbMetaInfoProvider DbMetaInfoProvider { get; set; }
            = new AnnotationDbMetaInfoProvider();
        /// <summary>
        /// Entity mapper provider
        /// </summary>
        public static IEntityMapperProvider EntityMapperProvider { get; set; }
            = new EntityMapperProvider();
        /// <summary>
        /// xml command configuration
        /// </summary>
        public static IXmlCommandsProvider XmlCommandsProvider { get; set; }
            = new XmlCommandsProvider();
    }
}