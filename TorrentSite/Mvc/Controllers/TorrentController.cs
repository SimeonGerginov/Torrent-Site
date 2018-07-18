using System;
using System.Linq;
using System.Web.Mvc;

using SitefinityWebApp.Mvc.Models;
using SitefinityWebApp.Mvc.Providers;
using SitefinityWebApp.Mvc.Services;
using SitefinityWebApp.Mvc.Services.Contracts;

using Telerik.Sitefinity.Mvc;
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
        private readonly TorrentManagerProvider torrentManagerProvider;

        private readonly ITorrentService torrentService;
        private readonly IUserService userService;

        public TorrentController()
        {
            this.dynamicModuleManagerProvider = new DynamicModuleManagerProvider();
            this.imageManagerProvider = new ImageManagerProvider();
            this.torrentManagerProvider = new TorrentManagerProvider();

            this.torrentService = new TorrentService();
            this.userService = new UserService();
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
            var user = ClaimsManager.GetCurrentIdentity();
            var isUserAuthenticated = this.userService.CheckIfUserIsAuthenticated(user);
            var isUserModerator = this.userService.CheckIfUserIsModerator(user);

            if (!isUserAuthenticated || !isUserModerator)
            {
                return this.RedirectToAction("Index");
            }

            var torrentModel = new CreateTorrentModel();

	        return this.View("TorrentForm", torrentModel);
	    }

	    [HttpPost]
        [Authorize]
	    public ActionResult CreateTorrent(CreateTorrentModel torrentModel)
	    {
	        var user = ClaimsManager.GetCurrentIdentity();
	        var isUserAuthenticated = this.userService.CheckIfUserIsAuthenticated(user);
	        var isUserModerator = this.userService.CheckIfUserIsModerator(user);

	        if (!isUserAuthenticated || !isUserModerator)
	        {
	            return this.RedirectToAction("Index");
	        }

            var dynamicModuleManager = this.dynamicModuleManagerProvider.DynamicModuleManager;
	        var imageManager = this.imageManagerProvider.ImageManager;
	        var torrentManager = this.torrentManagerProvider.TorrentManager;
            var torrentItem = dynamicModuleManager.CreateDataItem(torrentType);
	        var currentUserId = SecurityManager.GetCurrentUserId();

            this.torrentService.SetTorrentValues(torrentItem, torrentModel, currentUserId);
            this.torrentService.SetTorrentImage(imageManager, torrentItem);
            this.torrentService.SetTorrentFile(torrentManager, torrentItem);

	        dynamicModuleManager.Lifecycle.Publish(torrentItem);
            dynamicModuleManager.SaveChanges();

	        return this.RedirectToAction("TorrentDetails", new { urlName = torrentItem.UrlName });
	    }

        [HttpGet]
        [Authorize]
        public ActionResult TorrentDetails(string urlName)
        {
            var user = ClaimsManager.GetCurrentIdentity();
            var isUserAuthenticated = this.userService.CheckIfUserIsAuthenticated(user);

            if (!isUserAuthenticated)
            {
                return this.RedirectToAction("Index");
            }

            var dynamicModuleManager = this.dynamicModuleManagerProvider.DynamicModuleManager;

            var torrent = this.torrentService.GetTorrent(dynamicModuleManager, this.torrentType, urlName);

            return this.View("TorrentDetails", torrent);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DownloadTorrent(string fileName, string fileType)
        {
            var user = ClaimsManager.GetCurrentIdentity();
            var isUserAuthenticated = this.userService.CheckIfUserIsAuthenticated(user);

            if (!isUserAuthenticated)
            {
                return this.RedirectToAction("Index");
            }

            var file = this.torrentService.GetTorrentFile(fileName, fileType);

            return File(file, fileType, System.IO.Path.GetFileName(file));
        }
    }
}
