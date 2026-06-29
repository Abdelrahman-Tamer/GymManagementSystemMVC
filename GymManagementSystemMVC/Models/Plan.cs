namespace GymManagementSystemMVC.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAT { get; set; }
        public DateTime? UpdatedAT { get; set; }
        public string Description { get; set; } = null!;
        public int DurationInDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
