export const API_BASE = process.env.NEXT_PUBLIC_API_BASE || "http://localhost:5000";
export const WHATSAPP_PHONE = process.env.NEXT_PUBLIC_WHATSAPP_PHONE || "5543991523310";

export function waLink(text) {
  const encoded = encodeURIComponent(text);
  return `https://wa.me/${WHATSAPP_PHONE}?text=${encoded}`;
}

export function fmtDateBR(dateStr) {
  // dateStr: YYYY-MM-DD
  const [y,m,d] = dateStr.split("-").map(Number);
  const dt = new Date(y, m-1, d);
  return dt.toLocaleDateString("pt-BR");
}

export function fmtTimeFromIso(iso) {
  const dt = new Date(iso);
  return dt.toLocaleTimeString("pt-BR", { hour: "2-digit", minute: "2-digit" });
}
