using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common
{
    public interface IDatabase
    {
        ValueTask<User> CreateUser(User user);
        ValueTask<User> UpdateUser(User user);
        ValueTask<User> GetUser(int userId);
        ValueTask<User> DeleteUser(int userId);

        ValueTask<IEnumerable<Profile>> GetProfiles(int userId);
        ValueTask<Profile> CreateProfile(Profile profile);
        ValueTask<Profile> UpdateProfile(Profile profile);
        ValueTask<Profile> DeleteProfile(int profileId);
        ValueTask<int> DeleteProfiles(int userId);

        ValueTask<IEnumerable<Chapter>> GetChapters(int level);
        ValueTask<Chapter> CreateChapter(Chapter chapter);
        ValueTask<Chapter> UpdateChapter(Chapter chapter);
        ValueTask<Chapter> DeleteChapter(int chapterId);

        ValueTask<IEnumerable<Tip>> GetTips(int chapterId, int pageIndex);
        ValueTask<Tip> CreateTip(Tip tip);
        ValueTask<Tip> UpdateTip(Tip tip);
        ValueTask<Tip> DeleteTip(int tipId);
        ValueTask<int> DeleteTips(int chapterId, int pageIndex);

        ValueTask<ChapterStats> CreateChapterStats(ChapterStats chapterStats);
        ValueTask<ChapterStats> UpdateChapterStats(ChapterStats chapterStats);
        ValueTask<ChapterStats> GetChapterStats(int userId, int chapterId);
        ValueTask<int> DeleteChapterStats(int profileId);
    }
}
