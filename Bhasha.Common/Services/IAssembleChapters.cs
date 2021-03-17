using System;
using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IAssembleChapters
    {
        Task<Chapter> Assemble(Guid chapterId, Profile profile);
    }
}
