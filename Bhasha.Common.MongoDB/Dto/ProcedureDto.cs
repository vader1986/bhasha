using System;
using System.Linq;

namespace Bhasha.Common.MongoDB.Dto
{
    public class ProcedureDto
    {
        public string ProcedureId { get; set; } = "";
        public string Description { get; set; } = "";
        public string[]? Tutorial { get; set; }
        public string? Audio { get; set; }
        public string[] Support { get; set; } = new string[0];

        public Procedure ToProcedure()
        {
            var id = new ProcedureId(ProcedureId);
            var tutorial = Tutorial.Select(x => new ResourceId(x)).ToArray();
            var support = Support.Select(Enum.Parse<TokenType>).ToArray();
            var audio = ResourceId.Create(Audio);

            return new Procedure(id, Description, tutorial, audio, support);
        }
    }
}
