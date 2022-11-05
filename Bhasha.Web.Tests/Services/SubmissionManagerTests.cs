using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services;

public class SubmissionManagerTests
{
	private readonly SubmissionManager _submissionManager;
	private readonly IValidator _validator;
	private readonly IProgressManager _progressManager;
	private readonly IRepository<Profile> _profiles;

	public SubmissionManagerTests()
	{
		_validator = Substitute.For<IValidator>();
		_progressManager = Substitute.For<IProgressManager>();
		_profiles = Substitute.For<IRepository<Profile>>();
		_submissionManager = new SubmissionManager(_validator, _progressManager, _profiles);
	}

	[Theory, AutoData]
	public async Task GivenSubmission_WhenAccepted_ThenReturnFeedback(Submission submission, Profile profile, ValidationResult result)
    {
		// setup
		_profiles.Get(submission.ProfileId).Returns(profile);
		_validator.Validate(default!).ReturnsForAnyArgs(result);
		_progressManager.Update(profile, result).Returns(profile);

		// act
		var feedback = await _submissionManager.Accept(submission);

		// verify
		Assert.Equal(profile, feedback.Profile);
		Assert.Equal(result, feedback.Result);
	}
}

