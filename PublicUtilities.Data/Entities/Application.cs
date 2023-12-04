using System.ComponentModel.DataAnnotations;
using PublicUtilities.Data.Constants;

#pragma warning disable CS8618
namespace PublicUtilities.Data.Entities
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public DateTime DateOfWork { get; set; }

        [Required]
        [MaxLength(EntityConstants.AddressMaxLength)]
        [MinLength(EntityConstants.AddressMinLength)]
        public string Address { get; set; }

        [Required]
        [MaxLength(EntityConstants.NameFieldMaxLength)]
        [MinLength(EntityConstants.NameFieldMinLength)]
        public string ApplicantName { get; set; }

        [Required]
        public string TypeOfWork { get; set; }

        [Required]
        public string ScaleOfWork { get; set; }

        [Required]
        public bool Approved { get; set; }
    }
}
