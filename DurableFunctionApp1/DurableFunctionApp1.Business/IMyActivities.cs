using DurableLib.Abstractions;
using System.Diagnostics;

namespace DurableFunctionApp1.Business
{
    public interface IMyActivities: IActivity
    {
        Task<string> SayHello(string name, string comment);
    }
}
