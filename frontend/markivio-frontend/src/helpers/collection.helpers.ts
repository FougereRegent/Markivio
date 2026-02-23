import { keyof } from "zod";

class Group<V, T> {
  key: V;
  members: T[] = [];

  constructor(key: V) {
    this.key = key;
  }
}

export function groupBy<T, V extends keyof T>(list: T[], keyName:V): Record<PropertyKey, T[]> {
  return list.reduce((prev, current) => {
    const key = current[keyName] as PropertyKey;
    (prev[key] ||= []).push(current);
    return prev;
  }, {} as Record<PropertyKey, T[]>)
}
