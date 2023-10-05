import { catchError, debounceTime, distinctUntilChanged, ignoreElements, of, pipe, startWith } from 'rxjs';

export function delayedSearch() {
  return pipe(startWith<string | null>(''), debounceTime(250), distinctUntilChanged());
}

export function extractError() {
  return pipe(
    ignoreElements(),
    catchError((err: Error) => of(err))
  );
}
