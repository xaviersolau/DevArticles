using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FctCodeGen
{
    public interface IFunctionGenerator
    {
        Task Generate(string project);
    }
}
