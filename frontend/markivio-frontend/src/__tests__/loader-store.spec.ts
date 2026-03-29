import { describe, expect, vi, it, beforeEach, afterEach } from 'vitest';
import { useLoaderStore } from '@/stores/loader-store';
import { createPinia, setActivePinia } from 'pinia';

const timings = [10, 20, 50, 100, 150, 200, 250, 290];

describe("Start loader", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
    vi.useFakeTimers();
  });
  afterEach(() => {
    vi.restoreAllMocks();
  });
  it("Start loader and should be false instantly", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    expect(loaderStore.isLoading).eq(false);
  });
  it.each(timings)("Start loader and should be false after %d ms", (time) => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    vi.advanceTimersByTime(time);
    expect(loaderStore.isLoading).eq(false);
  });
  it("Start loader and should be true after 300ms", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    vi.advanceTimersByTime(300);
    expect(loaderStore.isLoading).eq(true);
  });
  it("Start loader and should be true when timer is trigger", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(true);
  });
});

describe("Stop loader", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
    vi.useFakeTimers()
  });
  afterEach(() => {
    vi.restoreAllMocks();
  });
  it("Stop loader should be false", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    loaderStore.stop();
    expect(loaderStore.isLoading).eq(false);
  })

  it.each(timings)("Stop loader should be false when delay is %d", (time) => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    vi.advanceTimersByTime(time);
    loaderStore.stop();
    expect(loaderStore.isLoading).eq(false);
  });

  it("Stop loader should be true when execute some start and one stop", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    loaderStore.start();
    vi.runAllTimers();
    loaderStore.stop();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(true);
  });

  it("Stop loader should be false when stop is execute", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(true);
    loaderStore.stop();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(false);
  });

  it("Stop loader should be true when stop is execute", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    loaderStore.start();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(true);
    loaderStore.stop();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(true);
  });

  it("Stop loader should be false when multiple start and stop is execute", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    loaderStore.start();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(true);
    loaderStore.stop();
    vi.runAllTimers();
    loaderStore.stop();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(false);
  });

  it("Stop loader should be true when multiple start and stop is execute", () => {
    const loaderStore = useLoaderStore();
    loaderStore.start();
    loaderStore.start();
    vi.runAllTimers();
    expect(loaderStore.isLoading).eq(true);
    loaderStore.stop();
    vi.runAllTimers();
    loaderStore.stop();
    expect(loaderStore.isLoading).eq(true);
  });
})
