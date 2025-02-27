namespace SmartBurguerValueAPI.Models
{
    public class BaseEntity
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }

        public void Update()
        {
            DateUpdated = DateTime.UtcNow;
        }
    }
}
