using Telerik.Sitefinity.Modules.Libraries;

namespace SitefinityWebApp.Mvc.Providers
{
    public class ImageManagerProvider
    {
        private LibrariesManager imageManager;

        public LibrariesManager ImageManager
        {
            get
            {
                if (this.imageManager == null)
                {
                    this.imageManager = LibrariesManager.GetManager();
                }

                return this.imageManager;
            }
        }
    }
}
