import { Alert, Loader } from '@mantine/core'
import type { UseQueryResult } from '@tanstack/react-query'
import type { ReactNode } from 'react'
import { getErrorMessage } from '../api/client'

/** Renders a loader / error message for a query and the content once the data is available. */
export function QueryState<T>({ query, children }: { query: UseQueryResult<T>; children: (data: T) => ReactNode }) {
  if (query.isPending) return <Loader size="sm" />
  if (query.isError) return <Alert color="red">{getErrorMessage(query.error)}</Alert>
  return children(query.data)
}
