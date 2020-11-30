namespace Architecture_3IMD.Data
{
    // 1:1 from appsettings.Development.json
    public interface ISalesDbContext
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}