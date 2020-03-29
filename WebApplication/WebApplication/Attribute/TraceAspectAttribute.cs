using System;
using System.Reflection;
using AspectInjector.Broker;

namespace WebApplication.Attribute
{
    [Aspect(Scope.Global)]
    [Injection(typeof(TraceAspectAttribute))]
    public sealed class TraceAspectAttribute : System.Attribute
    {
        [Advice(Kind.After, Targets = Target.Method)]
        public void TraceStart(
            [Argument(Source.Name)] string name,
            [Argument(Source.Metadata)] MethodBase methodBase)
        {
            foreach (var attribute in methodBase.CustomAttributes)
            {
                Console.WriteLine($"Method {name} using custom attribute: {attribute}");
            }
        }
    }
}