using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services
{
	public class ProgressManagerTests
	{
		/*
		 * TODO
		 * - cover Update method with tests
		 */

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
			Assert.Equal(chapterId, updatedProfile.Progress.ChapterId);
			Assert.Equal(0, updatedProfile.Progress.PageIndex);
			Assert.Equal(chapter.Pages.Length, updatedProfile.Progress.Pages.Length);
			Assert.True(updatedProfile.Progress.Pages.All(x => x == ValidationResultType.Wrong));
		}


	}
}

