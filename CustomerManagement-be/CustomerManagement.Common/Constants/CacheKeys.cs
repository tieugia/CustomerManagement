namespace CustomerManagement.Common.Constants;

public static class CacheKeys
{
    public static string AllCustomers => "all_customers";
    public static string Customer(Guid id) => $"customer_{id}";
}

