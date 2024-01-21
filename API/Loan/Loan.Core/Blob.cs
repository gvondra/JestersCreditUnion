using Azure.Storage.Blobs;
using JestersCreditUnion.Loan.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public sealed class Blob : IBlob
    {
        public async Task Upload(ISettings settings, string name, Stream stream)
        {
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(settings.IdentitificationCardContainerName),
                AzureCredential.DefaultAzureCredential);
            BlobClient blobClient = containerClient.GetBlobClient(name);
            using (Stream blobStream = await blobClient.OpenWriteAsync(true))
            {
                await stream.CopyToAsync(blobStream);
            }
        }

        public async Task<Stream> Download(ISettings settings, string name)
        {
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(settings.IdentitificationCardContainerName),
                AzureCredential.DefaultAzureCredential);
            BlobClient blobClient = containerClient.GetBlobClient(name);
            return await blobClient.OpenReadAsync();
        }
    }
}
