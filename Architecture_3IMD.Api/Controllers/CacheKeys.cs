namespace Architecture_3IMD.Controllers
{
    // You can use any format of string; however this one is "nice" because if you later want to move
    // to redis you can use the same ones and have implicit namespaces
    public static class CacheKeys
    {
        public static string AllSales = "Cache:Sales:All";
    }
}
