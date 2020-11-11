namespace Architecture_3IMD.Models.Web
{
    public class StoreWebOutput
    {
        public StoreWebOutput(int id, string name, string address, string region)
        {
            Id = id;
            Name = name;
            Address = address;
            Region = region;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Region { get; set; }
    }
}