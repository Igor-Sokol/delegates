using System;

namespace Delegates
{
    public static class FunctionExtensions
    {
        /// <summary>
        ///   Tries to invoke the specified function up to 3 times if the result is unavailable 
        /// </summary>
        /// <param name="function">specified function</param>
        /// <returns>
        ///   Returns the result of specified function, if WebException occurs during request then exception should be logged into trace 
        ///   and the new request should be started (up to 3 times).
        /// </returns>
        /// <example>
        ///   Sometimes if network is unstable it is required to try several request to get data:
        ///   
        ///   Func<string> f1 = ()=>(new System.Net.WebClient()).DownloadString("http://www.google.com/");
        ///   string data = f1.TimeoutSafeInvoke();
        ///   
        ///   If the first attempt to download data is failed by WebException then exception should be logged to trace log and the second attemp should be started.
        ///   The second attempt has the same workflow.
        ///   If the third attempt fails then this exception should be rethrow to the application.
        /// </example>
        public static T TimeoutSafeInvoke<T>(this Func<T> function)
        {
            // TODO : Implement TimeoutSafeInvoke<T>
            throw new NotImplementedException();
        }
    }
}