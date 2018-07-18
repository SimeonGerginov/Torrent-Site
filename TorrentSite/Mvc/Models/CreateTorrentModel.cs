using System.ComponentModel.DataAnnotations;

namespace SitefinityWebApp.Mvc.Models
{
    public class CreateTorrentModel
	{
        [Display(Name = "Torrent title")]
        [Required(ErrorMessage = "{0} is required!")]
	    public string Title { get; set; }

        [Display(Name = "Torrent description")]
        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.MultilineText)]
	    public string Description { get; set; }

        [Display(Name = "Torrent additional info")]
        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.MultilineText)]
	    public string AdditionalInfo { get; set; }

        [Display(Name = "Torrent genre")]
        [Required(ErrorMessage = "{0} is required!")]
	    public string Genre { get; set; }
    }
}
