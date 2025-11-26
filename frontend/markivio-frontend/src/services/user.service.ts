import { from, map, Observable } from "rxjs"
import { gql, useQuery } from "@urql/vue";
import { graphql } from "../gql";
import type { MeQuery } from "@/gql/graphql";
import { Client } from "@urql/vue";
import { inject } from "vue";


const query = graphql(`
query Me {
  me {
    id
    firstName
    lastName
    email
  }
}`);

export default {
  getMe() {
    const { data, error, fetching } = useQuery<MeQuery>({
      query: query,
      variables: {},
    })

    return { data, error, fetching };
  },
}
