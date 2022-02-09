using System.Threading.Tasks;
using St8Ment.Example.Console.StateMachines;
using St8Ment.Example.Console.States;

namespace St8Ment.Example.Console
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

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();

            await StateForgeCase.Execute();
        }
    }
}
