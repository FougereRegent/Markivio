import { from, map, Observable } from "rxjs"
import { client } from "./GraphQlService"
import { gql } from "@urql/vue";

export default {
  getMe() {
    const query = gql`
{
  me {
    id
    firstName
    lastName
    email
  }
}`;
    const variables: any[] = []

    return from(client.query(query, variables))
      .pipe(
        map(pre => pre.data)
      )
  }
}
