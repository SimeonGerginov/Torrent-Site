using System;
using System.Linq;

using SitefinityWebApp.Mvc.Models;

using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;

namespace SitefinityWebApp.Mvc.Services.Contracts
{
    public interface ITorrentService
    {
        IQueryable<DynamicContent> RetrieveCollectionOfTorrents(DynamicModuleManager dynamicModuleManager, Type torrentType);

        void SetTorrentValues(DynamicContent torrentItem, CreateTorrentModel torrentModel, Guid currentUserId);

        DynamicContent GetTorrent(DynamicModuleManager dynamicModuleManager, Type torrentType, string urlName);
    }
}
