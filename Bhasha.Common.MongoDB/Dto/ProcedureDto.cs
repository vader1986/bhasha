using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Exceptions;

namespace Bhasha.Common.MongoDB.Dto
{
    public class ProcedureDto
    {
        public string ProcedureId { get; set; } = "";
        public string Description { get; set; } = "";
        public string[]? Tutorial { get; set; }
        public string? AudioId { get; set; }
        public string[] Support { get; set; } = new string[0];

        public Procedure ToProcedure()
        {
            try
            {
                var id = new ProcedureId(ProcedureId);
                var tutorial = Tutorial != default ? Tutorial.Select(x => new ResourceId(x)).ToArray() : default;
                var support = Support.Select(Enum.Parse<TokenType>).ToArray();
                var audio = ResourceId.Create(AudioId);

                return new Procedure(id, Description, tutorial, audio, support);
            }
            catch (Exception e)
            {
                throw new InvalidProcedureException($"loaded invalid procedure {this.Stringify()}", e);
            }
        }
    }
}
