import { groupBy } from '@/helpers/collection.helpers';
import { ref, toValue, watch } from 'vue';
import * as z from 'zod';

export function useZodValidation<
  T extends z.ZodTypeAny,
  U = Record<string, unknown>,
  V = Record<string, z.ZodError[]>,
>(schema: T, data: U) {
  const isValid = ref(true);
  const errors = ref<V | null>(null);

  const clearErrors = () => {
    errors.value = null;
  };

  let unwatch: null | (() => void) = null;

  const validateWatch = () => {
    if (unwatch != null) return;

    unwatch = watch(
      () => data,
      () => {
        validate();
      },
      { deep: true },
    );
  };

  const stopWatch = () => {
    unwatch?.();
    unwatch = null;
  };

  const validate = () => {
    clearErrors();

    const result = toValue(schema).safeParse(toValue(data));
    isValid.value = result.success;

    if (!result.success) {
      errors.value = groupBy(result.error.issues, 'path');
      return false;
    }
    return true;
  };

  return { validate, isValid, errors, validateWatch, stopWatch };
}
