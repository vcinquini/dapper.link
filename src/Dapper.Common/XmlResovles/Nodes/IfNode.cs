using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.XmlResolves
{
    internal class IfNode : INode
    {
        public Delegate Delegate { get; set; }
        public string Test { get; set; }
        public string Value { get; set; }
    }
}
