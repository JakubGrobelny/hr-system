import { Alert, Button, Center, Checkbox, Paper, PasswordInput, Stack, Text, TextInput, Title } from '@mantine/core'
import { IconAlertCircle } from '@tabler/icons-react'
import { useMutation } from '@tanstack/react-query'
import { useState, type FormEvent } from 'react'
import { Navigate, useLocation, useNavigate } from 'react-router'
import { getErrorMessage } from '../api/client'
import { useAuth } from '../auth/AuthContext'

export function LoginPage() {
  const { user, login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const redirectTo = (location.state as { from?: string } | null)?.from ?? '/'

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [rememberMe, setRememberMe] = useState(false)

  const mutation = useMutation({
    mutationFn: login,
    onSuccess: () => navigate(redirectTo, { replace: true }),
  })

  if (user) {
    return <Navigate to={redirectTo} replace />
  }

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    mutation.mutate({ email, password, rememberMe })
  }

  return (
    <Center mih="100vh" bg="gray.0" p="md">
      <Paper withBorder shadow="sm" radius="md" p="xl" w="100%" maw={400}>
        <form onSubmit={handleSubmit}>
          <Stack>
            <div>
              <Title order={2}>HR System</Title>
              <Text c="dimmed" size="sm">
                Zaloguj się, aby kontynuować
              </Text>
            </div>

            {mutation.isError && (
              <Alert color="red" icon={<IconAlertCircle size={18} />}>
                {getErrorMessage(mutation.error)}
              </Alert>
            )}

            <TextInput
              label="E-mail"
              type="email"
              autoComplete="username"
              required
              value={email}
              onChange={(event) => setEmail(event.currentTarget.value)}
            />
            <PasswordInput
              label="Hasło"
              autoComplete="current-password"
              required
              value={password}
              onChange={(event) => setPassword(event.currentTarget.value)}
            />
            <Checkbox
              label="Zapamiętaj mnie"
              checked={rememberMe}
              onChange={(event) => setRememberMe(event.currentTarget.checked)}
            />
            <Button type="submit" loading={mutation.isPending} fullWidth>
              Zaloguj
            </Button>
          </Stack>
        </form>
      </Paper>
    </Center>
  )
}
