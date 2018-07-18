using System;
using System.Linq;

using SitefinityWebApp.Mvc.Models;

using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Modules.Libraries;

namespace SitefinityWebApp.Mvc.Services.Contracts
{
    public interface ITorrentService
    {
        IQueryable<DynamicContent> RetrieveCollectionOfTorrents(DynamicModuleManager dynamicModuleManager, Type torrentType);

        void SetTorrentValues(DynamicContent torrentItem, CreateTorrentModel torrentModel, Guid currentUserId);

        void SetTorrentImage(LibrariesManager imageManager, DynamicContent torrentItem);

        void SetTorrentFile(LibrariesManager torrentManager, DynamicContent torrentItem);

        DynamicContent GetTorrent(DynamicModuleManager dynamicModuleManager, Type torrentType, string urlName);
    }
}
