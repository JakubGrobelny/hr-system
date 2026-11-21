import { Center, Loader } from '@mantine/core'
import { Navigate, Outlet, useLocation } from 'react-router'
import { useAuth } from './AuthContext'

/** Route guard: renders child routes only for signed-in users. */
export function RequireAuth() {
  const { user, isLoading } = useAuth()
  const location = useLocation()

  if (isLoading) {
    return (
      <Center h="100vh">
        <Loader />
      </Center>
    )
  }

  if (!user) {
    return <Navigate to="/login" replace state={{ from: location.pathname }} />
  }

  return <Outlet />
}
