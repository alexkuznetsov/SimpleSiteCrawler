namespace SimpleSiteCrawler.Cli
{
    public static class ArrayExtensions
    {
        public static T GetValueSafe<T>(this T[] arr, int position)
        {
            if (position >= 0 && position < arr.Length)
            {
                return arr[position];
            }

            return default;
        }
    }
}