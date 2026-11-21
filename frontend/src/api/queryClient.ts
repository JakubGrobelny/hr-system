import { MutationCache, QueryCache, QueryClient } from '@tanstack/react-query'
import { isUnauthorized } from './client'
import { authKeys } from '../auth/authApi'

/**
 * Shared TanStack Query client. A 401 from any request means the session has expired:
 * clearing the cached user makes the route guard redirect to the login page.
 */
export const queryClient: QueryClient = new QueryClient({
  queryCache: new QueryCache({ onError: handleError }),
  mutationCache: new MutationCache({ onError: handleError }),
  defaultOptions: {
    queries: {
      staleTime: 30_000,
      refetchOnWindowFocus: false,
      retry: (failureCount, error) => !isUnauthorized(error) && failureCount < 2,
    },
  },
})

function handleError(error: unknown) {
  if (isUnauthorized(error)) {
    queryClient.setQueryData(authKeys.me, null)
  }
}
