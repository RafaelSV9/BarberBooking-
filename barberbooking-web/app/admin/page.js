export default function AdminLoginPage() {
  return (
    <div className="grid min-h-[70vh] place-items-center px-6">
      <div className="w-full max-w-md rounded-3xl border border-white/10 bg-black/35 p-6 shadow-xl backdrop-blur">
        <div className="mb-6">
          <h1 className="text-2xl font-black tracking-tight">Admin</h1>
          <p className="mt-1 text-sm text-zinc-300">
            Acesse o painel para gerenciar agendamentos.
          </p>
        </div>

        <form className="space-y-4">
          <div className="space-y-2">
            <label className="text-sm font-semibold text-zinc-200">Usuário</label>
            <input
              defaultValue="admin"
              className="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white outline-none placeholder:text-zinc-400 focus:border-amber-500/70 focus:ring-2 focus:ring-amber-500/20"
              placeholder="Digite seu usuário"
            />
          </div>

          <div className="space-y-2">
            <label className="text-sm font-semibold text-zinc-200">Senha</label>
            <input
              type="password"
              className="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white outline-none placeholder:text-zinc-400 focus:border-amber-500/70 focus:ring-2 focus:ring-amber-500/20"
              placeholder="Digite sua senha"
            />
          </div>

          <button
            type="submit"
            className="mt-2 inline-flex w-full items-center justify-center rounded-2xl bg-amber-500 px-4 py-3 text-sm font-extrabold text-zinc-950 hover:bg-amber-400 transition"
          >
            Entrar
          </button>

          <p className="text-center text-xs text-zinc-400">
            Depois conectamos com autenticação real (API).
          </p>
        </form>
      </div>
    </div>
  );
}
