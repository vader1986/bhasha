using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class SubmissionManager : ISubmissionManager
{
	private readonly IValidator _validator;
	private readonly IProgressManager _progressManager;
	private readonly IRepository<Profile> _profiles;

	public SubmissionManager(IValidator validator, IProgressManager progressManager, IRepository<Profile> profiles)
	{
		_validator = validator;
		_progressManager = progressManager;
		_profiles = profiles;
	}

    public async Task<Feedback> Accept(Submission submission)
    {
		var profile = await _profiles.Get(submission.ProfileId);

		if (profile == null)
			throw new ArgumentOutOfRangeException($"profile {submission.ProfileId} not found");

		var input = new ValidationInput(profile.Key.LangId, submission.ExpressionId, submission.Translation);
		var output = await _validator.Validate(input);
		var updatedProfile = await _progressManager.Update(profile, output);

		return new Feedback(updatedProfile, output);
	}
}

