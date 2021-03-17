namespace Bhasha.Common.MongoDB.Dto
{
    public interface IConvert<TDto, TProduct>
    {
        TProduct Convert(TDto dto);
        TDto Convert(TProduct product);
    }
}
