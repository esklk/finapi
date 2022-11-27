namespace Finance.Core.Practices
{
    public interface IFactory<out TObject, in TArgument>
    {
        TObject Create(TArgument argument);
    }
}
