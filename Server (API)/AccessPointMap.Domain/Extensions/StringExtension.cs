namespace AccessPointMap.Domain.Extensions
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value);
        }
    }
}
