export default function AdminLayout({ children }) {
  return (
    <section className="mx-auto w-full max-w-6xl px-6 py-10">
      <main className="mx-auto max-w-6xl px-6 py-10">{children}</main>
    </section>
  );
}
