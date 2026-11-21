import { describe, expect, it } from 'vitest'
import { Roles } from '../auth/roles'
import { allModules, visibleNavigation } from './navigation'

function visibleKeys(roles: string[]) {
  return visibleNavigation(roles).flatMap((section) => section.items.map((item) => item.key))
}

describe('visibleNavigation', () => {
  it('shows only general modules to a regular employee', () => {
    const keys = visibleKeys([Roles.Employee])

    expect(keys).toContain('timeTracking')
    expect(keys).toContain('absenceCalendar')
    expect(keys).not.toContain('approvals')
    expect(keys).not.toContain('hr')
    expect(keys).not.toContain('adminUsers')
  })

  it('drops sections without visible items', () => {
    const sections = visibleNavigation([Roles.Employee]).map((section) => section.label)

    expect(sections).not.toContain('Kadry')
    expect(sections).not.toContain('Administracja')
  })

  it('shows role-specific modules to users holding the role', () => {
    expect(visibleKeys([Roles.Employee, Roles.Manager])).toContain('approvals')
    expect(visibleKeys([Roles.Employee, Roles.Hr])).toEqual(expect.arrayContaining(['hr', 'reports']))
    expect(visibleKeys([Roles.Employee, Roles.Admin])).toEqual(expect.arrayContaining(['adminUsers', 'adminPeriods']))
  })

  it('defines unique keys and paths', () => {
    expect(new Set(allModules.map((m) => m.key)).size).toBe(allModules.length)
    expect(new Set(allModules.map((m) => m.path)).size).toBe(allModules.length)
  })
})
