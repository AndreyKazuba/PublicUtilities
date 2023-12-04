using System.ComponentModel.DataAnnotations;
using PublicUtilities.Data.Constants;

#pragma warning disable CS8618
namespace PublicUtilities.Data.Entities
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(EntityConstants.NameFieldMinLength)]
        [MaxLength(EntityConstants.NameFieldMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(EntityConstants.NameFieldMinLength)]
        [MaxLength(EntityConstants.NameFieldMaxLength)]
        public string LastName { get; set; }
    }
}
