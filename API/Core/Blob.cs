﻿using Azure.Identity;
using Azure.Storage.Blobs;
using JestersCreditUnion.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public sealed class Blob : IBlob
    {
        public async Task Upload(ISettings settings, string name, Stream stream)
        {
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(settings.IdentitificationCardContainerName),
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions()));
            BlobClient blobClient = containerClient.GetBlobClient(name);
            using (Stream blobStream = await blobClient.OpenWriteAsync(true))
            {
                await stream.CopyToAsync(blobStream);
            }
        }
    }
}
