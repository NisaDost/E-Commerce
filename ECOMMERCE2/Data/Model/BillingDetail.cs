namespace ECOMMERCE2.Data.Model
{
    public class BillingDetail
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}
