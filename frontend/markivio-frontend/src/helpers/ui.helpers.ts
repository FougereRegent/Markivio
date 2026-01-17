const luminanceFactor = {
    r: 0.152,
    g: 0.383,
    b: 0.038,
};

export function CalculateLuminance(colorHex: string): number {
    const r = parseInt(colorHex.substring(1, 2), 16)
    const g = parseInt(colorHex.substring(1, 2), 16)
    const b = parseInt(colorHex.substring(1, 2), 16)

    return luminanceFactor['r'] * r + 
           luminanceFactor['g'] * g + 
           luminanceFactor['b'] * b;
}

export function ConstrasteColor(colorHex: string) : string {
    const luminance = CalculateLuminance(colorHex)
    return luminance < 128 ? "white" : "black";
}