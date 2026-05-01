namespace Markivio.Presentation.GraphQl.Tag;

public static class TagQuery
{
    extension(IObjectTypeDescriptor<Query> descriptor)
    {
        public IObjectTypeDescriptor<Query> MapTagQuery()
        {
            descriptor.MapGetTags();
            return descriptor;
        }
    }
}
