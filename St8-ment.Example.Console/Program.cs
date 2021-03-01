using System.Threading.Tasks;
using St8_ment.Example.Console.StateMachines;
using St8_ment.Example.Console.States;

namespace St8_ment.Example.Console
{
    public class Program
    {
        public static async Task Main(string[] _)
        {
            await StateMachineCase.Execute();

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();

            await StateCase.Execute();
        }
    }
}
