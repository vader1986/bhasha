using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces
{
	public interface ITranslationManager
	{
		Task<Translation> AddOrUpdate(Translation translation, Translation? reference);
	}
}

