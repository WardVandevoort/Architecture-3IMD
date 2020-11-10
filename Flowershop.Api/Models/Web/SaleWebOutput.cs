namespace Architecture_3IMD.Models.Web
{
    public class SaleWebOutput
    {
        public SaleWebOutput(int id, int store_id, int bouquet_id, int amount)
        {
            Id = id;
            Store_id = store_id;
            Bouquet_id = bouquet_id;
            Amount = amount;
        }

        public int Id { get; set; }
        public int Store_id { get; set; }
        public int Bouquet_id { get; set; }
        public int Amount { get; set; }
    }
}