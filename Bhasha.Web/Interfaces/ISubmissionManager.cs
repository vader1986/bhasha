using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces
{
	public interface ISubmissionManager
	{
		Task<Feedback> Accept(Submission submission);
	}
}

