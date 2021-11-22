using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Spirion.Automation.Framework
{
    public class Wait
    {

        public static void For(Func<bool> condition)
        {
            while (condition.Invoke())
            {

            }
        }

        /// <summary>
        /// This will allow you to wait for a specified condition to become True. 
        /// </summary>
        /// <param name="secondsTotalToWait">The total number of milliseconds that you want to wait.</param>
        /// <param name="conditionToWaitFor">This is the condition that you want to keep waiting for. For example, if you want to wait for an IsReady property to be true, then you would pass "() => !IsReady" into this parameter.</param>
        internal bool WaitFor(int secondsTotalToWait, Func<bool> conditionToWaitFor)
        {
            var timer = new Stopwatch();
            long elapsed = 0;

            timer.Start();

            while (!conditionToWaitFor.Invoke())
            {
                elapsed = timer.Elapsed.Seconds;
                if (elapsed >= secondsTotalToWait)
                {
                    Log.Logger.Error($"The condition returned false after {elapsed} milliseconds.");
                    return false;
                }
            }
            Log.Logger.Information
                ($"The condition returned true after {elapsed} milliseconds.");
            return true;
        }

    }

}
