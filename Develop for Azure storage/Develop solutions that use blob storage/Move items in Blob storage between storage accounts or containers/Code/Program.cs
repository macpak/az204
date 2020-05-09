using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Code
{
    class Program
    {
        private static string ConnectionString = "";
        static async Task Main(string[] args)
        {
            CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse
                    (ConnectionString);

            var client = cloudStorageAccount.CreateCloudBlobClient();
            var containerReference = client.GetContainerReference("copyblobcont");
            await containerReference.CreateIfNotExistsAsync();

            try
            {
                Console.WriteLine();
                Console.WriteLine();

                var blobName = "blob1";
                var blockBlob = containerReference.GetBlockBlobReference(blobName);
                await blockBlob.UploadTextAsync("aaa");

                var destinationBlobName = "destinationBlob";
                await CopyBlockBlobAsync(containerReference, blobName, destinationBlobName);

                var copiedBlob = containerReference.GetBlobReference(destinationBlobName);
                using var stream = new MemoryStream();
                await copiedBlob.DownloadToStreamAsync(stream);
                Console.WriteLine("String of copied blob: "+ Encoding.ASCII.GetString(stream.ToArray()));
            }
            finally
            {
                await containerReference.DeleteIfExistsAsync();
            }
        }

        /// <summary>
        /// Gets a reference to a blob created previously, and copies it to a new blob in the same container.
        /// </summary>
        /// <param name="container">A CloudBlobContainer object.</param>
        /// <param name="sourceBlobName"></param>
        /// <param name="destinationBlobName"></param>
        /// <returns>A Task object.</returns>
        private static async Task CopyBlockBlobAsync(CloudBlobContainer container,
            string sourceBlobName, 
            string destinationBlobName)
        {
            CloudBlockBlob sourceBlob = null;
            CloudBlockBlob destBlob = null;
            string leaseId = null;

            try
            {
                // Get a block blob from the container to use as the source.
                sourceBlob = container.GetBlockBlobReference(sourceBlobName);

                // Lease the source blob for the copy operation to prevent another client from modifying it.
                // Specifying null for the lease interval creates an infinite lease.
                leaseId = await sourceBlob.AcquireLeaseAsync(null);

                // Get a reference to a destination blob (in this case, a new blob).
                destBlob = container.GetBlockBlobReference(destinationBlobName);

                // Ensure that the source blob exists.
                if (await sourceBlob.ExistsAsync())
                {
                    // Get the ID of the copy operation.
                    string copyId = await destBlob.StartCopyAsync(sourceBlob);

                    // Fetch the destination blob's properties before checking the copy state.
                    await destBlob.FetchAttributesAsync();

                    Console.WriteLine("Status of copy operation: {0}", destBlob.CopyState.Status);
                    Console.WriteLine("Completion time: {0}", destBlob.CopyState.CompletionTime);
                    Console.WriteLine("Bytes copied: {0}", destBlob.CopyState.BytesCopied.ToString());
                    Console.WriteLine("Total bytes: {0}", destBlob.CopyState.TotalBytes.ToString());
                    Console.WriteLine();
                }
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
            finally
            {
                // Break the lease on the source blob.
                if (sourceBlob != null)
                {
                    await sourceBlob.FetchAttributesAsync();

                    if (sourceBlob.Properties.LeaseState != LeaseState.Available)
                    {
                        await sourceBlob.BreakLeaseAsync(new TimeSpan(0));
                    }
                }
            }
        }
    }
}