using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FunctionAttribute : Attribute
    {
        public FunctionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    /// <summary>
    /// Trigger attribute used for durable activity functions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [DebuggerDisplay("{Activity}")]
    public sealed class ActivityTriggerAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the activity function.
        /// </summary>
        /// <remarks>
        /// If not specified, the function name is used as the name of the activity.
        /// This property supports binding parameters.
        /// </remarks>
        /// <value>
        /// The name of the activity function or <c>null</c> to use the function name.
        /// </value>
        public string? Activity { get; set; }
    }

    public class FunctionContext
    {
        public IServiceProvider InstanceServices { get; set; } = default!;
    }
}
