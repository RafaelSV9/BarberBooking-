# 💈 BarberBooking – Sistema de Agendamento para Barbearia

Sistema completo de **agendamento online para barbearia**, desenvolvido para funcionar de forma simples, rápida e escalável.

O projeto permite que clientes realizem **reservas por horário marcado** e que o administrador gerencie a agenda através de um **painel protegido por login**, com integração via **WhatsApp** para confirmação e cancelamento.

---

## ✨ Funcionalidades

### 👤 Cliente
- Escolha do barbeiro
- Agendamento por **horários fixos de 30 minutos**
- Funcionamento das **09:00 às 19:00**
- Agenda aberta para os **próximos 30 dias**
- Confirmação e cancelamento via **WhatsApp**
- Prevenção de conflitos de horário

### 🔐 Admin
- Login com autenticação JWT
- Visualização da agenda por data e barbeiro
- Confirmação e cancelamento de agendamentos
- Bloqueio de horários (almoço, folga, etc.)
- Estrutura pronta para adicionar mais barbeiros no futuro

---

## 🧱 Arquitetura

- **Front-end:** Next.js (React)
- **Back-end:** ASP.NET Core (.NET 8)
- **Banco de Dados:** PostgreSQL
- **ORM:** Entity Framework Core
- **Autenticação:** JWT
- **Integração:** WhatsApp (click-to-chat)
- **Infra local:** Docker (Postgres)

---
```
## 📂 Estrutura do Projeto

barberbooking-mvp/
│
├── BarberBooking.Api/ # API .NET 8
│ ├── Controllers/
│ ├── Domain/
│ ├── Data/
│ ├── DTOs/
│ ├── Services/
│ └── Program.cs
│
├── barberbooking-web/ # Front-end Next.js
│ ├── app/
│ ├── public/
│ └── package.json
│
├── docker-compose.yml # Postgres
└── README.md
```
---

## 🚀 Como Executar o Projeto

### 1️⃣ Subir o Banco de Dados
```bash
docker compose up -d
2️⃣ Rodar a API (.NET 8)
bash
Copiar código
cd BarberBooking.Api
cp .env.example .env
Edite o arquivo .env e configure:

env
Copiar código
ADMIN_USER=admin
ADMIN_PASSWORD=senha_forte_aqui
JWT_KEY=chave_super_secreta_com_32_chars_ou_mais
Depois:

bash
Copiar código
dotnet restore
dotnet run
Swagger disponível em:

bash
Copiar código
http://localhost:5000/swagger
3️⃣ Rodar o Front-end (Next.js)
bash

Copiar código
cd barberbooking-web
npm install
cp .env.local.example .env.local
npm run dev
Aplicação:

arduino
http://localhost:3000
```
🔑 Acesso Admin
URL: http://localhost:3000/admin/login

Usuário: admin

Senha: definida no .env

📲 Integração com WhatsApp
O sistema utiliza links automáticos (click-to-chat) para:

Confirmar agendamentos

Cancelar agendamentos

📞 Número configurado:

+55 43 99152-3310
A arquitetura já está pronta para futura integração com WhatsApp Business API (automação real).

📌 Regras de Negócio
Slots fixos de 30 minutos

Último horário inicia às 18:30

Um barbeiro não pode ter dois agendamentos no mesmo horário

Cancelamentos liberam automaticamente o horário

Timezone padrão: America/Sao_Paulo

🛠️ Possíveis Evoluções
Confirmação automática via WhatsApp API

Lembretes automáticos (24h / 2h antes)

Cadastro de serviços com duração variável

Sistema de fidelidade

Pagamento online (Pix / cartão)

Multi-admin

📄 Licença
Este projeto é de uso livre para estudos, demonstração e evolução.
Sinta-se à vontade para adaptar às suas necessidades.

💈 Desenvolvido com foco em simplicidade, organização e escalabilidade.
