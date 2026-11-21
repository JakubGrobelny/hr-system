import { Button, Center, Stack, Text, Title } from '@mantine/core'
import { isRouteErrorResponse, useRouteError } from 'react-router'

/** Shown when rendering a route throws – prevents a blank screen. */
export function RouteErrorPage() {
  const error = useRouteError()
  const message = isRouteErrorResponse(error)
    ? `${error.status} ${error.statusText}`
    : error instanceof Error
      ? error.message
      : 'Nieznany błąd'

  return (
    <Center mih="100vh">
      <Stack align="center" gap="sm">
        <Title order={2}>Coś poszło nie tak</Title>
        <Text c="dimmed">{message}</Text>
        <Button onClick={() => window.location.assign('/')} variant="light">
          Odśwież aplikację
        </Button>
      </Stack>
    </Center>
  )
}
