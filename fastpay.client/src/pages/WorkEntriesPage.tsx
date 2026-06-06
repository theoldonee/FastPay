const steps = [
  'Select a payroll cycle',
  'Select an employee',
  'Enter hours and notes',
  'Save via a single upsert endpoint',
];

export function WorkEntriesPage() {
  return (
    <div className="page-grid">
      <article className="panel">
        <p className="eyebrow">Work entries</p>
        <h3>Hours entry stays isolated from the app shell</h3>
        <p>
          This page will later host the work-hours form and its request states, without forcing the
          shell to know the details.
        </p>
      </article>

      <article className="panel">
        <p className="eyebrow">Flow</p>
        <ul className="field-list">
          {steps.map((step) => (
            <li key={step}>{step}</li>
          ))}
        </ul>
      </article>
    </div>
  );
}
