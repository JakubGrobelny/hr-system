import {
  IconBeach,
  IconBriefcase,
  IconCalendarEvent,
  IconChartBar,
  IconChecklist,
  IconClockHour4,
  IconId,
  IconLayoutDashboard,
  IconLock,
  IconSitemap,
  IconStars,
  IconUsers,
  type TablerIcon,
} from '@tabler/icons-react'
import { Roles, hasAnyRole, type Role } from '../auth/roles'

/** Project milestones (see "Harmonogram" in the project documentation). */
export const Milestones = {
  skeleton: '21.11.2026',
  core: '19.12.2026',
  remaining: '16.01.2027',
} as const

export type ModuleKey =
  | 'dashboard'
  | 'timeTracking'
  | 'absences'
  | 'reviews'
  | 'approvals'
  | 'absenceCalendar'
  | 'organization'
  | 'projects'
  | 'reports'
  | 'hr'
  | 'adminUsers'
  | 'adminPeriods'

export interface ModuleDefinition {
  key: ModuleKey
  label: string
  path: string
  icon: TablerIcon
  description: string
  /** Roles allowed to open the module; empty means every signed-in user. */
  roles?: readonly Role[]
  /** Milestone in which the module is delivered. */
  milestone: string
}

export interface NavigationSection {
  label: string
  items: ModuleDefinition[]
}

/**
 * Single source of truth for the menu structure, routes and role-based visibility
 * of every functional module of the system.
 */
export const navigation: NavigationSection[] = [
  {
    label: 'Moja praca',
    items: [
      {
        key: 'dashboard',
        label: 'Pulpit',
        path: '/',
        icon: IconLayoutDashboard,
        description: 'Podsumowanie najważniejszych informacji.',
        milestone: Milestones.skeleton,
      },
      {
        key: 'timeTracking',
        label: 'Czas pracy',
        path: '/time-tracking',
        icon: IconClockHour4,
        description: 'Rejestrowanie czasu pracy (start/stop), tygodniowy timesheet i wysyłanie go do zatwierdzenia.',
        milestone: Milestones.core,
      },
      {
        key: 'absences',
        label: 'Nieobecności',
        path: '/absences',
        icon: IconBeach,
        description: 'Wnioski o urlop i inne nieobecności, ich status oraz wykorzystany i pozostały limit urlopowy.',
        milestone: Milestones.core,
      },
      {
        key: 'reviews',
        label: 'Oceny pracownicze',
        path: '/reviews',
        icon: IconStars,
        description: 'Samoocena, ocena przełożonego i historia ocen pracowniczych.',
        milestone: Milestones.remaining,
      },
    ],
  },
  {
    label: 'Zespół',
    items: [
      {
        key: 'approvals',
        label: 'Do zatwierdzenia',
        path: '/approvals',
        icon: IconChecklist,
        description: 'Timesheety i wnioski o nieobecność oczekujące na decyzję przełożonego lub zastępcy.',
        roles: [Roles.Manager],
        milestone: Milestones.core,
      },
      {
        key: 'absenceCalendar',
        label: 'Kalendarz nieobecności',
        path: '/absence-calendar',
        icon: IconCalendarEvent,
        description: 'Zaplanowane nieobecności innych osób w zespole i organizacji.',
        milestone: Milestones.core,
      },
    ],
  },
  {
    label: 'Firma',
    items: [
      {
        key: 'organization',
        label: 'Struktura organizacyjna',
        path: '/organization',
        icon: IconSitemap,
        description: 'Zespoły, stanowiska, przełożeni oraz wyszukiwarka pracowników.',
        milestone: Milestones.core,
      },
      {
        key: 'projects',
        label: 'Projekty',
        path: '/projects',
        icon: IconBriefcase,
        description: 'Projekty klientów i przypisani do nich pracownicy.',
        milestone: Milestones.core,
      },
      {
        key: 'reports',
        label: 'Raporty',
        path: '/reports',
        icon: IconChartBar,
        description: 'Raporty czasu pracy dla projektów oraz raporty obecności z eksportem do pliku.',
        roles: [Roles.Manager, Roles.Hr, Roles.Admin],
        milestone: Milestones.remaining,
      },
    ],
  },
  {
    label: 'Kadry',
    items: [
      {
        key: 'hr',
        label: 'Dane kadrowe',
        path: '/hr',
        icon: IconId,
        description:
          'Profile pracowników, warunki i historia zatrudnienia, wynagrodzenia, limity urlopowe oraz cykle ocen.',
        roles: [Roles.Hr],
        milestone: Milestones.remaining,
      },
    ],
  },
  {
    label: 'Administracja',
    items: [
      {
        key: 'adminUsers',
        label: 'Użytkownicy i role',
        path: '/admin/users',
        icon: IconUsers,
        description: 'Konta użytkowników i przypisane im role.',
        roles: [Roles.Admin],
        milestone: Milestones.skeleton,
      },
      {
        key: 'adminPeriods',
        label: 'Okresy rozliczeniowe',
        path: '/admin/periods',
        icon: IconLock,
        description: 'Zamykanie okresów rozliczeniowych – blokada edycji wpisów czasu pracy i timesheetów.',
        roles: [Roles.Admin],
        milestone: Milestones.core,
      },
    ],
  },
]

export const allModules: ModuleDefinition[] = navigation.flatMap((section) => section.items)

/** Navigation filtered to the modules the user is allowed to open; empty sections are dropped. */
export function visibleNavigation(userRoles: readonly string[], sections = navigation): NavigationSection[] {
  return sections
    .map((section) => ({ ...section, items: section.items.filter((item) => hasAnyRole(userRoles, item.roles)) }))
    .filter((section) => section.items.length > 0)
}
