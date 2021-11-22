
using NUnit.Framework;
using Spirion.CloudServices;
using SpirionUITests.Fixtures;
using SpirionUITests.Models;
using SpirionUITests.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpirionUITests.Steps
{
    [Binding]
    public class UtilitiesSteps
    {
        public EnvironmentFixture _environmentFixture;
        public UtilitiesSteps(EnvironmentFixture environmentFixture)
        {
            _environmentFixture = environmentFixture;
        }

        [Then(@"Check Remote Machine is running")]
        public void ThenCheckRemoteMachineIsRunning()
        {
            AssertExt.IsTrue(MachineDetails.IsMachineUp(_environmentFixture.Environment.RemoteTarget.MachineIP), "Veify Remote Machine is online");
        }

        [Then(@"Create directory on Remote machine")]
        public void ThenCreateDirectoryOnRemoteMachine()
        {
            Utility.CreateSetUpDirectory(string.Format(_environmentFixture.Environment.RemoteTarget.RemoteBasePath, _environmentFixture.Environment.RemoteTarget.MachineIP));
        }

        [Then(@"Create directory on Local machine")]
        public void ThenCreateDirectoryOnLocalMachine()
        {
            Utility.CreateSetUpDirectory(string.Format(_environmentFixture.Environment.LocalAgent.LocalAgentBasePath, _environmentFixture.Environment.LocalAgent.MachineIP));
            ScannedData.Instance.TargetMachine = string.Empty;
        }

        [Given(@"User is preparing testData for DiscoveryScan from '(.*)' for '(.*)'  and uploading to '(.*)'")]
        public void GivenUserIsPreparingTestDataForDiscoveryScanFromForAndUploadingTo(string inputPath, string targetType, string outputFile)
        {
            List<string> files = new DirectoryInfo(inputPath).GetFiles().Select(x => x.FullName).ToList();
            outputFile = ScannedData.Instance.BasePath + outputFile;
            Utility.UploadDataToFolder(files, outputFile);
            AssertExt.IsTrue(Utility.AreFilesFound(outputFile, files), $"Verify Files are found in {outputFile}");
        }

        [Given(@"User is preparing testData at '(.*)' for '(.*)' from '(.*)'  with '(.*)' and uploading to '(.*)', '(.*)'")]
        public void GivenUserIsPreparingTestDataAtForFromWithAndUploadingTo(string scanType, string dataType, string inputFile, string logicOption, string outputFile, bool deleteExistingFiles)
        {
            PreparingTestDataForFromForAndUploadingTo(scanType, dataType, inputFile, logicOption, outputFile, deleteExistingFiles);
        }

        [Given(@"User is preparing testData for Dropbox '(.*)' from '(.*)'  and uploading to '(.*)', '(.*)'")]
        public async Task GivenUserIsPreparingTestDataForDropboxFromAndUploadingTo(string dataType, string inputFile, string outputFile, bool deleteExistingFiles)
        {
            await PreparingTestDataForFromForAndUploadingToCloud(dataType, inputFile, "", outputFile, CloudSources.Dropbox, deleteExistingFiles);
        }

        private async Task PreparingTestDataForFromForAndUploadingToCloud(string dataType, string inputFile, string logicOption, string outputFile, CloudSources cloudSources, bool deleteExistingFiles)
        {

            DataTypes dataTypes = (DataTypes)(Enum.Parse(typeof(DataTypes), dataType));
            List<TestData> data = Utility.ReadCSVFile<TestData>(inputFile);
            outputFile = string.Format(_environmentFixture.Environment.LocalAgent.LocalAgentBasePath, _environmentFixture.Environment.LocalAgent.MachineIP) + outputFile;
            DropboxServices.UserAccount = _environmentFixture.Environment.Cloud.DropBox.UserAccount;
            DropboxServices.DropboxToken = _environmentFixture.Environment.Cloud.DropBox.DropBoxToken;
            Utility.UploadDataToFolder(dataTypes, data, outputFile, deleteExistingFiles, logicOption);
            await CloudOperations.UploadFolder("Dropbox", ScannedData.Instance.DiscoveryFiles.First());
        }

        [Given(@"User is preparing testData at '(.*)' for '(.*)' from '(.*)'  and uploading to '(.*)', '(.*)'")]
        public void GivenUserIsPreparingTestDataAtForFromAndUploadingTo(string scanType, string dataType, string inputFile, string outputFile, bool deleteExistingFiles)
        {
            PreparingTestDataForFromForAndUploadingTo(scanType, dataType, inputFile, "", outputFile, deleteExistingFiles);
        }

        public void PreparingTestDataForFromForAndUploadingTo(string scanType, string dataType, string inputFile, string logicOption, string outputFile, bool deleteExistingFiles)
        {
            DataTypes dataTypes = (DataTypes)(Enum.Parse(typeof(DataTypes), dataType));
            List<TestData> data = Utility.ReadCSVFile<TestData>(inputFile);
            outputFile = ScannedData.Instance.BasePath + outputFile;
            Utility.UploadDataToFolder(dataTypes, data, outputFile, deleteExistingFiles, logicOption);
            AssertExt.IsTrue(Utility.IsFileFound(outputFile), $"Verify files are found in {outputFile}");
        }

        [Then(@"user is setting up file access time '(.*)' '(.*)'")]
        public void ThenUserIsSettingUpFileAccessTime(string accessTime, bool isValid)
        {
            accessTime = accessTime.Split(';').Last();
            Utility.SetAccessTimeForOrCondition(accessTime, isValid);
        }

        [Then(@"user is setting up file create time '(.*)' '(.*)'")]
        public void ThenUserIsSettingUpFileCreateTime(string createTime, bool isValid)
        {
            createTime = createTime.Split(';').Last();
            Utility.SetCreateTimeForOrCondition(createTime, isValid);
        }

        [Then(@"user is setting up file modify time '(.*)' '(.*)'")]
        public void ThenUserIsSettingUpFileModifyTime(string modifyTime, bool isValid)
        {
            modifyTime = modifyTime.Split(';').Last();
            Utility.SetModifyTimeForOrCondition(modifyTime, isValid);
        }





        [Then(@"user is setting up the file access time '(.*)'")]
        public void ThenUserIsSettingUpTheFileAccessTime(string accessTime)
        {
            accessTime = accessTime.Split(';').Last();
            Utility.SetAccessTime(accessTime);
        }

        [Then(@"user is setting up the file create time '(.*)'")]
        public void ThenUserIsSettingUpTheFileCreateTime(string createTime)
        {
            createTime = createTime.Split(';').Last();
            Utility.SetCreateTime(createTime);
        }

        [Then(@"user is setting up the file modify time '(.*)'")]
        public void ThenUserIsSettingUpTheFileModifyTime(string modifyTime)
        {
            modifyTime = modifyTime.Split(';').Last();
            Utility.SetModifyTime(modifyTime);
        }

      
        [Given(@"Restricted User is preparing testData at '(.*)' for '(.*)' from '(.*)'  and uploading to '(.*)', '(.*)'")]
        public void GivenRestrictedUserIsPreparingTestDataAtForFromAndUploadingTo(string scanType, string dataType, string inputFile, string outputFile, bool deleteExistingFiles)
        {
            string userName = _environmentFixture.Environment.RestrictedUser.Username;
            string password = _environmentFixture.Environment.RestrictedUser.Password;
            DataTypes dataTypes = (DataTypes)(Enum.Parse(typeof(DataTypes), dataType));
            List<TestData> data = Utility.ReadCSVFile<TestData>(inputFile);
            outputFile = ScannedData.Instance.BasePath + outputFile;
            Utility.DeleteDirectory(outputFile); 
            Impersonate.UploadDataToFolder(dataTypes, data, outputFile, deleteExistingFiles,userName ,password );
            AssertExt.IsTrue(Utility.IsFileFound(outputFile), $"Verify files are found in {outputFile}");
        }


        [Then(@"Verify the file is shredded '(.*)'")]
        public void ThenVerifyTheFileIsShredded(string directory)
        {
            directory = ScannedData.Instance.BasePath + directory;
            AssertExt.IsFalse(Utility.IsFileFound(directory), "Verify File is not found in directory");
        }

        [Then(@"Verify file is quarantined sucessfully '(.*)' '(.*)' '(.*)'")]
        public void ThenVerifyFileIsQuarantinedAndSucessfully(string quarantinedFileContent, string folderPath, string scanType)
        {
            string machineIP = _environmentFixture.Environment.LocalAgent.MachineIP;
            folderPath = ScannedData.Instance.BasePath + folderPath;
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            if (dir.GetFiles().Count() == 1)
            {
                FileInfo file = dir.GetFiles().First();
                string content = File.ReadAllText(file.FullName).ToLower();
                string fileName = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(file.FullName));
                string expectedContent = String.Format(quarantinedFileContent, ScannedData.Instance.DiscoveryFiles.Last()).ToLower();
                AssertExt.IsTrue(content.Contains(expectedContent), "Verify ");
                string quarantinedFilePath = content.Replace(expectedContent, "").Replace("\0", "").TrimStart(' ');
                quarantinedFilePath = quarantinedFilePath.Replace("c:", $"\\\\{machineIP}\\c$");
                AssertExt.IsTrue(File.Exists(quarantinedFilePath), $"Verify File exists on {quarantinedFilePath}");
                string ssn = File.ReadAllText(quarantinedFilePath);
                AssertExt.IsTrue(ssn.Equals(ScannedData.Instance.SSN.First()), "Verify ssn matches");
            }
            else AssertExt.IsTrue(false, "No File found in Quarantined Folder");
        }

        [Then(@"Verify file is redacted sucessfully '(.*)'")]
        public void ThenVerifyFileIsRedactedSucessfully(string path)
        {
            path = ScannedData.Instance.BasePath + path;
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.GetFiles().Count() == 1)
            {
                FileInfo file = dir.GetFiles().First();
                string content = File.ReadAllText(file.FullName);
                StringBuilder sb = new StringBuilder();
                for (int count = 0; count < content.Length; count++)
                {
                    sb.Append("X");
                }
                AssertExt.IsTrue(content.Equals(sb.ToString()), "Verify File content Redacted");
            }
        }

        [Then(@"Administrator is able to access the file '(.*)'")]
        public void ThenAdministratorIsAbleToAccessTheFile(string path)
        {
            path = ScannedData.Instance.BasePath + path;
            AssertExt.IsTrue(Utility.VerifyFileIsAccessibleAndCorrect(path), "Verify File is accessible to administrator");
        }

        [Then(@"Administrator is not able to access the file '(.*)'")]
        public void ThenAdministratorIsNotAbleToAccessTheFile(string path)
        {
            path = ScannedData.Instance.BasePath + path;
            AssertExt.IsFalse(Utility.VerifyFileIsAccessibleAndCorrect(path), "Verify File is accessible to administrator");
        }


        [Then(@"Login as RestrictedUser and file should not be accessible '(.*)'")]
        public void ThenLoginAsRestrictedUserAndFileShouldNotBeAccessible(string path)
        {
            path = ScannedData.Instance.BasePath + path;
            AssertExt.IsFalse(Impersonate.VerifyFileIsAccessibleFromRestrictedUser(_environmentFixture.Environment.RestrictedUser.Username, _environmentFixture.Environment.RestrictedUser.Password, path), "Verify file is not accessible from Restricted User");
        }

        [Then(@"Login as RestrictedUser and file should be accessible '(.*)'")]
        public void ThenLoginAsRestrictedUserAndFileShouldBeAccessible(string path)
        {
            path = ScannedData.Instance.BasePath + path;
            AssertExt.IsTrue(Impersonate.VerifyFileIsAccessibleFromRestrictedUser(_environmentFixture.Environment.RestrictedUser.Username, _environmentFixture.Environment.RestrictedUser.Password, path), "Verify file is accessible to restricted user");
        }


    }
}

