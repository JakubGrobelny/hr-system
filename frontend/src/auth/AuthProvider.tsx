import { useQuery, useQueryClient } from '@tanstack/react-query'
import { useCallback, useMemo, type ReactNode } from 'react'
import { authApi, authKeys, type LoginRequest } from './authApi'
import { AuthContext, type AuthContextValue } from './AuthContext'
import { hasAnyRole, type Role } from './roles'

export function AuthProvider({ children }: { children: ReactNode }) {
  const queryClient = useQueryClient()
  const { data: user = null, isPending } = useQuery({
    queryKey: authKeys.me,
    queryFn: authApi.me,
    staleTime: Infinity,
  })

  const login = useCallback(
    async (request: LoginRequest) => {
      const signedIn = await authApi.login(request)
      queryClient.setQueryData(authKeys.me, signedIn)
      return signedIn
    },
    [queryClient],
  )

  const logout = useCallback(async () => {
    try {
      await authApi.logout()
    } finally {
      // Drop all cached data of the previous user.
      queryClient.clear()
      queryClient.setQueryData(authKeys.me, null)
    }
  }, [queryClient])

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isLoading: isPending,
      login,
      logout,
      hasAnyRole: (...roles: Role[]) => !!user && hasAnyRole(user.roles, roles),
    }),
    [user, isPending, login, logout],
  )

  return <AuthContext value={value}>{children}</AuthContext>
}
