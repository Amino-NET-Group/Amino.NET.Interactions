using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions
{
    public class ParameterInfo
    {
        public string Name { get; set; }
        public bool IsOptional { get; set; }
    }

    public class FunctionAnalyzer
    {
        public ParameterInfo[] GetParameters(MethodInfo method)
        {
            return method.GetParameters()
                         .Select(p => new ParameterInfo
                         {
                             Name = p.ParameterType.Name,
                             IsOptional = p.HasDefaultValue
                         })
                         .ToArray();
        }
    }
}
