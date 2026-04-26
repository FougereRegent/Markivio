using FluentResults;
using Markivio.Application.Dto;
using Markivio.Application.Errors;
using Markivio.Application.Mapper;
using Markivio.Domain.Auth;
using Markivio.Domain.Entities;
using Markivio.Domain.Exceptions;
using Markivio.Domain.Repositories;

namespace Markivio.Application.UseCases;

public interface IArticleUseCase
{
    Task<Result<ArticleInformation>> GetById(Guid id, CancellationToken cancelationToken = default);
    Task<Result<ArticleInformation>> CreateArticle(CreateArticle createArticle, CancellationToken cancellationToken = default);
    Task<Result<ArticleInformation>> UpdateArticle(UpdateArticle updateArticle, CancellationToken cancellationToken = default);
    IQueryable<ArticleInformation> FindByFilter(ArticleFilters articleFilters);

    Task<Result<ArticleInformation>> AddTags(AddTagsToArticle addTags);
    Task<Result<ArticleInformation>> RemoveTags(RemoveTagsToArticle removeTags);
}

public class ArticleUseCase(ITagUseCase tagUseCase, IArticleRepository articleRepository, ITagRepository tagRepository, IAuthUser authUser) : IArticleUseCase
{
    public Task<Result<ArticleInformation>> GetById(Guid id, CancellationToken cancelationToken = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<ArticleInformation> FindByFilter(ArticleFilters articleFilters)
    {
        if (articleFilters is { Title: null, TagNames: null })
            return articleRepository.GetAll()
              .ProjectionToArticleInformation();

        return articleRepository.Filter(articleFilters.Title, articleFilters.TagNames)
          .ProjectionToArticleInformation();

    }

    public async Task<Result<ArticleInformation>> CreateArticle(CreateArticle createArticle, CancellationToken cancellationToken = default)
    {
        ArticleMapper mapper = new ArticleMapper();
        Article? article = await articleRepository.GetByTitle(createArticle.Title);
        if (article is not null)
            return Result.Fail(new AlreadyExistError("This article already exist"));

		var isFramableTask = CheckIfIsFramable(createArticle, cancellationToken);

        if (!CheckIfTagsExits(createArticle))
            return Result.Fail(new NotFoundError("A tag doesn't exist"));


        List<Tag> tags = tagRepository
            .GetByIds(createArticle.Tags.Select(pre => pre.Id).ToList())
            .ToList();

        try
        {
			var isFramable = await isFramableTask;
            article = mapper.Map(createArticle, tags, isFramable);
            article.User = authUser.CurrentUser;
        }
        catch (DomainException ex)
        {
            return Result.Fail(DomainError.Create(ex));
        }

        Article resultArticle = articleRepository.Save(article);
        return mapper.Map(resultArticle);
    }

    public async Task<Result<ArticleInformation>> AddTags(AddTagsToArticle addTags)
    {
        ArticleMapper mapper = new ArticleMapper();
        Article? article = await articleRepository.GetById(addTags.articleId);
        if (article is null)
            return Result.Fail(new NotFoundError("Article doesn't exist"));

        IReadOnlyList<Tag> tags = tagRepository.GetByIds(addTags.tagIds)
            .ToList();

        if (tags.Count() != addTags.tagIds.Length)
            return Result.Fail(new NotFoundError("Tags doesn't exist"));

        try
        {
            article.AddTags(tags);
        }
        catch (DomainException ex)
        {
            return Result.Fail(DomainError.Create(ex));
        }

        Article res = articleRepository.Update(article);
        return mapper.Map(res);
    }

    public async Task<Result<ArticleInformation>> RemoveTags(RemoveTagsToArticle removeTags)
    {
        ArticleMapper mapper = new ArticleMapper();
        Article? article = await articleRepository.GetById(removeTags.articleId);
        if (article is null)
            return Result.Fail(new NotFoundError("Article doesn't exist"));

        IReadOnlyList<Tag> tags = tagRepository.GetByIds(removeTags.tagIds)
            .ToList();

        if (tags.Count != removeTags.tagIds.Length)
            return Result.Fail(new NotFoundError("Tags doesn't exist"));

        try
        {
            article.RemoveTags(tags);
        }
		catch (DomainException ex) 
		{
            return Result.Fail(DomainError.Create(ex));
        }


        Article res = articleRepository.Update(article);
        return mapper.Map(res);
    }

    public async Task<Result<ArticleInformation>> UpdateArticle(UpdateArticle updateArticle,
            CancellationToken cancellationToken = default)
    {
        ArticleMapper mapper = new ArticleMapper();
        Article? article = await articleRepository.GetById(updateArticle.Id, cancellationToken);

        if (article is null)
            return Result.Fail(new NotFoundError("Artcile doesn't exist"));

        List<Tag> tags = tagRepository
            .GetByIds(updateArticle.Tags.Select(pre => pre.Id).ToList())
			.ToList();

		if(await CheckIfTitleAlreadyExist(updateArticle, article))
            return Result.Fail(new AlreadyExistError("This title article already exist"));

        try
        {
            article.Update(
                    title: updateArticle.Title,
                    description: updateArticle.Description,
                    tags: tags
                    );
        }
        catch (DomainException ex)
        {
            return Result.Fail(DomainError.Create(ex));
        }

        articleRepository.Update(article);
        return Result.Ok(mapper.Map(article));
    }

    private bool CheckIfTagsExits(CreateArticle createArticle)
    {
        TagMapper tagMapper = new TagMapper();
        if (createArticle.Tags is null || createArticle.Tags is { Length: 0 })
            return true;

        Guid[] tags = createArticle.Tags.Select(pre => pre.Id)
          .ToArray();

        return tagUseCase.TagsExist<Guid>(tags, TagExistConditionEnum.Id);
    }

	private async Task<bool> CheckIfIsFramable(CreateArticle article, CancellationToken cancellationToken = default) {
		return await articleRepository.IsFramable(article.Source, cancellationToken);
	}

	private async Task<bool> CheckIfTitleAlreadyExist(UpdateArticle updateArticle, Article source, CancellationToken cancellationToken = default) {
		if(updateArticle.Title.Equals(source.Title, StringComparison.InvariantCultureIgnoreCase)) {
			return false;
		}

		return await articleRepository.GetByTitle(updateArticle.Title, cancellationToken) != null;
	}
 
}
