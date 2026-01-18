
export interface ArticleInformation {
    Id: string,
    Title: string,
    Source: string,
    Tags: Array<{
        Name: string,
        Color: string,
    }>,
}