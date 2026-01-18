const luminanceFactor = {
    r: 0.152,
    g: 0.383,
    b: 0.038,
};

export function CalculateLuminance(colorHex: string): number {
    const r = parseInt(colorHex.substring(1, 3), 16)
    const g = parseInt(colorHex.substring(3, 5), 16)
    const b = parseInt(colorHex.substring(5, 7), 16)

    return luminanceFactor['r'] * r + 
           luminanceFactor['g'] * g + 
           luminanceFactor['b'] * b;
}

export function ConstrasteColor(colorHex: string) : string {
    const thresholdLuminance = 0.5;
    const luminance = CalculateLuminance(colorHex) / 255.0;
    return luminance < thresholdLuminance ? "white" : "black";
}