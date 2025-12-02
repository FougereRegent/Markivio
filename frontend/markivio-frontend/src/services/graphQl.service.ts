import { GetConfig } from "@/config/urql.config";
import { Client, type Exchange }from "@urql/core";

const configUrql = GetConfig();

export const client = new Client({
  exchanges: configUrql.Exchanges as Exchange[],
  url: configUrql.Url
});

