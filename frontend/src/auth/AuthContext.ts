import { createContext, useContext } from 'react'
import type { CurrentUser, LoginRequest } from './authApi'
import type { Role } from './roles'

export interface AuthContextValue {
  user: CurrentUser | null
  isLoading: boolean
  login: (request: LoginRequest) => Promise<CurrentUser>
  logout: () => Promise<void>
  hasAnyRole: (...roles: Role[]) => boolean
}

export const AuthContext = createContext<AuthContextValue | null>(null)

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within <AuthProvider>')
  }
  return context
}
