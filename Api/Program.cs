using Holism.Api;

namespace Holism.Accounts.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Holism.Api.Config.ConfigureEverything();
            Application.Run();
        }
    }
}
