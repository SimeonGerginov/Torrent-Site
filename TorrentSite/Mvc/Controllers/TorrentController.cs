using System;
using System.Linq;
using System.Web.Mvc;

using SitefinityWebApp.Mvc.Models;
using SitefinityWebApp.Mvc.Providers;
using SitefinityWebApp.Mvc.Services;
using SitefinityWebApp.Mvc.Services.Contracts;

using Telerik.Sitefinity.GenericContent.Model;
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
        private readonly ITorrentService torrentService;

        public TorrentController()
        {
            this.dynamicModuleManagerProvider = new DynamicModuleManagerProvider();
            this.imageManagerProvider = new ImageManagerProvider();
            this.torrentService = new TorrentService();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var dynamicModuleManager = this.dynamicModuleManagerProvider.DynamicModuleManager;

            var torrents = this.torrentService
                .RetrieveCollectionOfTorrents(dynamicModuleManager, this.torrentType)
                .AsEnumerable();

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
            var torrentItem = dynamicModuleManager.CreateDataItem(torrentType);
	        var currentUserId = SecurityManager.GetCurrentUserId();

            this.torrentService.SetTorrentValues(torrentItem, torrentModel, currentUserId);

	        var imageManager = this.imageManagerProvider.ImageManager;
            var imageItem = imageManager
                .GetImages()
                .FirstOrDefault(i => i.Status == ContentLifecycleStatus.Master);

            if (imageItem != null)
            {
                torrentItem.CreateRelation(imageItem, "Image");
            }

	        dynamicModuleManager.Lifecycle.Publish(torrentItem);
            dynamicModuleManager.SaveChanges();

	        return this.RedirectToAction("TorrentDetails", new { urlName = torrentItem.UrlName });
	    }

        [HttpGet]
        [Authorize]
        public ActionResult TorrentDetails(string urlName)
        {
            var dynamicModuleManager = this.dynamicModuleManagerProvider.DynamicModuleManager;

            var torrent = this.torrentService.GetTorrent(dynamicModuleManager, this.torrentType, urlName);

            return this.View("TorrentDetails", torrent);
        }
    }
}
