using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotVVM.DynamicData.Helpers.ViewModels
{
    public class LanguageSelectorViewModel : DotvvmViewModelBase
    {

        [Bind(Direction.ServerToClientFirstRequest)]
        public string CurrentLanguage => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        [Bind(Direction.ServerToClientFirstRequest)]
        public string[] SupportedCultures => Context.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
            .SupportedUICultures!.Select(c => c.TwoLetterISOLanguageName).ToArray();


        public void ChangeLanguage(string language)
        {
            if (!SupportedCultures.Contains(language))
            {
                throw new Exception("Unsupported culture!");
            }

            Context.GetAspNetCoreContext().Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language))
            );
            Context.RedirectToLocalUrl(Context.HttpContext.Request.Url.PathAndQuery);
        }

    }
}
