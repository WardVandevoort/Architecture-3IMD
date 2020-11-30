namespace Architecture_3IMD.Data
{
    public class SalesDbContext : ISalesDbContext
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
