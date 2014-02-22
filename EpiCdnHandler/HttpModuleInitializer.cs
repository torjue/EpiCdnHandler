using System.Web;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace EpiCdnHandler
{
    [InitializableModule]
    public class HttpModuleInitializer : IInitializableHttpModule
    {
        public void InitializeHttpEvents(HttpApplication application)
        {
            foreach (string moduleName in application.Modules)
            {
                if (application.Modules[moduleName] is UrlRewriteHttpModule)
                {
                    return;
                }
            }
            var cdnImageUrlRewriteModule = new UrlRewriteHttpModule();
            cdnImageUrlRewriteModule.Init(application);
        }

        public void Initialize(InitializationEngine context) {  }
        public void Uninitialize(InitializationEngine context) {  }
        public void Preload(string[] parameters) { }
    }
}