using Azure.Storage.Blobs;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Infrastructure.AzureBlob;

public sealed class AzureBlobResourcesManager(BlobServiceClient blobServiceClient) : IResourcesManager
{
    public async Task UploadImage(string resourceId, Stream image, CancellationToken token = default)
    {
        var container = blobServiceClient.GetBlobContainerClient("images");
        var client = container.GetBlobClient(resourceId);
        
        await client.UploadAsync(image, cancellationToken: token);
    }
}