/** Roles defined by the backend (HrSystem.Application.Common.Security.Roles). */
export const Roles = {
  Employee: 'Employee',
  Manager: 'Manager',
  Hr: 'Hr',
  Admin: 'Admin',
} as const

export type Role = (typeof Roles)[keyof typeof Roles]

export const roleLabels: Record<Role, string> = {
  Employee: 'Pracownik',
  Manager: 'Przełożony',
  Hr: 'Dział HR',
  Admin: 'Administrator',
}

export function roleLabel(role: string): string {
  return roleLabels[role as Role] ?? role
}

/** True when no roles are required or the user has at least one of them. */
export function hasAnyRole(userRoles: readonly string[], required?: readonly Role[]): boolean {
  return !required?.length || required.some((role) => userRoles.includes(role))
}
