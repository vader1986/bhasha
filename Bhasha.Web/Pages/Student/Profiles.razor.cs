using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Pages.Student
{
	public partial class Profiles : UserPage
    {
		[Inject] public IProfileManager ProfileManager { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private List<Profile> _profiles = new List<Profile>();
        private bool _disableCreateButton = true;
        private Language? _selectedNative;
        private Language? _selectedTarget;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _profiles.AddRange(await ProfileManager.GetProfiles(UserId!));
        }

        internal void OnSelectProfile(Profile profile)
        {
            NavigationManager.NavigateTo($"chapters/{profile.Id}");
        }

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
            if (_selectedNative != null && _selectedTarget != null && _profiles != null)
            {
                var alreadyExists = _profiles.Any(x => x.Native == _selectedNative && x.Target == _selectedTarget);
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
            _profiles.Add(await ProfileManager.Create(UserId!, _selectedNative!, _selectedTarget!));

            ValidateParameters();
        }
    }
}

