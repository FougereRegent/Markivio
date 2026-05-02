using Markivio.Presentation.GraphQl.Tag.CreateTags;

namespace Markivio.Presentation.GraphQl.Tag;

using Descriptor = IObjectTypeDescriptor<Mutation>;

public static class TagMutation
{
    extension(Descriptor descriptor)
    {
        public Descriptor MapTagMutation()
        {
            descriptor.MapCreateTags();
            return descriptor;
        }
    }
}
