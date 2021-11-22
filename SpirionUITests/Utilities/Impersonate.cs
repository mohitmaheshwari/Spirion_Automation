using SimpleImpersonation;
using SpirionUITests.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpirionUITests.Utilities
{
    public class Impersonate
    {
        public static bool VerifyFileIsAccessibleFromRestrictedUser(string username, string password, string path)
        {
            bool result = true;
            try
            {
                LogonType logonType = LogonType.Interactive;
                var credentials = new UserCredentials(System.Environment.UserDomainName, username, password);
                Impersonation.RunAsUser(credentials, logonType, () =>
                {
                    result = Utility.VerifyFileIsAccessibleAndCorrect(path);
                });
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UploadDataToFolder(DataTypes dataTypes, List<TestData> inpputCollection, string outputDir, bool deleteExistingFiles, string username, string password)
        {
            bool result = false;
            try
            {
                if (Directory.Exists(outputDir))
                    Utility.DeleteAllFiles(outputDir);
                LogonType logonType = LogonType.Interactive;
                var credentials = new UserCredentials(System.Environment.UserDomainName, username, password);
                Impersonation.RunAsUser(credentials, logonType, () =>
                {
                    Utility.UploadDataToFolder(dataTypes, inpputCollection, outputDir, false, "");
                    result = true;
                });
                return result;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteDirectory(string target_dir, string username, string password)
        {
            LogonType logonType = LogonType.Interactive;
            var credentials = new UserCredentials(System.Environment.UserDomainName, username, password);
            Impersonation.RunAsUser(credentials, logonType, () =>
            {
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir,username,password);
                }

                Directory.Delete(target_dir, false);
            });
        }
    }
}
