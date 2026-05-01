using Markivio.Application.Dto;
using Markivio.Presentation.GraphQl.User;
using Markivio.Presentation.GraphQl.Tag;

namespace Markivio.Presentation.GraphQl.Article;

public class ArticleInformationType : ObjectType<ArticleInformation>
{
    protected override void Configure(IObjectTypeDescriptor<ArticleInformation> descriptor)
    {
        descriptor
          .Field(f => f.Id)
          .Type<UuidType>();

        descriptor
          .Field(f => f.Source)
          .Type<StringType>();

        descriptor
          .Field(f => f.Title)
          .Type<StringType>();

        descriptor
          .Field(f => f.User)
          .Type<UserInformationType>();

        descriptor
          .Field(f => f.Tags)
          .Type<ListType<TagInformationType>>();

        descriptor
            .Field(f => f.IsFramable)
            .Type<BooleanType>();
    }
}

