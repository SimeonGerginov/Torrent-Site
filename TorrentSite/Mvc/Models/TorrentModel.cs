using System.ComponentModel.DataAnnotations;
using Telerik.Sitefinity.Libraries.Model;

namespace SitefinityWebApp.Mvc.Models
{
    public class TorrentModel
	{
	    public string Title { get; set; }

        [DataType(DataType.MultilineText)]
	    public string Description { get; set; }

        [DataType(DataType.MultilineText)]
	    public string AdditionalInfo { get; set; }

	    public string DownloadLink { get; set; }

	    public string Genre { get; set; }

	    public Image Image { get; set; }
	}
}
