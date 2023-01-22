using Bhasha.Domain;
using Bhasha.Grains;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orleans;

namespace Bhasha.Web.Shared.Components;

public partial class SelectProfile : ComponentBase
{
    [Inject] public IClusterClient ClusterClient { get; set; } = default!;

    [Parameter] public Action<Profile> OnSelection { get; set; }
    [Parameter] public string UserId { get; set; }
    [Parameter] public IEnumerable<Profile> Profiles { get; set; }

    private bool _disableCreateButton = true;
    private Language? _selectedNative;
    private Language? _selectedTarget;

    internal void OnSelectedNative(MudChip chip)
    {
        _selectedNative = chip.Value as Language;
        ValidateParameters();
    }

    internal void OnSelectedTarget(MudChip chip)
    {
        _selectedTarget = chip.Value as Language;
        ValidateParameters();
    }

    private void ValidateParameters()
    {
        if (_selectedNative != null && _selectedTarget != null && Profiles != null)
        {
            var alreadyExists = Profiles.Any(x => x.Key.LangId.Native == _selectedNative && x.Key.LangId.Target == _selectedTarget);
            var invalidSelection = _selectedNative == _selectedTarget;

            _disableCreateButton = alreadyExists || invalidSelection;
        }
        else
        {
            _disableCreateButton = true;
        }
    }

    internal async Task OnCreate()
    {
        if (_selectedNative == null)
            throw new InvalidOperationException("No native language selected");

        if (_selectedTarget == null)
            throw new InvalidOperationException("No target language selected");

        var grain = ClusterClient.GetGrain<IStudentGrain>(UserId);
        var profile = await grain.CreateProfile(new LangKey(_selectedNative, _selectedTarget));

        _disableCreateButton = true;

        OnSelection(profile);
    }
}

