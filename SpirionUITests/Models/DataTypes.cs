using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SpirionUITests.Models
{
    public enum DataTypes
    {
        SocialSecurityNumber,
        CreditCardNumber,
        TelephoneNumber,
        InvalidSSN
    }

    public enum RemoteMachineStatus
    {
        On,
        off
    }

    public enum AccessTime
    {
        On,
        NotOn,
        After,
        Before,
        Today,
        Yesterday,
        OnOrBefore,
        NotEmpty,
        OnOrAfter,
        Last30Days,
        Last365Days,
        Last7Days,
        LastMonth,
        LastWeek,
        LastXDays,
        LastXHours,
        LastXMonths,
        LastXYears,
        LastYear,
        OlderThanXDays,
        ThisWeek,
        ThisYear,
        OlderThanXMonths,
        LastXWeeks,
        OlderThanXWeeks,
        ThisMonth
    }

    public enum CreateTime
    {
        On,
        NotOn,
        After,
        Before,
        Today,
        Yesterday,
        OnOrBefore,
        NotEmpty,
        OnOrAfter,
        Last30Days,
        Last365Days,
        Last7Days,
        LastMonth,
        LastWeek,
        LastXDays,
        LastXHours,
        LastXMonths,
        LastXYears,
        LastYear,
        OlderThanXDays,
        ThisWeek,
        ThisYear,
        OlderThanXMonths,
        LastXWeeks,
        OlderThanXWeeks,
        ThisMonth
    }

    public enum ModifyTime
    {
        On,
        NotOn,
        After,
        Before,
        Today,
        Yesterday,
        OnOrBefore,
        NotEmpty,
        OnOrAfter,
        Last30Days,
        Last365Days,
        Last7Days,
        LastMonth,
        LastWeek,
        LastXDays,
        LastXHours,
        LastXMonths,
        LastYear,
        OlderThanXDays,
        ThisWeek,
        ThisYear,
        OlderThanXMonths,
        LastXWeeks,
        OlderThanXWeeks,
        ThisMonth,
        LastXYears
    }

    public enum CloudSources
    {
        Dropbox
    }

    public class ScannedData
    {
        private ThreadLocal<string> scriptThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> scanThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> playbookThreaded = new ThreadLocal<string>();
        private ThreadLocal<List<string>> ssnThreaded = new ThreadLocal<List<string>>();
        private ThreadLocal<string> notValidSSNThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> creditCardThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> phoneNumberThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> templateThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> assignedUserThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> subjectThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> targetMachineThreaded = new ThreadLocal<string>();
        private ThreadLocal<string> basePathThreaded = new ThreadLocal<string>();
        private ThreadLocal<List<string>> discoveryFilesThreaded = new ThreadLocal<List<string>>();
        private static ScannedData instance = null;
        public static ScannedData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScannedData();
                }
                return instance;
            }
        }

        public string ScriptName { get => scriptThreaded.Value; set => scriptThreaded.Value = value; }
        public string ScanName { get => scanThreaded.Value; set => scanThreaded.Value = value; }
        public string PlaybookName { get => playbookThreaded.Value; set => playbookThreaded.Value = value; }
        public List<string> SSN { get => ssnThreaded.Value; set => ssnThreaded.Value = value; }
        public string NotValidSSN { get => notValidSSNThreaded.Value; set => notValidSSNThreaded.Value = value; }
        public string CreditCard { get => creditCardThreaded.Value; set => creditCardThreaded.Value = value; }
        public string PhoneNumber { get => phoneNumberThreaded.Value; set => phoneNumberThreaded.Value = value; }
        public string TemplateName { get => templateThreaded.Value; set => templateThreaded.Value = value; }
        public string AssignedUser { get => assignedUserThreaded.Value; set => assignedUserThreaded.Value = value; }
        public string Subject { get => subjectThreaded.Value; set => subjectThreaded.Value = value; }
        public string TargetMachine { get => targetMachineThreaded.Value; set => targetMachineThreaded.Value = value; }
        public string BasePath { get => basePathThreaded.Value; set => basePathThreaded.Value = value; }
        public static bool RemoteFlag { get; set; }
        public static RemoteMachineStatus MachineStatus { get; set; }
        public List<string> DiscoveryFiles { get => discoveryFilesThreaded.Value; set => discoveryFilesThreaded.Value = value; }
    }

    public class TestData
    {
        public string SSN { get; set; }
        public string CreditCardNumber { get; set; }
        public string Phone { get; set; }
        public string InvalidSSN { get; set; }
    }




}
