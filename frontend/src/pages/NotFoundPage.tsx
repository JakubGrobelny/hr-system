import { Button, Stack, Text, Title } from '@mantine/core'
import { Link } from 'react-router'

export function NotFoundPage() {
  return (
    <Stack align="center" mt="xl" gap="sm">
      <Title order={2}>Nie znaleziono strony</Title>
      <Text c="dimmed">Strona, której szukasz, nie istnieje.</Text>
      <Button component={Link} to="/" variant="light">
        Wróć na pulpit
      </Button>
    </Stack>
  )
}
