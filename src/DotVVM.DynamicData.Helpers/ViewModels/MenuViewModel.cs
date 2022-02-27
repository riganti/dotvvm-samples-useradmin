using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.Configuration;
using DotVVM.DynamicData.Helpers.Model;
using DotVVM.Framework.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.DynamicData.Helpers.ViewModels
{
    public class MenuViewModel : DotvvmViewModelBase
    {
        [Bind(Direction.ServerToClientFirstRequest)]
        public List<MenuItemModel> Menu { get; set; }

        public override Task Init()
        {
            if (!Context.IsPostBack)
            {
                Menu = Context.Services.GetRequiredService<DynamicDataHelpersConfiguration>()
                    .Sections
                    .SelectMany(s => s.Pages.Where(p => p.IncludeInMenu))
                    .Select(p => new MenuItemModel()
                    {
                        Text = p.PageTitleProvider(),
                        Url = Context.TranslateVirtualPath(Context.Configuration.RouteTable[p.RouteName].BuildUrl(p.RouteParameters))
                    })
                    .ToList();
            }

            return base.Init();
        }
    }
}
