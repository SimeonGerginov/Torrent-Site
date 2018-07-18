using System;
using System.Linq;
using System.Web.Mvc;

using SitefinityWebApp.Mvc.Models;
using SitefinityWebApp.Mvc.Providers;

using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "Torrent_MVC", Title = "Torrent", SectionName = "CustomWidgets")]
	public class TorrentController : Controller
    {
        private readonly Type torrentType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Torrents.Torrent");
        private readonly DynamicModuleManagerProvider dynamicModuleManagerProvider;
        private readonly ImageManagerProvider imageManagerProvider;

        public TorrentController()
        {
            this.dynamicModuleManagerProvider = new DynamicModuleManagerProvider();
            this.imageManagerProvider = new ImageManagerProvider();
        }

        [Authorize]
        public ActionResult Index()
        {
            var torrents = this.RetrieveCollectionOfTorrents().AsEnumerable();

            return this.View("Index", torrents);
        }

        [HttpGet]
        [Authorize]
	    public ActionResult CreateTorrent()
        {
            var identity = ClaimsManager.GetCurrentIdentity();

            if (!identity.IsAuthenticated)
            {
                this.RedirectToAction("Index");
            }

            var torrentModel = new TorrentModel();

	        return this.View("TorrentForm", torrentModel);
	    }

	    [HttpPost]
        [Authorize]
	    public ActionResult CreateTorrent(TorrentModel torrentModel)
	    {
	        var dynamicModuleManager = this.dynamicModuleManagerProvider.DynamicModuleManager;

            DynamicContent torrentItem = dynamicModuleManager.CreateDataItem(torrentType);
            
            torrentItem.SetValue("Title", torrentModel.Title);
            torrentItem.SetValue("Description", torrentModel.Description);
            torrentItem.SetValue("AdditionalInfo", torrentModel.AdditionalInfo);
            torrentItem.SetValue("DownloadLink", torrentModel.DownloadLink);
            torrentItem.SetValue("Genre", torrentModel.Genre);
            torrentItem.SetValue("TorrentDateCreated", DateTime.UtcNow);

	        var imageManager = this.imageManagerProvider.ImageManager;
            var imageItem = imageManager
                .GetImages()
                .FirstOrDefault(i => i.Status == ContentLifecycleStatus.Master);

            if (imageItem != null)
            {
                torrentItem.CreateRelation(imageItem, "Image");
            }

	        torrentItem.SetString("UrlName", torrentModel.Title);
            torrentItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
            torrentItem.SetValue("PublicationDate", DateTime.UtcNow);
	        torrentItem.ApprovalWorkflowState = "Published";

	        dynamicModuleManager.Lifecycle.Publish(torrentItem);
            dynamicModuleManager.SaveChanges();

	        return this.RedirectToAction("TorrentDetails", new { urlName = torrentItem.UrlName });
	    }

        [Authorize]
        public ActionResult TorrentDetails(string urlName)
        {
            var torrent = this.RetrieveCollectionOfTorrents()
                .Where(t => t.UrlName == urlName)
                .SingleOrDefault();

            return this.View("TorrentDetails", torrent);
        }

        private IQueryable<DynamicContent> RetrieveCollectionOfTorrents()
        {
            var dynamicModuleManager = this.dynamicModuleManagerProvider.DynamicModuleManager;

            var myCollection = dynamicModuleManager
                .GetDataItems(torrentType)
                .Where(t => t.Status == ContentLifecycleStatus.Live && t.Visible == true);

            return myCollection;
        }
    }
}
