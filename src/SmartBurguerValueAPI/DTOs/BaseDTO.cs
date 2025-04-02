namespace SmartBurguerValueAPI.DTOs
{
    public class BaseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdate { get; set; }
        public bool IsActive { get; set; }

    }
}
