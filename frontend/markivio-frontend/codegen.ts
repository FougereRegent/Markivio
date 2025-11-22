import type { CodegenConfig } from '@graphql-codegen/cli';

const graphqlApi: string = "http://localhost:8080/graphql/schema.graphql";

const config: CodegenConfig = {
  schema: graphqlApi,
  documents: ['src/**/*.vue', 'src/**/*.ts', '!src/gql/**/*'],
  ignoreNoDocuments: true, // for better experience with the watcher
  generates: {
    './src/gql/': {
      preset: 'client',
      config: {
        useTypeImports: true,
      },
    },
  },
};

export default config;
