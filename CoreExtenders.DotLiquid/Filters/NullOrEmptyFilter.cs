namespace CoreExtenders.DotLiquid.Filters
{
    public static class CustomFilters
    {
        public static string IsNullOrEmpty(string input)
        {
            var status = string.IsNullOrEmpty(input) ? "true" : "false";
            return status;
        }
    }
}
