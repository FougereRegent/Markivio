
export type Constante = {
    debounceTime: {
        buttonTime: number
        refetchTime: number
    },
    flickerTime:  number
    toastTime: number,
}

export const CONST: Constante = {
    debounceTime: {
        buttonTime: 200,
        refetchTime: 200,
    },
    flickerTime: 300,
    toastTime: 3000,
};
