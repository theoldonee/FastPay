export type RouteKey = 'dashboard' | 'employees' | 'payroll-cycles' | 'work-entries';

export type RouteDefinition = {
  key: RouteKey;
  label: string;
  description: string;
  hash: string;
};

export const ROUTES: RouteDefinition[] = [
  {
    key: 'dashboard',
    label: 'Dashboard',
    description: 'Project status and next steps',
    hash: '#dashboard',
  },
  {
    key: 'employees',
    label: 'Employees',
    description: 'Manage restaurant staff',
    hash: '#employees',
  },
  {
    key: 'payroll-cycles',
    label: 'Payroll Cycles',
    description: 'Create and close pay periods',
    hash: '#payroll-cycles',
  },
  {
    key: 'work-entries',
    label: 'Work Entries',
    description: 'Record hours for a cycle',
    hash: '#work-entries',
  },
];

export function getRouteFromHash(hash: string): RouteKey {
  const cleanedHash = hash.replace(/^#\/?/, '');
  const matchedRoute = ROUTES.find((route) => route.key === cleanedHash);
  return matchedRoute?.key ?? 'dashboard';
}
