namespace ECOMMERCE2.Data.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public BillingDetail BillingDetail { get; set; }
        public Cart Cart { get; set; }
    }
}
