using System;
using System.Text.RegularExpressions;
using System.Web;

namespace EpiCdnHandler
{
    public class UrlRewriteHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            var regex = new Regex("^/cdn-[a-z0-9]{8}/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            var url = HttpContext.Current.Request.Path;
            if (!regex.IsMatch(url)) return;
            HttpContext.Current.Items[CdnConfigurationSection.GetConfiguration().ItemKey] = true;
            HttpContext.Current.RewritePath(url.Substring(13), String.Empty, String.Empty, true);
        }

        public void Dispose()
        {
        }
    }
}