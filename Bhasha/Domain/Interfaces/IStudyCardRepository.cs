namespace Bhasha.Domain.Interfaces;

public interface IStudyCardRepository
{
    Task<StudyCard> GetById(int id, CancellationToken token = default);
    Task Delete(int id, CancellationToken token = default);
    Task<StudyCard> AddOrUpdate(StudyCard studyCard, CancellationToken token = default);
    Task<IEnumerable<StudyCard>> FindByLanguage(string language, string studyLanguage, CancellationToken token = default);
}