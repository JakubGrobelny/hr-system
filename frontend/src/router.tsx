import type { ReactNode } from 'react'
import { createBrowserRouter, type RouteObject } from 'react-router'
import { RequireAuth } from './auth/RequireAuth'
import { RequireRole } from './auth/RequireRole'
import { UsersPage } from './features/admin/UsersPage'
import { AppLayout } from './layout/AppLayout'
import { allModules, type ModuleKey } from './layout/navigation'
import { DashboardPage } from './pages/DashboardPage'
import { LoginPage } from './pages/LoginPage'
import { ModulePlaceholderPage } from './pages/ModulePlaceholderPage'
import { NotFoundPage } from './pages/NotFoundPage'
import { RouteErrorPage } from './pages/RouteErrorPage'

/** Pages implemented so far; remaining modules render a placeholder until their milestone. */
const pages: Partial<Record<ModuleKey, ReactNode>> = {
  dashboard: <DashboardPage />,
  adminUsers: <UsersPage />,
}

const moduleRoutes: RouteObject[] = allModules.map((module) => {
  const element = (
    <RequireRole roles={module.roles}>{pages[module.key] ?? <ModulePlaceholderPage module={module} />}</RequireRole>
  )
  return module.path === '/' ? { index: true, element } : { path: module.path.slice(1), element }
})

export const router = createBrowserRouter([
  { path: '/login', element: <LoginPage />, errorElement: <RouteErrorPage /> },
  {
    element: <RequireAuth />,
    errorElement: <RouteErrorPage />,
    children: [
      {
        path: '/',
        element: <AppLayout />,
        children: [...moduleRoutes, { path: '*', element: <NotFoundPage /> }],
      },
    ],
  },
])
