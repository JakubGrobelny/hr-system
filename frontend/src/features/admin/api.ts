import { api } from '../../api/client'

export interface UserSummary {
  id: string
  email: string
  employeeId: string
  fullName: string
  roles: string[]
  isLockedOut: boolean
}

export const adminKeys = {
  users: ['admin', 'users'] as const,
}

export const adminApi = {
  getUsers: () => api.get<UserSummary[]>('/api/admin/users'),
}
