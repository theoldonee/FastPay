const rules = [
  'Cycles are bi-weekly',
  'Open cycles can be updated',
  'Finalized cycles are immutable',
];

export function PayrollCyclesPage() {
  return (
    <div className="page-grid">
      <article className="panel">
        <p className="eyebrow">Payroll cycles</p>
        <h3>One page for pay-period setup</h3>
        <p>
          This page will hold the create, list, and finalize flow for each two-week payroll
          period.
        </p>
      </article>

      <article className="panel">
        <p className="eyebrow">Rules</p>
        <ul className="field-list">
          {rules.map((rule) => (
            <li key={rule}>{rule}</li>
          ))}
        </ul>
      </article>
    </div>
  );
}
