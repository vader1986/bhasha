using Bhasha.Web.Domain;

namespace Bhasha.Web.Services
{
	public static class TranslationExtensions
	{
		public static Translation Merge(this Translation translation, Translation other)
		{
			return translation with {
				AudioId = other.AudioId ?? translation.AudioId,
				Spoken = other.Spoken ?? translation.Spoken
			};
		}
	}
}

