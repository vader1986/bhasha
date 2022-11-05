using Orleans;
using Orleans.Streams;

namespace Bhasha.Web.Grains;

public interface IFakeGrain : IGrainWithStringKey
{
    Task<int> CreateShit();
    Task CreateRandomShit();
}

public class FakeGrain : Grain, IFakeGrain
{
    private IAsyncStream<int>? _stream;
    private int _index;

    public FakeGrain()
    {
    }

    public override async Task OnActivateAsync()
    {
        var streamProvider = GetStreamProvider("SMSProvider");
        _stream = streamProvider.GetStream<int>(Guid.Empty, "GRAINKEY");
        await base.OnActivateAsync();
    }


    public Task<int> CreateShit()
    {
        return Task.FromResult(_index);
    }

    public async Task CreateRandomShit()
    {
        if (_index == 0)
        {
            _index = int.Parse(this.GetPrimaryKeyString());
        }

        _index++;

        if (_stream != null)
        {
            await _stream.OnNextAsync(_index);
        }
        else
        {
            throw new InvalidOperationException("stream does not exist");
        }
    }
}

