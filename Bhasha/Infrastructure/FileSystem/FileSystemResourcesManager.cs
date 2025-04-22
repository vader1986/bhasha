using Bhasha.Domain.Interfaces;

namespace Bhasha.Infrastructure.FileSystem;

public sealed class FileSystemResourcesManager : IResourcesManager
{
    public async Task UploadImage(string resourceId, Stream image, CancellationToken token = default)
    {
        await using var fileStream = File.Create(resourceId);
        await image.CopyToAsync(fileStream, token);
    }
}