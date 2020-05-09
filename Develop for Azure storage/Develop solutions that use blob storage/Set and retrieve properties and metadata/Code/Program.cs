using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Code
{
    class Program
    {
        private static string ConnectionString =
            "";            

        static async Task Main(string[] args)
        {
            CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse
                    (ConnectionString);

            var client = cloudStorageAccount.CreateCloudBlobClient();
            var containerReference = client.GetContainerReference("macpakcontainer1");
            await containerReference.CreateIfNotExistsAsync();

            try
            {
                await ReadContainerPropertiesAsync(containerReference);
                await AddContainerMetadataAsync(containerReference);
                await ReadContainerMetadataAsync(containerReference);
                
                Console.WriteLine();
                Console.WriteLine();

                var blockBlob = containerReference.GetBlockBlobReference("blob1");
                await blockBlob.UploadTextAsync("aaa");
                
                var blob = containerReference.GetBlobReference("blob1");
                await SetBlobPropertiesAsync(blob);
                await GetBlobPropertiesAsync(blob);
                await AddBlobMetadataAsync(blob);
                await ReadBlobMetadataAsync(blob);

            }
            finally
            {
                await containerReference.DeleteIfExistsAsync();
            }
        }
        
        private static async Task ReadContainerPropertiesAsync(CloudBlobContainer container)
        {
            try
            {
                // Fetch some container properties and write out their values.
                await container.FetchAttributesAsync();
                Console.WriteLine("Properties for container {0}", container.StorageUri.PrimaryUri);
                Console.WriteLine("Public access level: {0}", container.Properties.PublicAccess);
                Console.WriteLine("Last modified time in UTC: {0}", container.Properties.LastModified);
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                    e.RequestInformation.HttpStatusCode,
                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
        
        public static async Task AddContainerMetadataAsync(CloudBlobContainer container)
        {
            try
            {
                // Add some metadata to the container.
                container.Metadata.Add("docType", "textDocuments");
                container.Metadata["category"] = "guidance";

                // Set the container's metadata.
                await container.SetMetadataAsync();
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                    e.RequestInformation.HttpStatusCode,
                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
        
        public static async Task ReadContainerMetadataAsync(CloudBlobContainer container)
        {
            try
            {
                // Fetch container attributes in order to populate the container's properties and metadata.
                await container.FetchAttributesAsync();

                // Enumerate the container's metadata.
                Console.WriteLine("Container metadata:");
                foreach (var metadataItem in container.Metadata)
                {
                    Console.WriteLine("\tKey: {0}", metadataItem.Key);
                    Console.WriteLine("\tValue: {0}", metadataItem.Value);
                }
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                    e.RequestInformation.HttpStatusCode,
                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
        
        public static async Task SetBlobPropertiesAsync(CloudBlob blob)
        {
            try
            {
                Console.WriteLine("Setting blob properties.");

                // You must explicitly set the MIME ContentType every time
                // the properties are updated or the field will be cleared.
                blob.Properties.ContentType = "text/plain";
                blob.Properties.ContentLanguage = "en-us";

                // Set the blob's properties.
                await blob.SetPropertiesAsync();
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                    e.RequestInformation.HttpStatusCode,
                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
        
        private static async Task GetBlobPropertiesAsync(CloudBlob blob)
        {
            try
            {
                // Fetch the blob properties.
                await blob.FetchAttributesAsync();

                // Display some of the blob's property values.
                Console.WriteLine(" ContentLanguage: {0}", blob.Properties.ContentLanguage);
                Console.WriteLine(" ContentType: {0}", blob.Properties.ContentType);
                Console.WriteLine(" Created: {0}", blob.Properties.Created);
                Console.WriteLine(" LastModified: {0}", blob.Properties.LastModified);
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                    e.RequestInformation.HttpStatusCode,
                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
        
        public static async Task AddBlobMetadataAsync(CloudBlob blob)
        {
            try
            {
                // Add metadata to the blob by calling the Add method.
                blob.Metadata.Add("docType", "textDocuments");

                // Add metadata to the blob by using key/value syntax.
                blob.Metadata["category"] = "guidance";

                // Set the blob's metadata.
                await blob.SetMetadataAsync();
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                    e.RequestInformation.HttpStatusCode,
                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
        
        public static async Task ReadBlobMetadataAsync(CloudBlob blob)
        {
            try
            {
                // Fetch blob attributes in order to populate 
                // the blob's properties and metadata.
                await blob.FetchAttributesAsync();

                Console.WriteLine("Blob metadata:");

                // Enumerate the blob's metadata.
                foreach (var metadataItem in blob.Metadata)
                {
                    Console.WriteLine("\tKey: {0}", metadataItem.Key);
                    Console.WriteLine("\tValue: {0}", metadataItem.Value);
                }
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                    e.RequestInformation.HttpStatusCode,
                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}