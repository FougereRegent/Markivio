

interface ValErr {
  properyName?: string | undefined,
  errorMessage: string,
};

type ValidationCallback = () => boolean;

export class ValidationError extends Error {
  readonly type: string = "validation-errors";

  validationErrors: Array<ValErr>;

  constructor(errs: Array<ValErr>) {
    super();
    this.validationErrors = errs;
  }
}

export class Validation {
  _errors = new Array<ValErr>();

  get HasError(): boolean {
    return this._errors.length > 0;
  }

  public IsValid(callback: ValidationCallback, err: ValErr): Validation {
    if (callback() ?? true) {
      return this;
    }
    this._errors.push(err);
    return this;
  }

  public Throw() {
    if (this.HasError)
      throw new ValidationError(this._errors);
  }
}

export function nameof<K>(property: keyof K): string {
  return property as string;
}
