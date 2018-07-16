using System;
using System.Linq;
using System.Web.Mvc;

using SitefinityWebApp.Mvc.Models;

using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Versioning;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "Torrent_MVC", Title = "Torrent", SectionName = "CustomWidgets")]
	public class TorrentController : Controller
    {
        private string transactionName;
        private readonly Type torrentType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Torrents.Torrent");
	    private DynamicModuleManager dynamicModuleManager;
	    private LibrariesManager imageManager;
        private VersionManager versionManager;

        public DynamicModuleManager DynamicModuleManager
	    {
	        get
	        {
	            if (this.dynamicModuleManager == null || 
	                this.dynamicModuleManager.TransactionName != this.transactionName)
	            {
	                this.dynamicModuleManager = DynamicModuleManager.GetManager(string.Empty, transactionName);
	            }

	            return this.dynamicModuleManager;
	        }
	    }

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

        public VersionManager VersionManager
        {
            get
            {
                if (this.versionManager == null || this.versionManager.TransactionName != this.transactionName)
                {
                    this.versionManager = VersionManager.GetManager(null, transactionName);
                }

                return this.versionManager;
            }
        }

	    public IQueryable<DynamicContent> RetrieveCollectionOfTorrents()
	    {
	        var myCollection = this.DynamicModuleManager
	            .GetDataItems(torrentType)
	            .Where(t => t.Status == ContentLifecycleStatus.Live && t.Visible == true);

	        return myCollection;
        }

        public ActionResult Index()
        {
            var torrents = this.RetrieveCollectionOfTorrents().AsEnumerable();

            return this.View("Index", torrents);
        }

	    public ActionResult TorrentDetails(string urlName)
	    {
	        var torrent = this.RetrieveCollectionOfTorrents()
	            .Where(t => t.UrlName == urlName)
	            .SingleOrDefault();

	        return this.View("TorrentDetails", torrent);
	    }

        [HttpGet]
	    public ActionResult CreateTorrent()
	    {
            TorrentModel torrentModel = new TorrentModel();

	        return this.View("TorrentForm", torrentModel);
	    }

	    [HttpPost]
	    public ActionResult CreateTorrent(TorrentModel torrentModel)
	    {
	        this.transactionName = "Transaction" + torrentModel.Title;

            DynamicContent torrentItem = this.DynamicModuleManager.CreateDataItem(torrentType);
            
            torrentItem.SetValue("Title", torrentModel.Title);
            torrentItem.SetValue("Description", torrentModel.Description);
            torrentItem.SetValue("AdditionalInfo", torrentModel.AdditionalInfo);
            torrentItem.SetValue("DownloadLink", torrentModel.DownloadLink);
            torrentItem.SetValue("Genre", torrentModel.Genre);
            torrentItem.SetValue("TorrentDateCreated", DateTime.UtcNow);

            var imageItem = this.ImageManager
                .GetImages()
                .FirstOrDefault(i => i.Status == ContentLifecycleStatus.Master);

            if (imageItem != null)
            {
                torrentItem.CreateRelation(imageItem, "Image");
            }
            
            torrentItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
            torrentItem.SetValue("PublicationDate", DateTime.UtcNow);

            this.VersionManager.CreateVersion(torrentItem, false);
            TransactionManager.CommitTransaction(this.transactionName);

	        return this.RedirectToAction("TorrentDetails", new { urlName = torrentItem.UrlName });
	    }
	}
}
