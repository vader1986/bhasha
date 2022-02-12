using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
    public class ChapterProvider : IChapterProvider
    {
        private IRepository<Chapter> _chapterRepository;
        private IRepository<Profile> _profileRepository;
        private readonly ITranslationProvider _translationProvider;

        public ChapterProvider(
            IRepository<Chapter> chapterRepository,
            IRepository<Profile> profileRepository,
            ITranslationProvider translationProvider)
        {
            _chapterRepository = chapterRepository;
            _profileRepository = profileRepository;
            _translationProvider = translationProvider;
        }

        public async Task<ChapterDescription[]> GetChapters(Guid profileId)
        {
            var profile = await _profileRepository.Get(profileId);
            if (profile == null)
                throw new InvalidOperationException(
                    $"Could not find profile for {profileId}");

            var progress = profile.Progress;
            var chapters = await _chapterRepository.Find(x => x.RequiredLevel == progress.Level);

            var expressionIds = chapters.Select(x => x.NameId).Concat(chapters.Select(x => x.DescriptionId));
            var expressions = await _translationProvider.FindAll(profile.Native, expressionIds.ToArray());

            return chapters.Select(x =>
                new ChapterDescription(
                    x.Id,
                    expressions[x.NameId].Native,
                    expressions[x.DescriptionId].Native,
                    progress.CompletedChapters.Contains(x.Id))).ToArray();
        }
    }
}

