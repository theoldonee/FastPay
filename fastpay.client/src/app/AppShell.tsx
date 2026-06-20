import type { ReactNode } from 'react';
import type { RouteDefinition, RouteKey } from './routes';

type AppShellProps = {
  activeRoute: RouteKey;
  routes: RouteDefinition[];
  children: ReactNode;
};

export function AppShell({ activeRoute, routes, children }: AppShellProps) {
  const active = routes.find((route) => route.key === activeRoute) ?? routes[0];

  return (
    <div className="app-shell">
      <header className="shell-header">
        <div className="shell-intro">
          <p className="eyebrow">FastPay</p>
          <h1>Restaurant payroll, split into focused pages</h1>
          <p className="shell-subtitle">
            Keep shared navigation in one place and move each feature into its own page component.
          </p>
        </div>

        <nav className="shell-nav" aria-label="Primary">
          {routes.map((route) => (
            <a
              key={route.key}
              href={route.hash}
              className={route.key === activeRoute ? 'shell-nav-link active' : 'shell-nav-link'}
            >
              <span>{route.label}</span>
              <small>{route.description}</small>
            </a>
          ))}
        </nav>
      </header>

      <main className="shell-main">
        <section className="shell-hero">
          <div>
            <p className="eyebrow">Current page</p>
            <h2>{active.label}</h2>
            <p>{active.description}</p>
          </div>

          <div className="shell-hero-card">
            <span>Route</span>
            <strong>{active.hash}</strong>
          </div>
        </section>

        <section className="shell-content">{children}</section>
      </main>
    </div>
  );
}
