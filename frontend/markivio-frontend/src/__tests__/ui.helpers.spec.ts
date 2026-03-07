import { describe, expect, it } from 'vitest';
import { calculateLuminance, contrastColor } from '@/helpers/ui.helpers';

describe('calculateLuminance', () => {
  it('Should return 0 for black (#000000)', () => {
    expect(calculateLuminance('#000000')).eq(0);
  });

  it('Should return max luminance for white (#FFFFFF)', () => {
    const result = calculateLuminance('#FFFFFF');
    const expected = 0.152 * 255 + 0.383 * 255 + 0.038 * 255;
    expect(result).toBeCloseTo(expected, 2);
  });

  it('Should return correct luminance for pure red (#FF0000)', () => {
    const result = calculateLuminance('#FF0000');
    expect(result).toBeCloseTo(0.152 * 255, 2);
  });

  it('Should return correct luminance for pure green (#00FF00)', () => {
    const result = calculateLuminance('#00FF00');
    expect(result).toBeCloseTo(0.383 * 255, 2);
  });

  it('Should return correct luminance for pure blue (#0000FF)', () => {
    const result = calculateLuminance('#0000FF');
    expect(result).toBeCloseTo(0.038 * 255, 2);
  });
});

describe('contrastColor', () => {
  it('Should return "white" for black background', () => {
    expect(contrastColor('#000000')).eq('white');
  });

  it('Should return "black" for white background', () => {
    expect(contrastColor('#FFFFFF')).eq('black');
  });

  it('Should return "white" for dark blue background', () => {
    expect(contrastColor('#00008B')).eq('white');
  });

  it('Should return "black" for yellow background', () => {
    expect(contrastColor('#FFFF00')).eq('black');
  });

  it('Should return "white" for dark red background', () => {
    expect(contrastColor('#8B0000')).eq('white');
  });
});
