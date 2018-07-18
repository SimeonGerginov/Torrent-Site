using Telerik.Sitefinity.Modules.Libraries;

namespace SitefinityWebApp.Mvc.Providers
{
    public class TorrentManagerProvider
    {
        private LibrariesManager torrentManager;

        public LibrariesManager TorrentManager
        {
            get
            {
                if (this.torrentManager == null)
                {
                    this.torrentManager = LibrariesManager.GetManager();
                }

                return this.torrentManager;
            }
        }
    }
}
