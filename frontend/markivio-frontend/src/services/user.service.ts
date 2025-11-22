import { from, map, Observable } from "rxjs"
import { gql, useQuery } from "@urql/vue";
import { graphql } from "../gql";


const query = graphql(`query me { me { id } }`);
//const query = gql`query me {
//    me {
//        id
//        firstName
//        lastName
//        email
//    }
//}`;

export default {
  getMe() {
    const { data, error } = useQuery({
      query: query,
      variables: {},
    })

    console.log(data);
    console.log(error);
    return data
  }
}
