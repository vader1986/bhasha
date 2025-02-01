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
    private string? _error;

    private void OnSelectedNative(Language selectedNative)
    {
        _selectedNative = selectedNative;
        ValidateParameters();
    }

    private void OnSelectedTarget(Language selectedTarget)
    {
        _selectedTarget = selectedTarget;
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

    private async Task OnCreate()
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


    private EventCallback<MudChip<Profile>> OnDeleteProfile(Profile profile)
    {
        return new EventCallback<MudChip<Profile>>(this, async () => await OnDeleteAsync(profile));
    }
    
    private async Task OnDeleteAsync(Profile profile)
    {
        try
        {
            await StudyingService.DeleteProfile(profile.Key);
            Profiles = Profiles.Where(x => x.Id != profile.Id);
        }
        catch (Exception e)
        {
            _error = $"{e.Message}: {e.StackTrace}";
        }
        await InvokeAsync(StateHasChanged);
    }
}

