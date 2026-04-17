namespace StoreInventorySystem.Application.Common
{
    public static class CacheKeys
    {
        public static string Stats = "products:stats";

        public static string Products(int page, int pageSize) => $"products:page:{page}:size:{pageSize}";

        public static string Product(int id) => $"product:{id}";
    }
}
