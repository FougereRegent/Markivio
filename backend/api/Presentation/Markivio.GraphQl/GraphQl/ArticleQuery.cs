using Markivio.Application.Dto;

namespace Markivio.Presentation.GraphQl;

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
          .Type<ListType<TagSoftInformationType>>();
    }
}

