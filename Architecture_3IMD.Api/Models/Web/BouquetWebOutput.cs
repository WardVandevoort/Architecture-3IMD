namespace Architecture_3IMD.Models.Web
{
    public class BouquetWebOutput
    {
        public BouquetWebOutput(int id, string name, int price, string description)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
}