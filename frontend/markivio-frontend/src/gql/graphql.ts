/* eslint-disable */
import type { TypedDocumentNode as DocumentNode } from '@graphql-typed-document-node/core';
export type Maybe<T> = T | null;
export type InputMaybe<T> = T | null | undefined;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  UUID: { input: any; output: any; }
};

/** Defines when a policy shall be executed. */
export enum ApplyPolicy {
  /** After the resolver was executed. */
  AfterResolver = 'AFTER_RESOLVER',
  /** Before the resolver was executed. */
  BeforeResolver = 'BEFORE_RESOLVER',
  /** The policy is applied in the validation step before the execution. */
  Validation = 'VALIDATION'
}

export type ArticleInformation = {
  __typename?: 'ArticleInformation';
  id?: Maybe<Scalars['UUID']['output']>;
  source?: Maybe<Scalars['String']['output']>;
  title?: Maybe<Scalars['String']['output']>;
  user?: Maybe<UserInformation>;
};

/** A segment of a collection. */
export type ArticlesCollectionSegment = {
  __typename?: 'ArticlesCollectionSegment';
  /** A flattened list of the items. */
  items?: Maybe<Array<Maybe<ArticleInformation>>>;
  /** Information to aid in pagination. */
  pageInfo: CollectionSegmentInfo;
  totalCount: Scalars['Int']['output'];
};

/** Information about the offset pagination. */
export type CollectionSegmentInfo = {
  __typename?: 'CollectionSegmentInfo';
  /** Indicates whether more items exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more items exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
};

export type CreateArticleInput = {
  source: Scalars['String']['input'];
  title: Scalars['String']['input'];
};

export type Mutation = {
  __typename?: 'Mutation';
  createArticle?: Maybe<ArticleInformation>;
  updateMyUser?: Maybe<UserInformation>;
};


export type MutationCreateArticleArgs = {
  createArticle: CreateArticleInput;
};


export type MutationUpdateMyUserArgs = {
  updateUserInformation: UpdateUserInformationInput;
};

export type Query = {
  __typename?: 'Query';
  articles?: Maybe<ArticlesCollectionSegment>;
  me?: Maybe<UserInformation>;
  userById?: Maybe<UserInformation>;
  users?: Maybe<UsersCollectionSegment>;
};


export type QueryArticlesArgs = {
  skip?: InputMaybe<Scalars['Int']['input']>;
  take?: InputMaybe<Scalars['Int']['input']>;
};


export type QueryUserByIdArgs = {
  id?: InputMaybe<Scalars['UUID']['input']>;
};


export type QueryUsersArgs = {
  skip?: InputMaybe<Scalars['Int']['input']>;
  take?: InputMaybe<Scalars['Int']['input']>;
};

export type UpdateUserInformationInput = {
  firstName: Scalars['String']['input'];
  lastName: Scalars['String']['input'];
};

export type UserInformation = {
  __typename?: 'UserInformation';
  email?: Maybe<Scalars['String']['output']>;
  firstName?: Maybe<Scalars['String']['output']>;
  id?: Maybe<Scalars['UUID']['output']>;
  lastName?: Maybe<Scalars['String']['output']>;
};

/** A segment of a collection. */
export type UsersCollectionSegment = {
  __typename?: 'UsersCollectionSegment';
  /** A flattened list of the items. */
  items?: Maybe<Array<UserInformation>>;
  /** Information to aid in pagination. */
  pageInfo: CollectionSegmentInfo;
  totalCount: Scalars['Int']['output'];
};

export type MeQueryVariables = Exact<{ [key: string]: never; }>;


export type MeQuery = { __typename?: 'Query', me?: { __typename?: 'UserInformation', id?: any | null, firstName?: string | null, lastName?: string | null, email?: string | null } | null };


export const MeDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"Me"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"me"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"firstName"}},{"kind":"Field","name":{"kind":"Name","value":"lastName"}},{"kind":"Field","name":{"kind":"Name","value":"email"}}]}}]}}]} as unknown as DocumentNode<MeQuery, MeQueryVariables>;