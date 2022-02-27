using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotVVM.DynamicData.Helpers.ViewModels;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace UserAdmin.ViewModels;

public class MasterPageViewModel : DotvvmViewModelBase
{

    public MenuViewModel Menu { get; set; } = new();

    public LanguageSelectorViewModel LanguageSelector { get; set; } = new();

}