using CsvHelper;
using Serilog;
using Serilog.Core;
using SpirionUITests.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace SpirionUITests.Utilities
{
    public class MachineDetails
    {

        public static string GetDomainName() => System.Environment.UserDomainName;
        
        public static bool IsMachineUp(string hostName)
        {
            if (ScannedData.RemoteFlag == false)
            {
                try
                {
                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions();
                    // Use the default Ttl value which is 128,
                    // but change the fragmentation behavior.
                    options.DontFragment = true;
                    // Create a buffer of 32 bytes of data to be transmitted.
                    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    int timeout = 120;

                    PingReply reply = pingSender.Send(hostName, timeout, buffer, options);
                    ScannedData.RemoteFlag = true;
                    if (reply.Status == IPStatus.Success)
                    {
                        ScannedData.MachineStatus = RemoteMachineStatus.On;
                        return true;
                    }
                    else
                    {
                        ScannedData.MachineStatus = RemoteMachineStatus.off;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                if (ScannedData.MachineStatus == RemoteMachineStatus.On)
                    return true;
                else
                    return false;
            }
        }
    }
    public class Utility
    {
        public static List<T> ReadCSVFile<T>(string inputFile)
        {
            using (var reader = new StreamReader(inputFile))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>();
                return records.ToList();
            }
        }

        public static bool IsFileFound(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            return dir.GetFiles().Any();
        }

        public static bool AreFilesFound(string directoryPath, List<string> files)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            return dir.GetFiles().Select(y => y.FullName).OrderBy(x => x).SequenceEqual(ScannedData.Instance.DiscoveryFiles.OrderBy(x => x));
        }


        public static void SetAccessTime(string accessTime)
        {
            accessTime = accessTime.Replace(" ", "");
            AccessTime value = (AccessTime)Enum.Parse(typeof(AccessTime), accessTime);
            DateTime previousDays(int day) => DateTime.Now.AddDays(day * -1);
            DateTime previousYears(int year) => DateTime.Now.AddYears(year * -1);
            DateTime previousMonths(int month) => DateTime.Now.AddMonths(month * -1);
            DateTime previousHours(int hours) => DateTime.Now.AddHours(hours * -1);
            DateTime nextDays(int day) => DateTime.Now.AddDays(day);
            DateTime today = DateTime.Now;
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            DateTime lastWeekStartDate = currentWeekStartDate.AddDays(-7);
            DateTime firstDayOfthisMonth = today.AddDays(-(DateTime.Today.Day - 1));
            DateTime FirstDayOfLastMonth = firstDayOfthisMonth.AddMonths(-1);
            DateTime previousWeeks(int weeks) => currentWeekStartDate.AddDays(weeks * 7 * -1);
            switch (value)
            {
                case AccessTime.On:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), nextDays(2));
                    break;
                case AccessTime.Before:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case AccessTime.After://@Bug
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(1));
                    break;
                case AccessTime.Today:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(3));
                    break;
                case AccessTime.NotOn:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(4));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case AccessTime.OnOrBefore:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), nextDays(4));
                    break;
                case AccessTime.NotEmpty:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case AccessTime.OnOrAfter:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(4));
                    break;
                case AccessTime.Last30Days:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousDays(24).Year, previousDays(24).Month, previousDays(24).Day));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(previousDays(40).Year, previousDays(40).Month, previousDays(40).Day));
                    break;
                case AccessTime.Last365Days:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(310));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(400));
                    break;
                case AccessTime.Last7Days:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(9));
                    break;
                case AccessTime.LastMonth:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(FirstDayOfLastMonth.Year, FirstDayOfLastMonth.Month, FirstDayOfLastMonth.Day + 3));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(firstDayOfthisMonth.Year, firstDayOfthisMonth.Month, firstDayOfthisMonth.Day + 3));
                    break;
                case AccessTime.ThisMonth:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(FirstDayOfLastMonth.Year, FirstDayOfLastMonth.Month, FirstDayOfLastMonth.Day + 3));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(firstDayOfthisMonth.Year, firstDayOfthisMonth.Month, firstDayOfthisMonth.Day + 3));
                    break;
                case AccessTime.LastWeek:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), lastWeekStartDate);
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), lastWeekStartDate.AddDays(-2));
                    break;
                case AccessTime.ThisWeek:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), currentWeekStartDate);
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), currentWeekStartDate.AddDays(-2));
                    break;
                case AccessTime.LastXDays:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(3));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(5));
                    break;
                case AccessTime.OlderThanXDays:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(3));
                    break;
                case AccessTime.LastXHours:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousHours(3).Year, previousHours(3).Month, previousHours(3).Day));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(5));
                    break;
                case AccessTime.LastXMonths:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousMonths(3).Year, previousMonths(3).Month, previousMonths(3).Day));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousMonths(5));
                    break;
                case AccessTime.OlderThanXMonths:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(previousMonths(3).Year, previousMonths(3).Month, previousMonths(3).Day));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousMonths(5));
                    break;
                case AccessTime.LastXYears:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousYears(3).Year, previousYears(3).Month, previousYears(3).Day));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(5));
                    break;
                case AccessTime.LastYear:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousYears(1));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(2));
                    break;
                case AccessTime.ThisYear:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousYears(0));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(1));
                    break;
                case AccessTime.Yesterday:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(1));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(2));
                    break;
                case AccessTime.LastXWeeks:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousWeeks(3));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousWeeks(5));
                    break;
                case AccessTime.OlderThanXWeeks:
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), previousWeeks(5));
                    SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.Last(), previousWeeks(3));
                    break;
                default:
                    Log.Logger.Error("AccessType not found");
                    return;
            }

        }

            public static void DeleteDirectory(string target_dir)
            {
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(target_dir, false);
            }
        

        internal static void SetAccessTimeForOrCondition(string accessTime, bool isValid)
        {
            AccessTime value = (AccessTime)Enum.Parse(typeof(AccessTime), accessTime);
            switch (value)
            {
                case AccessTime.On:
                    if (!isValid)
                        SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now.AddDays(-2));
                    else
                        SetLastAccessTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    break;
            }
        }

        internal static void SetCreateTimeForOrCondition(string createTime, bool isValid)
        {
            CreateTime value = (CreateTime)Enum.Parse(typeof(CreateTime), createTime);
            switch (value)
            {
                case CreateTime.On:
                    if (!isValid)
                        SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now.AddDays(-2));
                    else
                        SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    break;
            }
        }

        internal static void SetModifyTimeForOrCondition(string modifyTime, bool isValid)
        {
            ModifyTime value = (ModifyTime)Enum.Parse(typeof(ModifyTime), modifyTime);
            switch (value)
            {
                case ModifyTime.On:
                    if (!isValid)
                        SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now.AddDays(-2));
                    else
                        SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    break;
            }
        }


        public static void SetCreateTime(string createTime)
        {
            createTime = createTime.Replace(" ", "");
            CreateTime value = (CreateTime)Enum.Parse(typeof(CreateTime), createTime);
            DateTime previousDays(int day) => DateTime.Now.AddDays(day * -1);
            DateTime previousYears(int year) => DateTime.Now.AddYears(year * -1);
            DateTime previousMonths(int month) => DateTime.Now.AddMonths(month * -1);
            DateTime previousHours(int hours) => DateTime.Now.AddHours(hours * -1);
            DateTime nextDays(int day) => DateTime.Now.AddDays(day);
            DateTime today = DateTime.Now;
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            DateTime lastWeekStartDate = currentWeekStartDate.AddDays(-7);
            DateTime firstDayOfthisMonth = today.AddDays(-(DateTime.Today.Day - 1));
            DateTime FirstDayOfLastMonth = firstDayOfthisMonth.AddMonths(-1);
            DateTime previousWeeks(int weeks) => currentWeekStartDate.AddDays(weeks * 7 * -1);
            switch (value)
            {
                case CreateTime.On:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), nextDays(2));
                    break;
                case CreateTime.Before:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case CreateTime.After://@Bug
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(1));
                    break;
                case CreateTime.Today:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(3));
                    break;
                case CreateTime.NotOn:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(4));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case CreateTime.OnOrBefore:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), nextDays(4));
                    break;
                case CreateTime.NotEmpty:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case CreateTime.OnOrAfter:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(4));
                    break;
                case CreateTime.Last30Days:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousDays(24).Year, previousDays(24).Month, previousDays(24).Day));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(previousDays(40).Year, previousDays(40).Month, previousDays(40).Day));
                    break;
                case CreateTime.Last365Days:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(310));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(400));
                    break;
                case CreateTime.Last7Days:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(9));
                    break;
                case CreateTime.LastMonth:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(FirstDayOfLastMonth.Year, FirstDayOfLastMonth.Month, FirstDayOfLastMonth.Day + 3));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(firstDayOfthisMonth.Year, firstDayOfthisMonth.Month, firstDayOfthisMonth.Day + 3));
                    break;
                case CreateTime.ThisMonth:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(FirstDayOfLastMonth.Year, FirstDayOfLastMonth.Month, FirstDayOfLastMonth.Day + 3));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(firstDayOfthisMonth.Year, firstDayOfthisMonth.Month, firstDayOfthisMonth.Day + 3));
                    break;
                case CreateTime.LastWeek:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), lastWeekStartDate);
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), lastWeekStartDate.AddDays(-2));
                    break;
                case CreateTime.ThisWeek:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), currentWeekStartDate);
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), currentWeekStartDate.AddDays(-2));
                    break;
                case CreateTime.LastXDays:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(3));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(5));
                    break;
                case CreateTime.OlderThanXDays:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(3));
                    break;
                case CreateTime.LastXHours:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousHours(3).Year, previousHours(3).Month, previousHours(3).Day));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(5));
                    break;
                case CreateTime.LastXMonths:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousMonths(3).Year, previousMonths(3).Month, previousMonths(3).Day));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousMonths(5));
                    break;
                case CreateTime.OlderThanXMonths:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(previousMonths(3).Year, previousMonths(3).Month, previousMonths(3).Day));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousMonths(5));
                    break;
                case CreateTime.LastXYears:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousYears(3).Year, previousYears(3).Month, previousYears(3).Day));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(5));
                    break;
                case CreateTime.LastYear:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousYears(1));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(2));
                    break;
                case CreateTime.ThisYear:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousYears(0));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(1));
                    break;
                case CreateTime.Yesterday:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(1));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(2));
                    break;
                case CreateTime.LastXWeeks:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousWeeks(3));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousWeeks(5));
                    break;
                case CreateTime.OlderThanXWeeks:
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.First(), previousWeeks(5));
                    SetFileCreateTime(ScannedData.Instance.DiscoveryFiles.Last(), previousWeeks(3));
                    break;
                default:
                    Log.Logger.Error("AccessType not found");
                    return;
            }

        }

        public static void SetModifyTime(string modifyTime)
        {
            modifyTime = modifyTime.Replace(" ", "");
            ModifyTime value = (ModifyTime)Enum.Parse(typeof(ModifyTime), modifyTime);
            DateTime previousDays(int day) => DateTime.Now.AddDays(day * -1);
            DateTime previousYears(int year) => DateTime.Now.AddYears(year * -1);
            DateTime previousMonths(int month) => DateTime.Now.AddMonths(month * -1);
            DateTime previousHours(int hours) => DateTime.Now.AddHours(hours * -1);
            DateTime nextDays(int day) => DateTime.Now.AddDays(day);
            DateTime today = DateTime.Now;
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            DateTime lastWeekStartDate = currentWeekStartDate.AddDays(-7);
            DateTime firstDayOfthisMonth = today.AddDays(-(DateTime.Today.Day - 1));
            DateTime FirstDayOfLastMonth = firstDayOfthisMonth.AddMonths(-1);
            DateTime previousWeeks(int weeks) => currentWeekStartDate.AddDays(weeks * 7 * -1);
            switch (value)
            {
                case ModifyTime.On:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), nextDays(2));
                    break;
                case ModifyTime.Before:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case ModifyTime.After://@Bug
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(1));
                    break;
                case ModifyTime.Today:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), DateTime.Now);
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(3));
                    break;
                case ModifyTime.NotOn:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(4));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case ModifyTime.OnOrBefore:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), nextDays(4));
                    break;
                case ModifyTime.NotEmpty:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), DateTime.Now);
                    break;
                case ModifyTime.OnOrAfter:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), nextDays(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(4));
                    break;
                case ModifyTime.Last30Days:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousDays(24).Year, previousDays(24).Month, previousDays(24).Day));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(previousDays(40).Year, previousDays(40).Month, previousDays(40).Day));
                    break;
                case ModifyTime.Last365Days:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(310));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(400));
                    break;
                case ModifyTime.Last7Days:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(9));
                    break;
                case ModifyTime.LastMonth:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(FirstDayOfLastMonth.Year, FirstDayOfLastMonth.Month, FirstDayOfLastMonth.Day + 3));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(firstDayOfthisMonth.Year, firstDayOfthisMonth.Month, firstDayOfthisMonth.Day + 3));
                    break;
                case ModifyTime.ThisMonth:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(FirstDayOfLastMonth.Year, FirstDayOfLastMonth.Month, FirstDayOfLastMonth.Day + 3));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(firstDayOfthisMonth.Year, firstDayOfthisMonth.Month, firstDayOfthisMonth.Day + 3));
                    break;
                case ModifyTime.LastWeek:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), lastWeekStartDate);
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), lastWeekStartDate.AddDays(-2));
                    break;
                case ModifyTime.ThisWeek:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), currentWeekStartDate);
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), currentWeekStartDate.AddDays(-2));
                    break;
                case ModifyTime.LastXDays:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(3));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(5));
                    break;
                case ModifyTime.OlderThanXDays:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(3));
                    break;
                case ModifyTime.LastXHours:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousHours(3).Year, previousHours(3).Month, previousHours(3).Day));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(5));
                    break;
                case ModifyTime.LastXMonths:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousMonths(3).Year, previousMonths(3).Month, previousMonths(3).Day));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousMonths(5));
                    break;
                case ModifyTime.OlderThanXMonths:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), new DateTime(previousMonths(3).Year, previousMonths(3).Month, previousMonths(3).Day));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousMonths(5));
                    break;
                case ModifyTime.LastXYears:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), new DateTime(previousYears(3).Year, previousYears(3).Month, previousYears(3).Day));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(5));
                    break;
                case ModifyTime.LastYear:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousYears(1));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(2));
                    break;
                case ModifyTime.ThisYear:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousYears(0));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousYears(1));
                    break;
                case ModifyTime.Yesterday:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousDays(1));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousDays(2));
                    break;
                case ModifyTime.LastXWeeks:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousWeeks(3));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousWeeks(5));
                    break;
                case ModifyTime.OlderThanXWeeks:
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.First(), previousWeeks(5));
                    SetFileModifyTime(ScannedData.Instance.DiscoveryFiles.Last(), previousWeeks(3));
                    break;
                default:
                    Log.Logger.Error("AccessType not found");
                    return;
            }

        }


        private static void SetLastAccessTime(string path, DateTime dt)
        {
            File.SetLastAccessTime(path, dt);
            Thread.Sleep(1000);
        }

        private static void SetFileCreateTime(string path, DateTime dt)
        {
            File.SetCreationTime(path, dt);
            Thread.Sleep(1000);
        }

        private static void SetFileModifyTime(string path, DateTime dt)
        {
            File.SetLastWriteTime(path, dt);
            Thread.Sleep(1000);
        }


        public static void UploadDataToFolder(List<string> files, string outputDir)
        {
            ScannedData.Instance.DiscoveryFiles = new List<string>();
            DeleteAllFiles(outputDir);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string fileExtension = Path.GetExtension(file);
                string outputFile = fileName + $"_{Guid.NewGuid().ToString().Substring(0, 4)}{fileExtension}";
                File.Copy(file, Path.Combine(outputDir, outputFile));
                ScannedData.Instance.DiscoveryFiles.Add(Path.Combine(outputDir, outputFile));
            }
        }
        internal static void UploadDataToFolder(DataTypes dataTypes, List<TestData> inpputCollection, string outputDir, bool deleteExistingFiles, string logicTypeOption)
        {
            if (deleteExistingFiles) DeleteAllFiles(outputDir);
            if (Directory.Exists(outputDir) == false)
                Directory.CreateDirectory(outputDir);
            string outputFile = "";
            if (string.IsNullOrEmpty(logicTypeOption))
                outputFile = outputDir + $"\\{dataTypes}_{Guid.NewGuid().ToString().Substring(0, 4)}.txt";
            else
            {
                logicTypeOption = logicTypeOption.Split(';').Last();
                string scenario = (deleteExistingFiles) ? "Positive" : "Negative";
                outputFile = outputDir + $"\\{dataTypes}_{logicTypeOption}_{Guid.NewGuid().ToString().Substring(0, 4)}_{scenario}.txt";
            }
            switch (dataTypes)
            {
                case DataTypes.SocialSecurityNumber:
                    ScannedData.Instance.SSN.Add(inpputCollection[0].SSN);
                    File.WriteAllText(outputFile, ScannedData.Instance.SSN.Last());
                    break;
                case DataTypes.CreditCardNumber:
                    ScannedData.Instance.CreditCard = inpputCollection[0].CreditCardNumber;
                    File.WriteAllText(outputFile, ScannedData.Instance.CreditCard);
                    break;
                case DataTypes.TelephoneNumber:
                    ScannedData.Instance.PhoneNumber = inpputCollection[0].Phone;
                    File.WriteAllText(outputFile, ScannedData.Instance.PhoneNumber);
                    break;
                case DataTypes.InvalidSSN:
                    ScannedData.Instance.NotValidSSN = inpputCollection[0].InvalidSSN;
                    File.WriteAllText(outputFile, ScannedData.Instance.NotValidSSN);
                    break;
                default:
                    return;
            }
            ScannedData.Instance.DiscoveryFiles.Add(outputFile);
        }

        public static void DeleteAllFiles(string outputDir)
        {
            var files = new DirectoryInfo(outputDir).GetFiles();
            if (files.Any())
            {
                foreach (var file in files)
                {
                    file.Delete();
                }
            }
        }

        public static bool CheckIfDirectoryExists(string dirPath)
        {
            return Directory.Exists(dirPath);
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }



        public static bool VerifyFileIsAccessibleAndCorrect(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.GetFiles().Count() == 1)
            {
                try
                {
                    FileInfo file = dir.GetFiles().First();
                    string content = File.ReadAllText(file.FullName);
                    return content.Equals(ScannedData.Instance.SSN.First());
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        internal static void CreateSetUpDirectory(string basePath)
        {
            ScannedData.Instance.BasePath = basePath;
            if (Directory.Exists(basePath) == false)
                Directory.CreateDirectory(basePath);
            string discoveryScan = Path.Combine(basePath, "DiscoveryScan");
            if (Directory.Exists(discoveryScan) == false)
                Directory.CreateDirectory(discoveryScan);
            string sensitiveDataScanPath = Path.Combine(basePath, "SensitiveDataScan");
            if (Directory.Exists(sensitiveDataScanPath) == false)
                Directory.CreateDirectory(sensitiveDataScanPath);
            List<string> directories = new List<string> { "AssignRole", "AssignUser", "ClassifyMultipleDataType", "ClassifySingleDataType", "Discovery", "ExecuteScript", "Ignore", "MIPLabel", "Notify", "PlayBookLogic", "Quarantine", "Redact",  "Shred", "TakeNoAction", "UserAction", "RestrictAccessOnlyFileOwner", "RestrictAccessOnlyFileAdmministrator" };
            foreach (var dir in directories)
            {
                string path = Path.Combine(sensitiveDataScanPath, dir);
                if (Directory.Exists(path))
                    continue;
                Directory.CreateDirectory(path);
            }
            string playBookPath = Path.Combine(sensitiveDataScanPath, "PlayBookLogic");
            List<string> playBookSubDirs = new List<string>() { "AccessDate", "CreateDate", "ModifyDate" };

            foreach (string dir in playBookSubDirs)
            {
                string dirPath = Path.Combine(playBookPath, dir);
                List<string> playbookLogicDirs = new List<string>() { "Before", "On", "After", "NotOn", "Today", "Yesterday", "OnOrBefore", "OnOrAfter", "Last7Days", "Last30Days", "Last365Days", "ThisMonth", "ThisWeek", "LastMonth", "LastWeek", "LastYear", "ThisYear", "LastXMonths", "LastXHours", "LastXYears", "LastXDays", "OlderThanXDays", "OlderThanXMonths", "LastXWeeks", "OlderThanXWeeks" };
                foreach (var subDir in playbookLogicDirs)
                {
                    string path = Path.Combine(dirPath, $"{dir}_{subDir}");
                    if (Directory.Exists(path))
                        continue;
                    Directory.CreateDirectory(path);
                }
            }
        }

    }
}
