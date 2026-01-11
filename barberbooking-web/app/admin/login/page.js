"use client";

import { useState } from "react";
import { API_BASE } from "../../lib/api";

export default function AdminLogin() {
  const [user, setUser] = useState("admin");
  const [password, setPassword] = useState("");
  const [msg, setMsg] = useState("");

  async function submit() {
    setMsg("");
    const res = await fetch(`${API_BASE}/api/admin/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ user, password })
    });
    if (!res.ok) {
      setMsg("Login inválido");
      return;
    }
    const data = await res.json();
    localStorage.setItem("admin_token", data.token);
    window.location.href = "/admin";
  }

  return (
    <main style={{ maxWidth: 420 }}>
      <h1>Admin - Login</h1>

      <label>Usuário</label>
      <input value={user} onChange={e => setUser(e.target.value)} style={{ width: "100%", padding: 8, marginBottom: 10 }} />

      <label>Senha</label>
      <input type="password" value={password} onChange={e => setPassword(e.target.value)} style={{ width: "100%", padding: 8, marginBottom: 10 }} />

      <button onClick={submit} style={{ padding: 10, cursor: "pointer" }}>Entrar</button>

      {msg && <p style={{ color: "crimson" }}>{msg}</p>}
    </main>
  );
}
