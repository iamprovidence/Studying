namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Menu menu = new Menu())
            {
                menu.RunAsync().GetAwaiter().GetResult();
            }
        }
    }
}
