"use client";

import { useEffect, useState } from "react";
import { API_BASE, waLink, fmtTimeFromIso } from "../../lib/api";

export default function Agendamento({ params }) {
  const { id } = params;
  const [appt, setAppt] = useState(null);
  const [err, setErr] = useState("");

  useEffect(() => {
    fetch(`${API_BASE}/api/appointments/${id}`)
      .then(r => r.ok ? r.json() : Promise.reject())
      .then(setAppt)
      .catch(() => setErr("Agendamento não encontrado."));
  }, [id]);

  if (err) return <p>{err}</p>;
  if (!appt) return <p>Carregando...</p>;

  const dateStr = new Date(appt.startsAt).toLocaleDateString("pt-BR");
  const timeStr = fmtTimeFromIso(appt.startsAt);

  const confirmMsg =
`Olá! Quero CONFIRMAR meu agendamento.
Nome: ${appt.customerName}
Barbeiro: ${appt.barberName}
Data/Hora: ${dateStr} ${timeStr}
Código: ${appt.id}`;

  const cancelMsg =
`Olá! Quero CANCELAR meu agendamento.
Nome: ${appt.customerName}
Barbeiro: ${appt.barberName}
Data/Hora: ${dateStr} ${timeStr}
Código: ${appt.id}`;

  return (
    <main>
      <h1>Agendamento criado ✅</h1>
      <p><strong>Barbeiro:</strong> {appt.barberName}</p>
      <p><strong>Data:</strong> {dateStr}</p>
      <p><strong>Horário:</strong> {timeStr}</p>
      <p><strong>Status:</strong> {appt.status}</p>

      <div style={{ display: "flex", gap: 12, marginTop: 16, flexWrap: "wrap" }}>
        <a href={waLink(confirmMsg)} target="_blank" rel="noreferrer">
          <button style={{ padding: 10, cursor: "pointer" }}>Confirmar no WhatsApp</button>
        </a>
        <a href={waLink(cancelMsg)} target="_blank" rel="noreferrer">
          <button style={{ padding: 10, cursor: "pointer" }}>Cancelar no WhatsApp</button>
        </a>
      </div>

      <p style={{ marginTop: 16, opacity: 0.8 }}>
        Obs.: No MVP, a confirmação/cancelamento é via WhatsApp (link). Depois dá pra automatizar via WhatsApp Business API.
      </p>
    </main>
  );
}
