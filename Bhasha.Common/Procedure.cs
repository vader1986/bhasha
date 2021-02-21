namespace Bhasha.Common
{
    public class Procedure
    {
        public ProcedureId Id { get; }
        public string Description { get; }
        public ResourceId[] Tutorial { get; }
        public ResourceId? Audio { get; }
        public TokenType[] Support { get; }

        public Procedure(ProcedureId id, string description, ResourceId[]? tutorial, ResourceId? audio, TokenType[] support)
        {
            Id = id;
            Description = description;
            Tutorial = tutorial ?? new ResourceId[0];
            Audio = audio;
            Support = support;
        }
    }
}
