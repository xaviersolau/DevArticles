using DurableLib.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FctCodeGen.Patterns.Itf
{
    public class ReturnType { }

    public interface IActivityPattern: IActivity
    {
        Task<ReturnType> MethodPattern(object argument);
    }
}
