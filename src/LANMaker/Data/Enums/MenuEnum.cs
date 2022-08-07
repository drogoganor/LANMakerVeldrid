using System.ComponentModel.DataAnnotations;

namespace LANMaker.Data.Enums
{
    public enum MenuEnum
    {
        [Display(Name = "Installed Games")]
        InstalledGames,

        [Display(Name = "Store")]
        Store,

        [Display(Name = "Downloads")]
        Downloads,

        [Display(Name = "Configure")]
        Configure
    }
}
