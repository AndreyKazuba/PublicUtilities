using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Data.Entities
{
    public class ApplicationWorker
    {
        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public int WorkerId { get; set; }
    }
}
