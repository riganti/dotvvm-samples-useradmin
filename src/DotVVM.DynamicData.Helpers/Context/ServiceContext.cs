using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.Framework.Hosting;

namespace DotVVM.DynamicData.Helpers.Context
{
    public class ServiceContext
    {
        public IDotvvmRequestContext DotvvmRequestContext { get; }

        public DynamicDataHelpersPageConfiguration PageConfiguration { get; }


        public ServiceContext(IDotvvmRequestContext dotvvmRequestContext, DynamicDataHelpersPageConfiguration pageConfiguration)
        {
            DotvvmRequestContext = dotvvmRequestContext;
            PageConfiguration = pageConfiguration;
        }
        
    }
}
