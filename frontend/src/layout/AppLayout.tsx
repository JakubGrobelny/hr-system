import { AppShell, Avatar, Burger, Group, Menu, NavLink, ScrollArea, Text, Title, UnstyledButton } from '@mantine/core'
import { useDisclosure } from '@mantine/hooks'
import { IconChevronDown, IconLogout } from '@tabler/icons-react'
import { Link, Outlet, useLocation, useNavigate } from 'react-router'
import { useAuth } from '../auth/AuthContext'
import { roleLabel } from '../auth/roles'
import { visibleNavigation } from './navigation'

export function AppLayout() {
  const [navbarOpened, { toggle: toggleNavbar, close: closeNavbar }] = useDisclosure()
  const { user, logout } = useAuth()
  const location = useLocation()
  const navigate = useNavigate()

  if (!user) return null

  const sections = visibleNavigation(user.roles)
  const isActive = (path: string) =>
    path === '/' ? location.pathname === '/' : location.pathname === path || location.pathname.startsWith(`${path}/`)

  const handleLogout = async () => {
    await logout()
    navigate('/login', { replace: true })
  }

  return (
    <AppShell
      header={{ height: 56 }}
      navbar={{ width: 270, breakpoint: 'sm', collapsed: { mobile: !navbarOpened } }}
      padding="lg"
    >
      <AppShell.Header>
        <Group h="100%" px="md" justify="space-between">
          <Group gap="sm">
            <Burger opened={navbarOpened} onClick={toggleNavbar} hiddenFrom="sm" size="sm" aria-label="Menu" />
            <UnstyledButton component={Link} to="/">
              <Title order={4}>HR System</Title>
            </UnstyledButton>
          </Group>

          <Menu position="bottom-end" withinPortal>
            <Menu.Target>
              <UnstyledButton aria-label="Menu użytkownika">
                <Group gap="xs">
                  <Avatar color="blue" radius="xl" size="sm">
                    {user.firstName[0]}
                    {user.lastName[0]}
                  </Avatar>
                  <Text size="sm" fw={500} visibleFrom="xs">
                    {user.firstName} {user.lastName}
                  </Text>
                  <IconChevronDown size={14} />
                </Group>
              </UnstyledButton>
            </Menu.Target>
            <Menu.Dropdown>
              <Menu.Label>{user.email}</Menu.Label>
              <Menu.Label>{user.roles.map(roleLabel).join(', ')}</Menu.Label>
              <Menu.Divider />
              <Menu.Item leftSection={<IconLogout size={16} />} onClick={handleLogout}>
                Wyloguj
              </Menu.Item>
            </Menu.Dropdown>
          </Menu>
        </Group>
      </AppShell.Header>

      <AppShell.Navbar p="sm">
        <ScrollArea>
          <nav aria-label="Menu główne">
            {sections.map((section) => (
              <div key={section.label}>
                <Text size="xs" fw={700} c="dimmed" tt="uppercase" px="sm" mt="md" mb={4}>
                  {section.label}
                </Text>
                {section.items.map((item) => (
                  <NavLink
                    key={item.key}
                    component={Link}
                    to={item.path}
                    label={item.label}
                    leftSection={<item.icon size={18} stroke={1.6} />}
                    active={isActive(item.path)}
                    onClick={closeNavbar}
                  />
                ))}
              </div>
            ))}
          </nav>
        </ScrollArea>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  )
}
