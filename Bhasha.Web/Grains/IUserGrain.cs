using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Orleans;

namespace Bhasha.Web.Grains;

public interface IUserGrain : IGrainWithStringKey
{
    /// <summary>
    /// Gets the profile for the specified language combination for the current
    /// user. 
    /// </summary>
    /// <param name="langId">Language combination of the profile</param>
    /// <returns>User profile for the specified language combination.</returns>
    /// <exception cref="InvalidOperationException">profile does not exist</exception>
    ValueTask<Profile> GetProfile(LangKey langId);

    /// <summary>
    /// Gets the current chapter of the profile. If no chapter is
    /// selected <c>null</c> is returned. 
    /// </summary>
    /// <param name="langId">Language combination of the profile</param>
    /// <returns>Current chapter or <c>null</c> if no chapter is selected.</returns>
    /// <exception cref="InvalidOperationException">profile does not exist</exception>
    Task<DisplayedChapter?> GetCurrentChapter(LangKey langId);

    /// <summary>
    /// Gets a list of chapter summaries for uncompleted chapters for the
    /// selected user profile. 
    /// </summary>
    /// <param name="langId">Language combination of the profile</param>
    /// <returns>A list of uncompleted chapter summaries.</returns>
    Task<ImmutableList<Summary>> GetSummaries(LangKey langId);

}
