using EddiDataDefinitions;
using EddiEvents;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace EddiNavigationService
{
    public interface IQueryResolver
    {
        QueryType Type { get; }
        Dictionary<string, object> SpanshQueryFilter { get; }
        RouteDetailsEvent Resolve ( [NotNull] Query query, [NotNull] StarSystem startSystem );
    }
}
