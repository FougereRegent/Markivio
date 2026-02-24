export enum ErrType {
  auth,
  business,
  forbiden,
  notfound,
  unknow,
};

export interface Err {
  type: ErrType,
  message: string,
};

export function mapGraphqlError(errorCode: string): Err {
  switch (errorCode) {
    case "AlreadyExistError":
      return {
        type: ErrType.business,
        message: "This item already exist"
      };
    case "DuplicatedItemsError":
      return {
        type: ErrType.business,
        message: "Duplicated items in your collections"
      };
    case "NotFoundError":
      return {
        type: ErrType.business,
        message: "Item not found"
      };
    case "UnauthorizedError":
      return {
        type: ErrType.auth,
        message: "Not connected",
      };

    default:
      return {
        type: ErrType.unknow,
        message: ""
      }
  }
};
