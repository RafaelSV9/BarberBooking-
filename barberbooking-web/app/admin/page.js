"use client";

import { useEffect, useState } from "react";
import { API_BASE } from "../lib/api";

function today() {
  const d = new Date();
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, "0");
  const dd = String(d.getDate()).padStart(2, "0");
  return `${y}-${m}-${dd}`;
}

export default function Admin() {
  const [token, setToken] = useState("");
  const [date, setDate] = useState(today());
  const [barberId, setBarberId] = useState("");
  const [barbers, setBarbers] = useState([]);
  const [items, setItems] = useState([]);
  const [msg, setMsg] = useState("");

  useEffect(() => {
    const t = localStorage.getItem("admin_token") || "";
    if (!t) window.location.href = "/admin/login";
    setToken(t);

    fetch(`${API_BASE}/api/barbers`)
      .then(r => r.json())
      .then(data => {
        setBarbers(data);
        if (data?.length) setBarberId(data[0].id);
      });
  }, []);

  async function load() {
    setMsg("");
    const q = barberId ? `&barberId=${barberId}` : "";
    const res = await fetch(`${API_BASE}/api/admin/appointments?date=${date}${q}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) {
      setMsg("Falha ao carregar agenda (token válido?)");
      return;
    }
    setItems(await res.json());
  }

  useEffect(() => {
    if (token) load();
  }, [token, date, barberId]);

  async function action(id, kind) {
    const res = await fetch(`${API_BASE}/api/admin/appointments/${id}/${kind}`, {
      method: "PATCH",
      headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) {
      setMsg("Falha ao atualizar");
      return;
    }
    load();
  }

  return (
    <main>
      <h1>Admin - Agenda</h1>

      <div style={{ display: "flex", gap: 12, flexWrap: "wrap", marginBottom: 12 }}>
        <label>
          Data
          <input type="date" value={date} onChange={e => setDate(e.target.value)} style={{ marginLeft: 8, padding: 6 }} />
        </label>

        <label>
          Barbeiro
          <select value={barberId} onChange={e => setBarberId(e.target.value)} style={{ marginLeft: 8, padding: 6 }}>
            {barbers.map(b => <option key={b.id} value={b.id}>{b.name}</option>)}
          </select>
        </label>

        <button onClick={() => { localStorage.removeItem("admin_token"); window.location.href="/admin/login"; }} style={{ padding: 8, cursor:"pointer" }}>
          Sair
        </button>
      </div>

      {msg && <p style={{ color: "crimson" }}>{msg}</p>}

      <table border="1" cellPadding="8" style={{ borderCollapse: "collapse", width: "100%", maxWidth: 900 }}>
        <thead>
          <tr>
            <th>Hora</th>
            <th>Cliente</th>
            <th>Whats</th>
            <th>Status</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {items.map(it => {
            const dt = new Date(it.startsAt);
            const time = dt.toLocaleTimeString("pt-BR", { hour:"2-digit", minute:"2-digit" });
            return (
              <tr key={it.id}>
                <td>{time}</td>
                <td>{it.customerName}</td>
                <td>{it.customerPhone}</td>
                <td>{it.status}</td>
                <td style={{ display: "flex", gap: 8 }}>
                  <button onClick={() => action(it.id, "confirm")} style={{ cursor:"pointer" }}>Confirmar</button>
                  <button onClick={() => action(it.id, "cancel")} style={{ cursor:"pointer" }}>Cancelar</button>
                </td>
              </tr>
            );
          })}
          {!items.length && (
            <tr><td colSpan="5" style={{ textAlign: "center", opacity: 0.7 }}>Sem agendamentos</td></tr>
          )}
        </tbody>
      </table>
    </main>
  );
}
