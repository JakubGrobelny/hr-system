import { Button, Stack, Text, Title } from '@mantine/core'
import { IconLockAccess } from '@tabler/icons-react'
import { Link } from 'react-router'

export function ForbiddenPage() {
  return (
    <Stack align="center" mt="xl" gap="sm">
      <IconLockAccess size={48} stroke={1.4} />
      <Title order={2}>Brak dostępu</Title>
      <Text c="dimmed">Nie masz uprawnień do wyświetlenia tej strony.</Text>
      <Button component={Link} to="/" variant="light">
        Wróć na pulpit
      </Button>
    </Stack>
  )
}
