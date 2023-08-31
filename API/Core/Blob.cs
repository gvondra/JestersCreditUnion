using Azure.Identity;
using Azure.Storage.Blobs;
using JestersCreditUnion.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public sealed class Blob : IBlob
    {
        public async Task Upload(ISettings settings, string name)
        {
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(settings.IdentitificationCardContainerName),
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions()));
            BlobClient blobClient = containerClient.GetBlobClient(name);
            using (Stream stream = await blobClient.OpenWriteAsync(true))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine("test");
                }
            }
        }
    }
}
