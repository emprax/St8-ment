using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using St8Ment.StateMachines;

namespace St8Ment.Tests.Integration.Utilities;

public class Test1Callback(ILogger<TesTSubject> logger) : ITransitionCallback<Test1Action>
{
    public Task Execute(Test1Action action)
    {
        logger.LogInformation(action.ActionName);
        return Task.CompletedTask;
    }
}

public class Test2Callback(ILogger<TesTSubject> logger) : ITransitionCallback<Test2Action>
{
    public Task Execute(Test2Action action)
    {
        logger.LogInformation(action.ActionName);
        return Task.CompletedTask;
    }
}

public class Test3Callback(ILogger<TesTSubject> logger) : ITransitionCallback<Test3Action>
{
    public Task Execute(Test3Action action)
    {
        logger.LogInformation(action.ActionName);
        return Task.CompletedTask;
    }
}
