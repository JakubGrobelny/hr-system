import { api, isUnauthorized } from '../api/client'

export interface CurrentUser {
  id: string
  email: string
  employeeId: string
  firstName: string
  lastName: string
  roles: string[]
}

export interface LoginRequest {
  email: string
  password: string
  rememberMe?: boolean
}

export const authKeys = {
  me: ['auth', 'me'] as const,
}

export const authApi = {
  login: (request: LoginRequest) => api.post<CurrentUser>('/api/auth/login', request),
  logout: () => api.post<void>('/api/auth/logout'),
  /** Returns the signed-in user, or null when there is no active session. */
  me: async (): Promise<CurrentUser | null> => {
    try {
      return await api.get<CurrentUser>('/api/auth/me')
    } catch (error) {
      if (isUnauthorized(error)) return null
      throw error
    }
  },
}
