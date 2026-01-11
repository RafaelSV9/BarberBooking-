import Link from "next/link";

export default function Home() {
  return (
    <main className="relative">
      {/* Imagem de fundo */}
      <div
        className="absolute inset-0 bg-cover bg-center"
        style={{
          backgroundImage:
            "url('https://images.unsplash.com/photo-1599351431202-1e0f0137899a?auto=format&fit=crop&w=2400&q=80')",
        }}
      />
      {/* Overlay */}
      <div className="absolute inset-0 bg-gradient-to-b from-black/80 via-black/70 to-zinc-950" />

      <section className="relative mx-auto flex min-h-[calc(100vh-72px)] max-w-6xl items-center px-6 py-14">
        <div className="grid w-full grid-cols-1 gap-10 lg:grid-cols-2">
          {/* Texto principal */}
          <div className="flex flex-col justify-center">
            <div className="inline-flex w-fit items-center gap-2 rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-zinc-200">
              <span className="h-2 w-2 rounded-full bg-amber-500" />
              Agendamento rápido e organizado
            </div>

            <h1 className="mt-6 text-4xl font-black leading-tight tracking-tight md:text-5xl">
              Agendamento da Barbearia,
              <span className="text-amber-500"> do jeito certo</span>.
            </h1>

            <p className="mt-4 max-w-xl text-base text-zinc-300 md:text-lg">
              Escolha o barbeiro, selecione um horário e confirme em poucos
              cliques. Visual moderno, simples e pronto para crescer.
            </p>

            <div className="mt-8 flex flex-col gap-3 sm:flex-row">
              <Link
                href="/agendar"
                className="inline-flex items-center justify-center rounded-2xl bg-amber-500 px-6 py-3 text-sm font-extrabold text-zinc-950 hover:bg-amber-400 transition"
              >
                ✂️ Ir para agendamento
              </Link>

              <Link
                href="/admin"
                className="inline-flex items-center justify-center rounded-2xl border border-white/15 bg-white/5 px-6 py-3 text-sm font-bold text-white hover:bg-white/10 transition"
              >
                Painel Admin
              </Link>
            </div>

            <div className="mt-10 grid max-w-xl grid-cols-3 gap-3">
              {[
                { k: "30s", v: "Agendamento" },
                { k: "2", v: "Barbeiros" },
                { k: "0", v: "Filas" },
              ].map((x) => (
                <div
                  key={x.v}
                  className="rounded-2xl border border-white/10 bg-black/30 p-4 backdrop-blur"
                >
                  <div className="text-2xl font-black text-amber-500">
                    {x.k}
                  </div>
                  <div className="text-xs font-semibold text-zinc-200">
                    {x.v}
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Card lateral */}
          <div className="flex items-center">
            <div className="w-full rounded-3xl border border-white/10 bg-black/35 p-6 backdrop-blur">
              <h2 className="text-xl font-black">Como funciona</h2>

              <ul className="mt-4 space-y-3 text-sm text-zinc-200">
                {[
                  "Escolha o barbeiro disponível",
                  "Selecione um horário na agenda",
                  "Confirme e receba a validação",
                ].map((item) => (
                  <li key={item} className="flex gap-3">
                    <span className="mt-1 inline-block h-2 w-2 rounded-full bg-amber-500" />
                    {item}
                  </li>
                ))}
              </ul>

              <div className="mt-6 rounded-2xl border border-white/10 bg-white/5 p-4">
                <p className="text-sm text-zinc-200">
                  Próximo passo: integrar com a API (.NET) para listar horários e
                  criar agendamentos reais.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>
    </main>
  );
}
