namespace Architecture_3IMD.Models.Web
{
    public class SaleWebOutput
    {
        public SaleWebOutput(int id, int store_id, int bouquet_id, int amount, string firstname, string lastname)
        {
            Id = id;
            Store_id = store_id;
            Bouquet_id = bouquet_id;
            Amount = amount;
            FirstName = firstname;
            LastName = lastname;
        }

        public int Id { get; set; }
        public int Store_id { get; set; }
        public int Bouquet_id { get; set; }
        public int Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}