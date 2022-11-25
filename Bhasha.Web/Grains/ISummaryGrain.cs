using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Orleans;

namespace Bhasha.Web.Grains;

public interface ISummaryGrain : IGrainWithStringKey
{
    ValueTask<ImmutableList<Summary>> GetSummaries();
}