using NUnit.Framework;
using Spirion.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpirionUITests
{
    public class AssertExt
    {
        public static void IsTrue(bool result, string message)
        {
            string status = (result == true) ? "Passed" : "Failed";
            Report.Instance.LogTestValidation(status, message);
            Assert.IsTrue(result, message);
        }

        public static void IsFalse(bool result, string message)
        {
            string status = (result == false) ? "Passed" : "Failed";
            Report.Instance.LogTestValidation(status, message);
            Assert.IsFalse(result, message);
        }
    }
}
