namespace AccessPointMap.Domain.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool IsLengthLess(this string value, int max)
        {
            return value.Length < max;
        }
    }
}
