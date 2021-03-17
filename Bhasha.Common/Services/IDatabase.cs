using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IDatabase
    {
        Task<Translation> QueryTranslationByTokenId(Guid tokenId, Language language);

        Task<ChapterStats> QueryStatsByChapterAndProfileId(Guid chapterId, Guid profileId);

        Task<IEnumerable<GenericChapter>> QueryChaptersByLevel(int level);

        Task<IEnumerable<Tip>> QueryTips(Guid chapterId, int pageIndex);

        Task<IEnumerable<Profile>> QueryProfilesByUserId(Guid userId);

        Task<IEnumerable<ChapterStats>> QueryStatsByProfileId(Guid profileId);
    }
}
