using Dropbox.Api;
using Dropbox.Api.Files;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spirion.CloudServices
{
    /// <summary>
    /// Perform the below operation for Dropbox Cloud Storage.
    /// <list type="bullet">
    /// <item>Create Dropbox client</item>
    /// <item>Upload test data to Dropbox cloud storage</item>
    /// <item>Download test data for verification from Dropbox cloud storage after remediation.</item>
    /// <item>Delete test data from Dropbox cloud storage after all operations.</item>
    /// </list>
    /// </summary>

    public class DropboxServices
    {
        private static DropboxClient dropbox = null;
        private static DropboxTeamClient dropboxTeam = null;

        [ThreadStatic] private static string folderName = "Spirion-UIAutomation";
        public static string UserAccount { get; set; }
        public static string DropboxToken { get; set; }
        
        /// <summary>
        /// Gets Dropbox's Client based on the authentication DropboxToken.
        /// </summary>
        /// <return>Json Object for Dropbox.</return>
        private static DropboxClient DropboxClient
        {
            get
            {
                if (dropbox == null)
                {
                    dropbox = new DropboxClient(DropboxToken);
                }

                return dropbox;
            }
        }

        public static DropboxTeamClient TeamClient
        {
            get
            {
                if (dropboxTeam == null)
                {
                    dropboxTeam = new DropboxTeamClient(DropboxToken);
                }
                return dropboxTeam;
            }
        }

        /// <summary>
        /// Upload the test data files from project directory to new directory created on cloud storage
        /// The new directory is created with agent's name if SDM agent is used.
        /// The new directory is created with name as current Timestamp if Cloud agent is used.
        /// </summary>
        public static async Task UploadFiles(string filePath)
        {

            var members = await TeamClient.Team.MembersListAsync();
            string mId = "";
            foreach (var member in members.Members)
            {
                if (member.Profile.Email == UserAccount)
                    mId = member.Profile.TeamMemberId;
            }
            folderName = "Spirion-UIAutomation";
            var list = TeamClient.AsMember(mId).Files.ListFolderAsync(string.Empty).Result;
            if (list.Entries.Where(e => e.Name.Equals(folderName)).Count() > 0)
            {
                TeamClient.AsMember(mId).Files.DeleteV2Async("/" + folderName).Wait();
            }

            //create a dropbox folder
            TeamClient.AsMember(mId).Files.CreateFolderV2Async("/" + folderName).Wait();

            //upload files

            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(filePath)))
            {
                string fileName = Path.GetFileName(filePath);
                TeamClient.AsMember(mId).Files.UploadAsync($"/{folderName}/{fileName}", WriteMode.Overwrite.Instance, body: stream).Wait();
            }
        }


        /// <summary>
        /// Download the test data directory from the cloud storage at give download path.
        /// </summary>
        /// <param name="downloadPath">The path where test data directory will be downloaded for further verification.</param>
        /// <returns>Number of files downloaded from cloud storage</returns>
        public static int Download(string downloadPath)
        {
            ListFolderResult list = DropboxClient.Files.ListFolderAsync("/" + folderName).Result;
            foreach (Metadata item in list.Entries)
            {
                Dropbox.Api.Stone.IDownloadResponse<FileMetadata> file = DropboxClient.Files.DownloadAsync(item.PathDisplay).Result;
                File.WriteAllText(downloadPath + "\\" + item.Name, file.GetContentAsStringAsync().Result);
            }
            return list.Entries.Count;
        }
        /// <summary>
        /// Delete the directory created on the Cloud Storage for test data of given scan.
        /// </summary>
        public static void DeleteFolder()
        {
            DropboxClient.Files.DeleteV2Async("/" + folderName).Wait();
        }
    }
}
