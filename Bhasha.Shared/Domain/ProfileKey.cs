namespace Bhasha.Shared.Domain;

public record ProfileKey(string UserId, string Native, string Target)
{
    public override string ToString()
    {
        return $"{UserId}:{Native}>{Target}";
    }
}

