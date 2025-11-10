using Markivio.Application.Dto;

namespace Markivio.Presentation.GraphQl;

public class ArticleType : ObjectType<ArticleInformation>
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
          .Type<UserType>();
    }
}

