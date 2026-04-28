namespace Markivio.Presentation.GraphQl.Tag;

public static class TagQuery
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapTag()
        {
            descriptor.MapGetTags();
            return descriptor;
        }
    }
}
