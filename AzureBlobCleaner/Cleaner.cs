using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Nikonda.Cron.API;
using System.Text.RegularExpressions;
using System.Globalization;
using log4net;

namespace AzureBlobCleaner
{
    public class Cleaner : IJob
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Cleaner));

        public int Id { get; set; }
        public bool IsRunning { get; set; }
        public string Parameters { get; set; }

        Regex r = new Regex(@"_(?<d>20[0-9][0-9][0-1][0-9][1-3][0-9]).BAK", RegexOptions.IgnoreCase);        

        public void Execute()
        {
            var container = ConfigurationManager.AppSettings.Get("AzureBlobCleanerContainer");
            var account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("AzureBlobCleanerConnectionString"));
            var client = account.CreateCloudBlobClient();
            var backupContainer = client.GetContainerReference(container);
            var list = backupContainer.ListBlobs(useFlatBlobListing: true);
            var deleted = 0;
            var kept = 0;

            foreach (var blob in list)
            {
                var blobFileName = blob.Uri.Segments.Last();
                Match match = r.Match(blobFileName);
                if (match.Success)
                {
                    var blobDate = DateTime.ParseExact(match.Groups["d"].Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                    if ((blobDate <= DateTime.Today.Subtract(TimeSpan.FromDays(8))) && blobDate.Day != 1) 
                    {
                        log.Debug("delete.. " + blobFileName);
                        var blobForDelete = backupContainer.GetBlockBlobReference(blobFileName);
                        if (blobForDelete.Exists())
                        {
                            blobForDelete.Delete();
                            deleted++;
                        }
                    } 
                    else
                    {
                        log.Debug(blobFileName + " " + blobDate.ToShortDateString());
                        kept++;
                    }
                }
            }
            log.Info("Deleted " + deleted + "; kept " + kept);
        }
    }
}
