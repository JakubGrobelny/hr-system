import { Badge, Card, Group, SimpleGrid, Stack, Text, ThemeIcon, Title } from '@mantine/core'
import { useQuery } from '@tanstack/react-query'
import { Link } from 'react-router'
import { useAuth } from '../auth/AuthContext'
import { roleLabel } from '../auth/roles'
import { employeesApi, employeeKeys } from '../features/employees/api'
import { Milestones, allModules, visibleNavigation } from '../layout/navigation'
import { QueryState } from './QueryState'

export function DashboardPage() {
  const { user } = useAuth()
  const profile = useQuery({ queryKey: employeeKeys.me, queryFn: employeesApi.getMe })

  if (!user) return null

  const modules = visibleNavigation(user.roles)
    .flatMap((section) => section.items)
    .filter((module) => module.key !== 'dashboard')

  return (
    <Stack gap="lg">
      <Title order={2}>Witaj, {user.firstName}!</Title>

      <Card withBorder radius="md">
        <Title order={4} mb="sm">
          Mój profil
        </Title>
        <QueryState query={profile}>
          {(employee) => (
            <Stack gap={4}>
              <Text fw={500}>
                {employee.firstName} {employee.lastName}
              </Text>
              <Text size="sm" c="dimmed">
                {employee.email}
              </Text>
              <Group gap="xs" mt="xs">
                {user.roles.map((role) => (
                  <Badge key={role} variant="light">
                    {roleLabel(role)}
                  </Badge>
                ))}
              </Group>
            </Stack>
          )}
        </QueryState>
      </Card>

      <div>
        <Title order={4} mb="sm">
          Moduły
        </Title>
        <SimpleGrid cols={{ base: 1, sm: 2, lg: 3 }}>
          {modules.map((module) => (
            <Card key={module.key} withBorder radius="md" component={Link} to={module.path}>
              <Group justify="space-between" mb="xs" wrap="nowrap">
                <Group gap="sm" wrap="nowrap">
                  <ThemeIcon variant="light" size="lg">
                    <module.icon size={20} />
                  </ThemeIcon>
                  <Text fw={500}>{module.label}</Text>
                </Group>
                {module.milestone !== Milestones.skeleton && (
                  <Badge size="xs" variant="outline" color="gray">
                    {module.milestone}
                  </Badge>
                )}
              </Group>
              <Text size="sm" c="dimmed">
                {module.description}
              </Text>
            </Card>
          ))}
        </SimpleGrid>
      </div>

      <Text size="xs" c="dimmed">
        Moduły dostępne w systemie: {allModules.length}. Daty przy modułach oznaczają termin ich realizacji.
      </Text>
    </Stack>
  )
}
