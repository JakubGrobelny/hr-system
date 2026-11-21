import { api } from '../../api/client'

export interface Employee {
  id: string
  firstName: string
  lastName: string
  email: string
  isActive: boolean
}

export const employeeKeys = {
  all: ['employees'] as const,
  me: ['employees', 'me'] as const,
  detail: (id: string) => ['employees', id] as const,
}

export const employeesApi = {
  getMe: () => api.get<Employee>('/api/employees/me'),
  getById: (id: string) => api.get<Employee>(`/api/employees/${id}`),
}
