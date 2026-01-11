"use client";

import { useEffect, useMemo, useState } from "react";
import { API_BASE } from "../lib/api";

function todayPlusDays(days) {
  const d = new Date();
  d.setDate(d.getDate() + days);
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, "0");
  const dd = String(d.getDate()).padStart(2, "0");
  return `${y}-${m}-${dd}`;
}

export default function Agendar() {
  const [barbers, setBarbers] = useState([]);
  const [barberId, setBarberId] = useState("");
  const [date, setDate] = useState(todayPlusDays(0));
  const [slots, setSlots] = useState([]);
  const [time, setTime] = useState("");
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [msg, setMsg] = useState("");

  const maxDate = useMemo(() => todayPlusDays(30), []);

  useEffect(() => {
    fetch(`${API_BASE}/api/barbers`)
      .then(r => r.json())
      .then(data => {
        setBarbers(data);
        if (data?.length) setBarberId(data[0].id);
      })
      .catch(() => setMsg("Falha ao carregar barbeiros"));
  }, []);

  useEffect(() => {
    if (!barberId || !date) return;
    setTime("");
    fetch(`${API_BASE}/api/availability?barberId=${barberId}&date=${date}`)
      .then(r => r.json())
      .then(setSlots)
      .catch(() => setMsg("Falha ao carregar horários"));
  }, [barberId, date]);

  async function submit() {
    setMsg("");
    if (!barberId || !date || !time || !name || !phone) {
      setMsg("Preencha todos os campos.");
      return;
    }
    // Build startsAt in Sao Paulo offset (-03:00)
    const startsAt = `${date}T${time}:00-03:00`;
    const res = await fetch(`${API_BASE}/api/appointments`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        barberId,
        customerName: name,
        customerPhone: phone,
        startsAt
      })
    });
    if (!res.ok) {
      const t = await res.text();
      setMsg(`Erro: ${t}`);
      return;
    }
    const data = await res.json();
    window.location.href = `/agendamento/${data.id}`;
  }

  return (
    <main>
      <h1>Agendar</h1>

      <div style={{ display: "grid", gap: 12, maxWidth: 520 }}>
        <label>
          Barbeiro
          <select value={barberId} onChange={e => setBarberId(e.target.value)} style={{ width: "100%", padding: 8 }}>
            {barbers.map(b => <option key={b.id} value={b.id}>{b.name}</option>)}
          </select>
        </label>

        <label>
          Data (até 30 dias)
          <input type="date" value={date} min={todayPlusDays(0)} max={maxDate}
                 onChange={e => setDate(e.target.value)} style={{ width: "100%", padding: 8 }} />
        </label>

        <label>
          Horário
          <select value={time} onChange={e => setTime(e.target.value)} style={{ width: "100%", padding: 8 }}>
            <option value="">Selecione</option>
            {slots.filter(s => s.available).map(s => (
              <option key={s.time} value={s.time}>{s.time}</option>
            ))}
          </select>
        </label>

        <label>
          Nome
          <input value={name} onChange={e => setName(e.target.value)} style={{ width: "100%", padding: 8 }} />
        </label>

        <label>
          WhatsApp
          <input value={phone} onChange={e => setPhone(e.target.value)} placeholder="ex: 43999999999"
                 style={{ width: "100%", padding: 8 }} />
        </label>

        <button onClick={submit} style={{ padding: 10, cursor: "pointer" }}>
          Concluir agendamento
        </button>

        {msg && <p style={{ color: "crimson" }}>{msg}</p>}
      </div>
    </main>
  );
}
