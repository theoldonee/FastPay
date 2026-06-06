const fields = ['Full name', 'Hourly rate', 'Active status', 'Created at', 'Updated at'];

export function EmployeesPage() {
  return (
    <div className="page-grid">
      <article className="panel">
        <p className="eyebrow">Employees</p>
        <h3>Dedicated page for staff management</h3>
        <p>
          Move the employee list, form, and validation states here instead of keeping them in
          `App.tsx`.
        </p>
      </article>

      <article className="panel">
        <p className="eyebrow">Planned fields</p>
        <ul className="field-list">
          {fields.map((field) => (
            <li key={field}>{field}</li>
          ))}
        </ul>
      </article>
    </div>
  );
}
