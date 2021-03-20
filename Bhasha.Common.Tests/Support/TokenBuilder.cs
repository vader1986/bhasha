using System;
using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class TokenBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _label = Rnd.Create.NextString();
        private int _level = Rnd.Create.Next();
        private CEFR _cefr = Rnd.Create.Choose(Enum.GetValues<CEFR>());
        private TokenType _tokenType = Rnd.Create.Choose(Enum.GetValues<TokenType>());
        private string[] _categories = Rnd.Create.NextStrings().ToArray();
        private ResourceId _pictureId = Rnd.Create.NextString();

        public TokenBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public static TokenBuilder Default => new();

        public Token Build()
        {
            return new Token(
                _id,
                _label,
                _level,
                _cefr,
                _tokenType,
                _categories,
                _pictureId);
        }
    }
}
