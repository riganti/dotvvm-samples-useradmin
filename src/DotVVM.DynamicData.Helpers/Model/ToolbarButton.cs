using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotVVM.DynamicData.Helpers.Model
{
    public class ToolbarButtonModel
    {

        public string Text { get; set; }

        public string Url { get; set; }

        public string RouteName { get; set; }

        public object RouteParameters { get; set; }
    }
}
