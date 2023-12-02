using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Profile = Bhasha.Domain.Profile;

namespace Bhasha.Web.Shared.Components;

public partial class SelectProfile : ComponentBase
{
    [Inject] public required IStudyingService StudyingService { get; set; }
    [Parameter] public required Action<Profile> OnSelection { get; set; }
    [Parameter] public required string UserId { get; set; }
    [Parameter] public required IEnumerable<Profile> Profiles { get; set; }

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
        if (_selectedNative != null && _selectedTarget != null)
        {
            var alreadyExists = Profiles.Any(x => x.Key.Native == _selectedNative && x.Key.Target == _selectedTarget);
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

        var profileKey = new ProfileKey(UserId, _selectedNative, _selectedTarget);
        var profile = await StudyingService.CreateProfile(profileKey);

        _disableCreateButton = true;

        OnSelection(profile);
    }
}

