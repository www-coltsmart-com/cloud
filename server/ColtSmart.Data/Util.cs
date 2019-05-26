namespace ColtSmart.Data
{
    public static class Util
    {
        
        public static int GetId(this ISqlExecutor sqlExecutor, string tableName)
        {
            return sqlExecutor.ExecuteScalar<int>("select max(Id) from " + tableName) + 1;
        }
    }
}
