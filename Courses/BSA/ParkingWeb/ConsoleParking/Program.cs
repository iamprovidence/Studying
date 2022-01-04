namespace ConsoleParking
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Menu menu = new Menu())
            {
                menu.Run();
            }
        }
    }
}
