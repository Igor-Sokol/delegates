using System;
using System.Diagnostics;
using System.Net;

#pragma warning disable S2259 // Null pointers should not be dereferenced (TimeoutSafeInvoke никак не сможет бросить lastWebException как null)

namespace Delegates
{
    public static class FunctionExtensions
    {
        /// <summary>
        ///   Tries to invoke the specified function up to 3 times if the result is unavailable.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="function">specified function.</param>
        /// <returns>
        ///   Returns the result of specified function, if WebException occurs during request then exception should be logged into trace 
        ///   and the new request should be started (up to 3 times).
        /// </returns>
        /// <example>
        ///   Sometimes if network is unstable it is required to try several request to get data:
        ///   
        ///   Func.<string> f1 = ()=>(new System.Net.WebClient()).DownloadString("http://www.google.com/");
        ///   string data = f1.TimeoutSafeInvoke();
        ///   
        ///   If the first attempt to download data is failed by WebException then exception should be logged to trace log and the second attemp should be started.
        ///   The second attempt has the same workflow.
        ///   If the third attempt fails then this exception should be rethrow to the application.
        /// </example>
        public static T TimeoutSafeInvoke<T>(this Func<T> function)
        {
            WebException lastWebException = null;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    return function();
                }
                catch (WebException ex)
                {
                    Trace.WriteLine(ex.GetType());

                    lastWebException = new WebException(ex.Message, ex);
                }
            }

            throw lastWebException;
        }
    }
}
