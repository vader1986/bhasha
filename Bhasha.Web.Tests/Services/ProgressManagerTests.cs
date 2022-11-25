using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services;

public class ProgressManagerTests
{
	private readonly ProgressManager _progressManager;
	private readonly IRepository<Chapter> _chapters;
	private readonly IRepository<Profile> _profiles;

	public ProgressManagerTests()
	{
		_chapters = Substitute.For<IRepository<Chapter>>();
		_profiles = Substitute.For<IRepository<Profile>>();
		_progressManager = new ProgressManager(_chapters, _profiles);
	}

	[Theory, AutoData]
	public void GivenMissingProfile_WhenStartChapter_ThenThrowException(Guid profileId, Guid chapterId, Chapter chapter)
    {
		// setup
		_profiles.Get(profileId).Returns(default(Profile));
		_chapters.Get(chapterId).Returns(chapter);

		// act & verify
		Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await _progressManager.StartChapter(profileId, chapterId));
    }


	[Theory, AutoData]
	public void GivenMissingChapter_WhenStartChapter_ThenThrowException(Guid profileId, Guid chapterId, Profile profile)
	{
		// setup
		_profiles.Get(profileId).Returns(profile);
		_chapters.Get(chapterId).Returns(default(Chapter));

		// act & verify
		Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await _progressManager.StartChapter(profileId, chapterId));
	}

	[Theory, AutoData]
	public void GivenFailingUpdate_WhenStartChapter_ThenThrowException(Guid profileId, Guid chapterId, Profile profile, Chapter chapter)
	{
		// setup
		_profiles.Get(profileId).Returns(profile);
		_chapters.Get(chapterId).Returns(chapter);
		_profiles.Update(profileId, profile).ReturnsForAnyArgs(false);

		// act & verify
		Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await _progressManager.StartChapter(profileId, chapterId));
	}

	[Theory, AutoData]
	public async Task GivenSuccessfulUpdate_WhenStartChapter_ThenReturnUpdatedProfile(Guid profileId, Guid chapterId, Profile profile, Chapter chapter)
	{
		// setup
		_profiles.Get(profileId).Returns(profile);
		_chapters.Get(chapterId).Returns(chapter);
		_profiles.Update(profileId, profile).ReturnsForAnyArgs(true);

		// act
		var updatedProfile = await _progressManager.StartChapter(profileId, chapterId);

		// verify
		Assert.Equal(chapterId, updatedProfile.ChapterId);
		Assert.Equal(0, updatedProfile.PageIndex);
		Assert.Equal(chapter.Pages.Length, updatedProfile.Pages.Length);
		Assert.True(updatedProfile.Pages.All(x => x == ValidationResultType.Wrong));
	}

	[Theory, AutoData]
	public void GivenMissingChapter_WhenProfileUpdated_ThenThrowException(Profile profile, ValidationResult result)
	{
		// setup
		profile = profile with { ChapterId = Guid.NewGuid() };
		_chapters.Get(profile.ChapterId.Value).Returns(default(Chapter));

		// act & verify
		Assert.ThrowsAsync<ArgumentOutOfRangeException>(
			async () => await _progressManager.Update(profile, result));
	}

	[Theory, AutoData]
	public void GivenProfile_WhenUpdateFails_ThenThrowException(
		Profile profile, ValidationResult result, Chapter chapter)
	{
        // setup
        profile = profile with { ChapterId = Guid.NewGuid() };
        _chapters.Get(profile.ChapterId.Value).Returns(chapter);
		_profiles.Update(default, default!).ReturnsForAnyArgs(false);

		// act & verify
		Assert.ThrowsAsync<InvalidOperationException>(
			async () => await _progressManager.Update(profile, result));
	}

	[Theory, AutoData]
	public async Task GivenCorrectResultForProfile_WhenUpdated_ThenUpdateProfile(
		Profile profile, ValidationResult result, Chapter chapter)
    {
		// setup
		result = result with { Result = ValidationResultType.Correct };
		profile = profile with
		{
			ChapterId = chapter.Id,
			CompletedChapters = Array.Empty<Guid>(),
			Level = 1,
			PageIndex = 0,
			Pages = chapter.Pages.Select(_ => ValidationResultType.Wrong).ToArray()
		};

		_chapters.Get(profile.ChapterId.Value).Returns(chapter);
		_profiles.Update(default, default!).ReturnsForAnyArgs(true);

		// act
		await _progressManager.Update(profile, result);

		// verify
		await _profiles
			.Received(1)
			.Update(profile.Id, Arg.Is<Profile>(
				x => x.PageIndex == 1 &&
						x.Pages[0] == ValidationResultType.Correct));
	}

	[Theory, AutoData]
	public async Task GivenFinishedChapterForProfile_WhenUpdated_ThenUpdateProfile(
		Profile profile, ValidationResult result, Chapter chapter)
	{
		// setup
		result = result with { Result = ValidationResultType.Correct };
		profile = profile with
		{
			ChapterId = chapter.Id,
			CompletedChapters = Array.Empty<Guid>(),
			Level = 1,
			PageIndex = chapter.Pages.Length - 1,
			Pages = chapter.Pages.Select(_ => ValidationResultType.Correct).ToArray()
		};

		_chapters.Get(profile.ChapterId.Value).Returns(chapter);
		_profiles.Update(default, default!).ReturnsForAnyArgs(true);

		// act
		await _progressManager.Update(profile, result);

		// verify
		await _profiles
			.Received(1)
			.Update(profile.Id, Arg.Is<Profile>(
				x => x.ChapterId == null &&
						x.CompletedChapters.Contains(chapter.Id)));
	}

	[Theory, AutoData]
	public async Task GivenWrongResultForProfile_WhenUpdated_ThenUpdateProfile(
		Profile profile, ValidationResult result, Chapter chapter)
	{
		// setup
		result = result with { Result = ValidationResultType.Wrong };
		profile = profile with
		{
			ChapterId = chapter.Id,
			CompletedChapters = Array.Empty<Guid>(),
			Level = 1,
			PageIndex = 0,
			Pages = chapter.Pages.Select(_ => ValidationResultType.PartiallyCorrect).ToArray()
		};

		_chapters.Get(profile.ChapterId.Value).Returns(chapter);
		_profiles.Update(default, default!).ReturnsForAnyArgs(true);

		// act
		await _progressManager.Update(profile, result);

		// verify
		await _profiles
			.Received(1)
			.Update(profile.Id, Arg.Is<Profile>(
				x => x.PageIndex == 1 &&
						x.Pages[0] == ValidationResultType.Wrong));
	}
}

