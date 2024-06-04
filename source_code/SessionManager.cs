public static class SessionManager
{
    public static int? UserId { get; private set; }
    public static int? CustomerId { get; private set; }

    public static void SetUserId(int? userId)
    {
        UserId = userId;
    }

    public static void SetCustomerId(int? customerId)
    {
        CustomerId = customerId;
    }

    public static void ClearUser()
    {
        UserId = null; 
    }

    public static void ClearCustomer()
    {
        CustomerId = null;
    }
}
