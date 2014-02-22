using System;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace EpiCdnHandler
{
    [InitializableModule]
    class UrlBuilder : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ContentRoute.CreatedVirtualPath += CreatedVirtualPath;
        }

        private void CreatedVirtualPath(object sender, UrlBuilderEventArgs urlBuilderEventArgs)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                return;
            }

            var enabled = CdnConfigurationSection.GetConfiguration().Enabled;
            if (!enabled)
            {
                return;
            }

            object contentReferenceObject;
            if (!urlBuilderEventArgs.RouteValues.TryGetValue(RoutingConstants.NodeKey, out contentReferenceObject))
            {
                return;
            }

            var routedContentLink = contentReferenceObject as ContentReference;
            if (ContentReference.IsNullOrEmpty(routedContentLink))
            {
                return;
            }

            // Check that the link is to a image
            var imageData = DataFactory.Instance.Get<IContent>(routedContentLink) as ImageData;
            if (imageData == null)
            {
                return;
            }

            // Check that everyone has read access to the image
            var securable = imageData as ISecurable;
            if ((securable.GetSecurityDescriptor().GetAccessLevel(PrincipalInfo.AnonymousPrincipal) & AccessLevel.Read) != AccessLevel.Read)
            {
                return;
            }

            // Generate timestamp
            var hash = imageData.Saved.Ticks.ToString("X").Substring(2, 8).ToLower();

            // Generate new url
            var baseUrl = CdnConfigurationSection.GetConfiguration().Url;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = SiteDefinition.Current.SiteUrl.AbsoluteUri;
            }
            var cdnPath = "cdn-" + hash;
            var originalPath = urlBuilderEventArgs.UrlBuilder.Path;
            var cdnUri = new Uri(baseUrl).Append(cdnPath, originalPath);

            urlBuilderEventArgs.UrlBuilder.Uri = cdnUri;
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
