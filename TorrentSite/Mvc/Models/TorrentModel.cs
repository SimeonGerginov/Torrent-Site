using System;
using System.ComponentModel.DataAnnotations;

using Telerik.Sitefinity.Libraries.Model;

namespace SitefinityWebApp.Mvc.Models
{
    public class TorrentModel
	{
        [Required]
	    public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
	    public string Description { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
	    public string AdditionalInfo { get; set; }

        [Required]
	    public string DownloadLink { get; set; }

        [Required]
	    public string Genre { get; set; }

        [Required]
	    public DateTime TorrentDateCreated { get; set; }

	    public Image Image { get; set; }
	}
}
