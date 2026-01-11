export const metadata = { title: "BarberBooking" };

export default function RootLayout({ children }) {
  return (
    <html lang="pt-BR">
      <body style={{ fontFamily: "system-ui, Arial", margin: 0, padding: 0 }}>
        <div style={{ padding: 16, borderBottom: "1px solid #eee" }}>
          <strong>BarberBooking</strong>
          <span style={{ marginLeft: 12 }}>
            <a href="/" style={{ marginRight: 12 }}>Home</a>
            <a href="/agendar" style={{ marginRight: 12 }}>Agendar</a>
            <a href="/admin/login">Admin</a>
          </span>
        </div>
        <div style={{ padding: 16 }}>{children}</div>
      </body>
    </html>
  );
}
