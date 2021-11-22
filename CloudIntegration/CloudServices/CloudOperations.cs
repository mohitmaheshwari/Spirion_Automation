using System.Threading.Tasks;

namespace Spirion.CloudServices
{
    /// <summary>
    /// Perform the below operation for all Cloud Storages or email server.
    /// <list type="bullet">
    /// <item>Upload test data to cloud storage or email server</item>
    /// <item>Download test data for verification from cloud storage or email server after remediation.</item>
    /// <item>Delete test data from cloud storage or email server after all operations.</item>
    /// </list>
    /// </summary>
    public class CloudOperations
    {

        /// <summary>
        /// Delete the directory created on the Cloud Storage for test data of given scan.
        /// </summary>
        /// <param name="endpointType"> The endpoint type created for the scan.</param>
        public static void DeleteFolder(string endpointType)
        {
            switch (endpointType)
            {

                case "Dropbox":
                    DropboxServices.DeleteFolder();
                    break;

            }
        }

        /// <summary>
        /// Download the test data directory from the cloud storage at given download path.
        /// </summary>
        /// <param name="endpointType">The endpoint type created for the scan.</param>
        /// <param name="DownloadPath">The path where test data directory will be downloaded for further verification</param>
        /// <returns>Number of files downloaded from cloud storage.</returns>
        public static int DownloadFiles(string endpointType, string DownloadPath)
        {
            switch (endpointType)
            {

                case "Dropbox":
                    return DropboxServices.Download(DownloadPath);
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Upload the test data files from project directory to new directory created on cloud storage.
        /// </summary>
        /// <param name="endpointType">The endpoint type created for the scan.</param>
        public static async Task UploadFolder(string endpointType, string filePath)
        {
            switch (endpointType)
            {

                case "Dropbox":
                    await DropboxServices.UploadFiles(filePath);
                    break;

            }

        }
    }
}

