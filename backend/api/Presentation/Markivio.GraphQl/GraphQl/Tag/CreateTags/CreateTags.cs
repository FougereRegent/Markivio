using Markivio.Presentation.Middleware;

namespace Markivio.Presentation.GraphQl.Tag.CreateTags;

using Descriptor = IObjectTypeDescriptor<Mutation>;

public static class CreateTagMutation
{
    extension(Descriptor descriptor)
    {
        public Descriptor MapCreateTags()
        {
            descriptor
              .Field(f => f.CreateTags(default!, default!, default!))
              .UseTransactionMiddleware()
              .UsePaging()
              .Type<ListType<TagInformationType>>();
            return descriptor;
        }
    }
}
