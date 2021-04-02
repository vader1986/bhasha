namespace Bhasha.Common.Arguments
{
    public class OneOutOfFourArguments
    {
        public Option[] Options { get; }

        public OneOutOfFourArguments(Option[] options)
        {
            Options = options;
        }

        public class Option
        {
            public string Value { get; }
            public string DisplayName { get; }

            public Option(string value, string displayName)
            {
                Value = value;
                DisplayName = displayName;
            }
        }
    }
}
