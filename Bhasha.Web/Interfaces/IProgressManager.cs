using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IProgressManager
{
	Task<Profile> Update(Profile profile, ValidationResult result);
	Task<Profile> StartChapter(Guid profileId, Guid chapterId);
}