export type Constant = {
  debounceTime: {
    buttonTime: number;
    refetchTime: number;
  };
  flickerTime: number;
  toastTime: number;
};

export const CONST: Constant = {
  debounceTime: {
    buttonTime: 200,
    refetchTime: 200,
  },
  flickerTime: 300,
  toastTime: 3000,
};
