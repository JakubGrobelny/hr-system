import { afterEach, describe, expect, it, vi } from 'vitest'
import { ApiError, api, getErrorMessage, isUnauthorized } from './client'

function mockFetch(response: Response) {
  const fetchMock = vi.fn().mockResolvedValue(response)
  vi.stubGlobal('fetch', fetchMock)
  return fetchMock
}

function problemResponse(status: number, problem: object) {
  return new Response(JSON.stringify(problem), {
    status,
    headers: { 'Content-Type': 'application/problem+json' },
  })
}

afterEach(() => {
  vi.unstubAllGlobals()
})

describe('api client', () => {
  it('sends JSON bodies and parses JSON responses', async () => {
    const fetchMock = mockFetch(Response.json({ id: 1 }))

    const result = await api.post<{ id: number }>('/api/things', { name: 'x' })

    expect(result).toEqual({ id: 1 })
    const [path, init] = fetchMock.mock.calls[0] as [string, RequestInit]
    expect(path).toBe('/api/things')
    expect(init.method).toBe('POST')
    expect(init.body).toBe('{"name":"x"}')
    expect(init.credentials).toBe('same-origin')
    expect(init.headers).toMatchObject({ 'Content-Type': 'application/json' })
  })

  it('returns undefined for 204 No Content', async () => {
    mockFetch(new Response(null, { status: 204 }))

    await expect(api.post('/api/auth/logout')).resolves.toBeUndefined()
  })

  it('throws ApiError carrying problem details', async () => {
    mockFetch(problemResponse(404, { title: 'Resource not found', status: 404, detail: 'Employee was not found.' }))

    const error = await api.get('/api/employees/1').catch((e: unknown) => e)

    expect(error).toBeInstanceOf(ApiError)
    expect((error as ApiError).status).toBe(404)
    expect(getErrorMessage(error)).toBe('Employee was not found.')
  })

  it('detects unauthorized responses', async () => {
    mockFetch(new Response(null, { status: 401 }))

    const error = await api.get('/api/auth/me').catch((e: unknown) => e)

    expect(isUnauthorized(error)).toBe(true)
  })
})

describe('getErrorMessage', () => {
  it('prefers the first validation error', () => {
    const error = new ApiError(400, { title: 'Validation failed', errors: { Email: ['Invalid e-mail.'] } })

    expect(getErrorMessage(error)).toBe('Invalid e-mail.')
  })

  it('reports network failures', () => {
    expect(getErrorMessage(new TypeError('Failed to fetch'))).toBe('Brak połączenia z serwerem.')
  })
})
