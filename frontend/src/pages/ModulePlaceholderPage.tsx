import { Badge, Card, Group, Stack, Text, ThemeIcon, Title } from '@mantine/core'
import type { ModuleDefinition } from '../layout/navigation'

/** Placeholder for modules scheduled for a later milestone. */
export function ModulePlaceholderPage({ module }: { module: ModuleDefinition }) {
  return (
    <Stack>
      <Group gap="sm">
        <ThemeIcon variant="light" size="xl">
          <module.icon size={24} />
        </ThemeIcon>
        <Title order={2}>{module.label}</Title>
      </Group>
      <Card withBorder radius="md">
        <Stack gap="xs">
          <Text>{module.description}</Text>
          <Group gap="xs">
            <Text size="sm" c="dimmed">
              Moduł w przygotowaniu – planowany termin realizacji:
            </Text>
            <Badge variant="light">{module.milestone}</Badge>
          </Group>
        </Stack>
      </Card>
    </Stack>
  )
}
