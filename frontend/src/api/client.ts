/** RFC 9457 problem details returned by the backend for every error response. */
export interface ProblemDetails {
  type?: string
  title?: string
  status?: number
  detail?: string
  errors?: Record<string, string[]>
}

export class ApiError extends Error {
  readonly status: number
  readonly problem: ProblemDetails | undefined

  constructor(status: number, problem?: ProblemDetails) {
    super(problem?.detail ?? problem?.title ?? `Request failed with status ${status}`)
    this.name = 'ApiError'
    this.status = status
    this.problem = problem
  }

  static async fromResponse(response: Response): Promise<ApiError> {
    const contentType = response.headers.get('Content-Type') ?? ''
    const problem = contentType.includes('json')
      ? ((await response.json().catch(() => undefined)) as ProblemDetails | undefined)
      : undefined
    return new ApiError(response.status, problem)
  }
}

interface RequestOptions extends Omit<RequestInit, 'body' | 'headers'> {
  body?: unknown
  headers?: Record<string, string>
}

/**
 * Thin wrapper around fetch for calls to the backend REST API.
 * Authentication is handled by the HttpOnly session cookie, which the browser
 * attaches automatically to same-origin requests.
 */
export async function apiFetch<T>(path: string, { body, headers, ...init }: RequestOptions = {}): Promise<T> {
  const response = await fetch(path, {
    credentials: 'same-origin',
    ...init,
    headers: {
      Accept: 'application/json',
      ...(body !== undefined && { 'Content-Type': 'application/json' }),
      ...headers,
    },
    body: body !== undefined ? JSON.stringify(body) : undefined,
  })

  if (!response.ok) {
    throw await ApiError.fromResponse(response)
  }

  if (response.status === 204 || response.headers.get('Content-Length') === '0') {
    return undefined as T
  }

  return (await response.json()) as T
}

export const api = {
  get: <T>(path: string, init?: RequestOptions) => apiFetch<T>(path, { ...init, method: 'GET' }),
  post: <T>(path: string, body?: unknown, init?: RequestOptions) =>
    apiFetch<T>(path, { ...init, method: 'POST', body }),
  put: <T>(path: string, body?: unknown, init?: RequestOptions) =>
    apiFetch<T>(path, { ...init, method: 'PUT', body }),
  delete: <T>(path: string, init?: RequestOptions) => apiFetch<T>(path, { ...init, method: 'DELETE' }),
}

export function isUnauthorized(error: unknown): boolean {
  return error instanceof ApiError && error.status === 401
}

/** Human-readable (Polish) message for any error thrown by an API call. */
export function getErrorMessage(error: unknown): string {
  if (error instanceof ApiError) {
    const firstValidationError = Object.values(error.problem?.errors ?? {})[0]?.[0]
    return firstValidationError ?? error.problem?.detail ?? error.problem?.title ?? `Błąd serwera (${error.status})`
  }
  if (error instanceof TypeError) {
    return 'Brak połączenia z serwerem.'
  }
  return 'Wystąpił nieoczekiwany błąd.'
}
