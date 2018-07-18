using Telerik.Sitefinity.DynamicModules;

namespace SitefinityWebApp.Mvc.Providers
{
    public class DynamicModuleManagerProvider
    {
        private DynamicModuleManager dynamicModuleManager;

        public DynamicModuleManager DynamicModuleManager
        {
            get
            {
                if (this.dynamicModuleManager == null)
                {
                    this.dynamicModuleManager = DynamicModuleManager.GetManager();
                }

                return this.dynamicModuleManager;
            }
        }
    }
}
