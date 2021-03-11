namespace Bhasha.Common.MongoDB
{
    public class Names
    {
        public const string Database = "bhasha";

        public class Collections
        {
            public const string Users = nameof(Users);
            public const string Profiles = nameof(Profiles);
            public const string Chapters = nameof(Chapters);
            public const string Tokens = nameof(Tokens);
            public const string Tips = nameof(Tips);
            public const string Stats = nameof(Stats);
        }
    }
}
