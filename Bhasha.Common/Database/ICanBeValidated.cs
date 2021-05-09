namespace Bhasha.Common.Database
{
    /// <summary>
    /// Defines an interface for objects which can be validated.
    /// </summary>
    public interface ICanBeValidated
    {
        /// <summary>
        /// Validates this object.
        /// </summary>
        /// <exception cref="Bhasha.Common.Exceptions.InvalidObjectException"/>
        void Validate();
    }
}
