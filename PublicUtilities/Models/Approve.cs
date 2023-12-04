#nullable disable
namespace PublicUtilities.Models
{
    public class Approve
    {
        public int ApplicationId { get; set; }
        public IEnumerable<int> WorkerIds { get; set; }
    }
}
