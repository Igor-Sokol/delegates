using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using NUnit.Framework;

#pragma warning disable CA1707

namespace Delegates.Tests
{
    [TestFixture]
    public class FunctionExtensionsTests
    {
        [Test]
        [Category(nameof(FunctionExtensions.TimeoutSafeInvoke))]
        public void FunctionExtension_TimeoutSafeInvoke_Should_Invoke_Specified_Function()
        {
            int magicNumber = 1234;
            Func<int> f = () => magicNumber;
            Assert.AreEqual(magicNumber, f.TimeoutSafeInvoke(), "FunctionExtension.TimeoutSafeInvoke should invoke the specified function");
        }

        [Test]
        [Category(nameof(FunctionExtensions.TimeoutSafeInvoke))]
        public void FunctionExtensions_TimeoutSafeInvoke_Should_Catch_Up_To_2_Timeouts()
        {
            StringWriter sw = new StringWriter();
            TraceListener listener = new TextWriterTraceListener(sw);
            Trace.Listeners.Add(listener);
            int magicNumber = 1234;
            int tries = 0;
            
            Func<int> function = () =>
                tries++ < 2
                    ? throw new WebException("The operation has timed out", WebExceptionStatus.Timeout)
                    : magicNumber;

            Assert.AreEqual(magicNumber, function.TimeoutSafeInvoke(), "FunctionExtensions.TimeoutSafeInvoke should invoke the specified function");
            Trace.Listeners.Remove(listener);
            listener.Flush();
            sw.Close();
            string trace = sw.ToString();
            Assert.IsTrue(trace.Contains("System.Net.WebException"), "FunctionExtensions.TimeoutSafeInvoke should log a WebException to trace log");
        }

        [Test]
        [Category(nameof(FunctionExtensions.TimeoutSafeInvoke))]
        public void FunctionExtensions_TimeoutSafeInvoke_Should_Raise_TimeoutException_If_Number_Of_Timeouts_More_Then_2()
        {
            int magicNumber = 1234;
            int tries = 0;
         
            Func<int> function = () =>
                tries++ < 3
                    ? throw new WebException("The operation has timed out", WebExceptionStatus.Timeout)
                    : magicNumber;
            
            Assert.Throws<WebException>(() => function.TimeoutSafeInvoke());
        }
    }
}