using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnitLite;
using NUnit.Common;
using System.Reflection;

namespace Tests
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var writter = new ExtendedTextWrapper(Console.Out);
            return new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args, writter, Console.In);
        }
    }
}
