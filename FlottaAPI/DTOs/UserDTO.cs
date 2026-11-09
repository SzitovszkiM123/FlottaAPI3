namespace FlottaAPI.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Status { get; set; } = "pending";
        public bool Active { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string Image { get; set; } = "missing.jpg";
        public int Permission { get; set; }
    }
}
