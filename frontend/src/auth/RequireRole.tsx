import type { ReactNode } from 'react'
import { ForbiddenPage } from '../pages/ForbiddenPage'
import { useAuth } from './AuthContext'
import type { Role } from './roles'

/**
 * Hides a page from users without any of the given roles. This only improves UX –
 * the backend enforces the same rules on every request.
 */
export function RequireRole({ roles, children }: { roles?: readonly Role[]; children: ReactNode }) {
  const { hasAnyRole } = useAuth()

  if (roles?.length && !hasAnyRole(...roles)) {
    return <ForbiddenPage />
  }

  return children
}
