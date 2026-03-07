using Markivio.Application.Dto;


namespace Markivio.Presentation.GraphQl;

public class TagInformationType : ObjectType<TagInformation>
{
    protected override void Configure(IObjectTypeDescriptor<TagInformation> descriptor)
    {
        descriptor
          .Field(f => f.Id)
          .Type<UuidType>();

        descriptor
          .Field(f => f.Name)
          .Type<StringType>();

        descriptor
          .Field(f => f.Color)
          .Type<StringType>();
    }
}

public class TagSoftInformationType : ObjectType<TagSoftInformation>
{
    protected override void Configure(IObjectTypeDescriptor<TagSoftInformation> descriptor)
    {
        descriptor
          .Field(f => f.Name)
          .Type<StringType>();

        descriptor
          .Field(f => f.Color)
          .Type<StringType>();
    }
}
