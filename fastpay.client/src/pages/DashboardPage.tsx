const milestones = [
  'Create the shared `/api` proxy and client shell',
  'Add employee, payroll cycle, and work entry pages',
  'Wire the server models, contracts, and database layer',
  'Finish payroll preview and finalization',
];

export function DashboardPage() {
  return (
    <div className="page-grid">
      <article className="panel">
        <p className="eyebrow">Why this exists</p>
        <h3>Keep the client split into pages</h3>
        <p>
          The shell keeps `App.tsx` thin. Each FastPay feature now belongs in its own page
          component, which makes the later employee and payroll work easier to build and test.
        </p>
      </article>

      <article className="panel">
        <p className="eyebrow">Next milestones</p>
        <ul className="checklist">
          {milestones.map((item) => (
            <li key={item}>{item}</li>
          ))}
        </ul>
      </article>
    </div>
  );
}
