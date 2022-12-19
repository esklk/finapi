using AutoMapper;

namespace Finance.Bot.Business.Mapping.Converters
{
    public class AssemblyQualifiedNameTypeConverter : ITypeConverter<string, Type>, ITypeConverter<Type, string>
    {
        public Type Convert(string source, Type destination, ResolutionContext context)
        {
            return Type.GetType(source) ?? throw new InvalidOperationException($"The \"{source}\" type is missing.");
        }

        public string Convert(Type source, string destination, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.AssemblyQualifiedName))
            {
                throw new ArgumentException($"The \"{nameof(source.AssemblyQualifiedName)}\" attribute is missing.", nameof(source));
            }

            return source.AssemblyQualifiedName;
        }
    }
}
