import { useEffect, useState } from 'react';
import { AppShell } from './app/AppShell';
import { ROUTES, getRouteFromHash, type RouteKey } from './app/routes';
import { DashboardPage } from './pages/DashboardPage';
import { EmployeesPage } from './pages/EmployeesPage';
import { PayrollCyclesPage } from './pages/PayrollCyclesPage';
import { WorkEntriesPage } from './pages/WorkEntriesPage';
import './App.css';

function App() {
  const [activeRoute, setActiveRoute] = useState<RouteKey>(() => getRouteFromHash(window.location.hash));

  useEffect(() => {
    if (!window.location.hash) {
      window.location.hash = '#dashboard';
    }

    const handleHashChange = () => {
      setActiveRoute(getRouteFromHash(window.location.hash));
    };

    window.addEventListener('hashchange', handleHashChange);
    return () => window.removeEventListener('hashchange', handleHashChange);
  }, []);

  return (
    <AppShell activeRoute={activeRoute} routes={ROUTES}>
      {activeRoute === 'dashboard' && <DashboardPage />}
      {activeRoute === 'employees' && <EmployeesPage />}
      {activeRoute === 'payroll-cycles' && <PayrollCyclesPage />}
      {activeRoute === 'work-entries' && <WorkEntriesPage />}
    </AppShell>
  );
}

export default App;
