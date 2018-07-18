using System;
using System.Linq;

using SitefinityWebApp.Mvc.Models;
using SitefinityWebApp.Mvc.Services.Contracts;

using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.RelatedData;

namespace SitefinityWebApp.Mvc.Services
{
    public class TorrentService : ITorrentService
    {
        public IQueryable<DynamicContent> RetrieveCollectionOfTorrents(DynamicModuleManager dynamicModuleManager, Type torrentType)
        {
            var myCollection = dynamicModuleManager
                .GetDataItems(torrentType)
                .Where(t => t.Status == ContentLifecycleStatus.Live && t.Visible == true);

            return myCollection;
        }

        public void SetTorrentValues(DynamicContent torrentItem, CreateTorrentModel torrentModel, Guid currentUserId)
        {
            torrentItem.SetValue("Title", torrentModel.Title);
            torrentItem.SetValue("Description", torrentModel.Description);
            torrentItem.SetValue("AdditionalInfo", torrentModel.AdditionalInfo);
            torrentItem.SetValue("Genre", torrentModel.Genre);
            torrentItem.SetValue("TorrentDateCreated", DateTime.UtcNow);

            torrentItem.SetString("UrlName", torrentModel.Title);
            torrentItem.SetValue("PublicationDate", DateTime.UtcNow);
            torrentItem.SetValue("Owner", currentUserId);

            torrentItem.ApprovalWorkflowState = "Published";
        }

        public void SetTorrentImage(LibrariesManager imageManager, DynamicContent torrentItem)
        {
            var imageItem = imageManager
                .GetImages()
                .FirstOrDefault(i => i.Status == ContentLifecycleStatus.Master);

            if (imageItem != null)
            {
                torrentItem.CreateRelation(imageItem, "Image");
            }
        }

        public void SetTorrentFile(LibrariesManager torrentManager, DynamicContent torrentItem)
        {
            var torrentFileItem = torrentManager
                .GetDocuments()
                .FirstOrDefault(i => i.Status == ContentLifecycleStatus.Master);

            if (torrentFileItem != null)
            {
                torrentFileItem.SetValue("FileType", torrentFileItem.MimeType);
                torrentItem.CreateRelation(torrentFileItem, "TorrentFile");
            }
        }

        public DynamicContent GetTorrent(DynamicModuleManager dynamicModuleManager, Type torrentType, string urlName)
        {
            var torrent = this.RetrieveCollectionOfTorrents(dynamicModuleManager, torrentType)
                .SingleOrDefault(t => t.UrlName == urlName);

            return torrent;
        }
    }
}
