import { Badge, Card, Group, Stack, Table, Text, Title } from '@mantine/core'
import { useQuery } from '@tanstack/react-query'
import { roleLabel } from '../../auth/roles'
import { QueryState } from '../../pages/QueryState'
import { adminApi, adminKeys } from './api'

export function UsersPage() {
  const users = useQuery({ queryKey: adminKeys.users, queryFn: adminApi.getUsers })

  return (
    <Stack>
      <Title order={2}>Użytkownicy i role</Title>
      <Card withBorder radius="md" p={0}>
        <QueryState query={users}>
          {(data) => (
            <Table.ScrollContainer minWidth={600}>
              <Table striped highlightOnHover verticalSpacing="sm">
                <Table.Thead>
                  <Table.Tr>
                    <Table.Th>Imię i nazwisko</Table.Th>
                    <Table.Th>E-mail</Table.Th>
                    <Table.Th>Role</Table.Th>
                    <Table.Th>Status</Table.Th>
                  </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                  {data.map((user) => (
                    <Table.Tr key={user.id}>
                      <Table.Td>
                        <Text fw={500}>{user.fullName}</Text>
                      </Table.Td>
                      <Table.Td>{user.email}</Table.Td>
                      <Table.Td>
                        <Group gap={4}>
                          {user.roles.map((role) => (
                            <Badge key={role} variant="light" size="sm">
                              {roleLabel(role)}
                            </Badge>
                          ))}
                        </Group>
                      </Table.Td>
                      <Table.Td>
                        {user.isLockedOut ? (
                          <Badge color="red" variant="light">
                            Zablokowany
                          </Badge>
                        ) : (
                          <Badge color="green" variant="light">
                            Aktywny
                          </Badge>
                        )}
                      </Table.Td>
                    </Table.Tr>
                  ))}
                </Table.Tbody>
              </Table>
            </Table.ScrollContainer>
          )}
        </QueryState>
      </Card>
    </Stack>
  )
}
