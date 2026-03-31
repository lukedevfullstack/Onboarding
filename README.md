# 🏦 Sistema de Onboarding de Contas - Digital Bank

Este projeto é uma **Web API robusta desenvolvida em .NET 8**, focada no gerenciamento de contas bancárias (Onboarding).  
A arquitetura foi desenhada para suportar **alta escalabilidade**, **baixo acoplamento entre áreas do banco** e **otimização de custos de infraestrutura em nuvem (AWS)**.

---

## 🚀 Destaques da Solução

### 1. 🔗 Desacoplamento entre Áreas (Event-Driven Architecture)

Para atender ao requisito de que diversas áreas (Fraude, Cartões, Seguros) precisam ser notificadas sobre mudanças nas contas, implementamos **Domain Events** processados via **MediatR**.

- Quando uma conta é criada, o `AccountHandler` publica um `AccountCreatedEvent`
- Handlers independentes (`FraudDetectionHandler`, `CardServiceHandler`) reagem a esses eventos

**Benefício:**  
A regra de negócio de "Cartões" não polui o código de "Onboarding", respeitando o **Single Responsibility Principle (SRP)**.
Respeito total ao **Open/Closed Principle (SOLID)**. Novas áreas podem ser adicionadas ao banco sem alterar uma única linha de código do cadastro de contas.
---

### 2. 💰 Otimização de Custos AWS (Estratégia de Cache)

Para evitar cobranças excessivas de leitura no banco de dados (**MySQL/RDS**) por consultas repetidas:

- Implementado **Cache de Segundo Nível** com `IDistributedCache`
- Estratégia:
  - Consulta → verifica cache primeiro
  - Cache miss → busca no banco → armazena com TTL

#### 🔄 Invalidação de Cache
- Atualização ou exclusão de conta → invalidação imediata
- Garante consistência dos dados

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** .NET 8 (C#)
- **Banco de Dados:** MySQL (EF Core + Pomelo)
- **Mensageria Interna:** MediatR (CQRS + Events)
- **Validação:** FluentValidation (Pipeline Behaviors)
- **Tratamento de Erros:** Global Exception Handling (Middleware com `IExceptionHandler`)
- **Testes:** xUnit, Moq, FluentAssertions

---

## 📐 Arquitetura e Padrões

A solução segue os princípios de:

- **Clean Architecture**
- **DDD (Domain-Driven Design)**

### 📂 Camadas

- **Domain**
  - Entidades
  - Interfaces
  - Eventos de domínio
  - Regras de negócio puras

- **Application**
  - Commands & Queries (CQRS)
  - Handlers
  - Validators

- **Infrastructure**
  - Repositórios
  - DbContext
  - Cache (Redis / Distributed Cache)

- **API**
  - Controllers
  - Middlewares

---

## 🧪 Testes Unitários

Os testes cobrem:

- ✅ Cenários de sucesso
- ❌ CPF duplicado
- ❌ Dados inválidos
- ❌ Regras de negócio

### ▶️ Executar testes

```bash
dotnet test
```

## ⚙️ Como Executar o Projeto
📌 Pré-requisitos
- .NET 8 SDK
- MySQL Server ou Docker

## 🔧 Configurar o Banco de Dados

No appsettings.json:

```bash
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=onboarding_db;user=root;password=123456"
}
```
## 🗄️ Rodar Migrations

```bash
dotnet ef database update
```

## ▶️ Executar a API

```bash
dotnet run
```
## 📚 Acessar Swagger
```bash
https://localhost:{porta}/swagger
```

## 📝 Decisões Técnicas de Destaque
⚡ Fail-Fast Validation

-- Uso de Pipeline Behaviors do MediatR para validar comandos antes de chegarem ao Handler.

✔ Evita processamento desnecessário
✔ Melhora performance

🛑 Global Exception Handling

## Middleware centralizado que:

Intercepta exceções
Retorna padrão ProblemDetails (RFC 7807)

✔ Evita erros genéricos 500
✔ Padroniza respostas da API

## 🔒 Imutabilidade

Uso de records para Commands e Events:

✔ Garante integridade dos dados
✔ Evita efeitos colaterais