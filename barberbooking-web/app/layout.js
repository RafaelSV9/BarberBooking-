import "./globals.css";
import Link from "next/link";

const nav = [
  { href: "/", label: "Home" },
  { href: "/agendar", label: "Agendar" },
  { href: "/admin", label: "Admin" },
];

export const metadata = {
  title: "BarberBooking",
  description: "Agendamento online para barbearias",
};

export default function RootLayout({ children }) {
  return (
    <html lang="pt-BR">
      <body className="min-h-screen bg-zinc-950 text-zinc-100">
        <header className="sticky top-0 z-50 border-b border-white/10 bg-zinc-950/70 backdrop-blur">
          <nav className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
            <Link href="/" className="flex items-center gap-2">
              <span className="inline-flex h-9 w-9 items-center justify-center rounded-xl bg-amber-500 text-zinc-950 font-black">
                B
              </span>
              <span className="text-lg font-bold tracking-tight">
                BarberBooking
              </span>
            </Link>

            <div className="flex items-center gap-2">
              {nav.map((item) => (
                <Link
                  key={item.href}
                  href={item.href}
                  className="rounded-xl px-4 py-2 text-sm font-semibold text-zinc-200 hover:bg-white/10 hover:text-white transition"
                >
                  {item.label}
                </Link>
              ))}

              <Link
                href="/agendar"
                className="ml-2 rounded-xl bg-amber-500 px-4 py-2 text-sm font-extrabold text-zinc-950 hover:bg-amber-400 transition"
              >
                Agendar agora
              </Link>
            </div>
          </nav>
        </header>

        <div>{children}</div>
      </body>
    </html>
  );
}
