using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotVVM.DynamicData.Helpers.Configuration
{
    public class ToolbarButton
    {
        public Func<string> TextProvider { get; set; }

        public string Url { get; set; }

        public string RouteName { get; set; }

        public object RouteParameters { get; set; }
        
    }
}
