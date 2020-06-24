using St8_ment.Example.Console.V1;
using St8_ment.Example.Console.V2;
using System.Threading.Tasks;

namespace St8_ment.Example.Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CaseV1.Execute();
            await CaseV2.Execute();
        }
    }
}
