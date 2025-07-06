namespace Bhasha.Domain.Interfaces;

public interface IResourcesManager
{
    Task UploadImage(string resourceId, Stream image, CancellationToken token = default);
    Task UploadAudio(string resourceId, Stream audio, CancellationToken token = default);
}