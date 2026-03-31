# 🏦 Sistema de Onboarding de Contas - Digital Bank

Este projeto é uma **Web API robusta desenvolvida em .NET 8**, focada no gerenciamento de contas bancárias (Onboarding).  
A arquitetura foi desenhada para suportar **alta escalabilidade**, **baixo acoplamento entre áreas do banco** e **otimização de custos de infraestrutura em nuvem (AWS)**.

---

## 🚀 Destaques da Solução

### 1. 🔗 Desacoplamento entre Áreas (Event-Driven Architecture)

Para atender ao requisito de que diversas áreas (Fraude, Cartões, Seguros) precisam ser notificadas sobre mudanças nas contas, implementamos **Domain Events** processados via **MediatR**.

- Quando uma conta é criada, o `AccountHandler` publica um `AccountCreatedEvent`
- Handlers independentes (`FraudDetectionHandler`, `CardServiceHandler`) reagem a esses eventos


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

## Solid
📐 Aplicando o S.O.L.I.D.
  - S - Single Responsibility Principle (Princípio da Responsabilidade Única)* - A regra de negócio de "Cartões" não polui o código de "Onboarding".
    
  - O - Open/Closed Principle (Princípio Aberto/Fechado)
    - No MediatR: Se amanhã o banco decidir que a área de "Seguros" também precisa saber sobre novas contas, você não altera o AccountHandler. Você apenas cria um novo InsuranceHandler que "escuta" o mesmo evento. O sistema está fechado para modificação, mas aberto para extensão.

    - Validation Behavior: Adicionamos validação a todos os comandos sem mexer em um único Handler.

  - L - Liskov Substitution Principle (Substituição de Liskov)
    - Interfaces de Repositório: O seu AccountRepository implementa IAccountRepository. Se em um teste unitário você trocar o repositório real por um Mock, o sistema continua funcionando perfeitamente. O comportamento esperado da interface é respeitado por qualquer implementação.

  - I - Interface Segregation Principle (Segregação de Interface)
    - Interfaces Específicas: Em vez de uma interface gigante IBankService que faz tudo, temos IAccountRepository e ICacheService. Cada classe só depende do que realmente usa. Seu Handler não sabe nada sobre "Conexão com Banco", ele só conhece a interface de persistência.

  - D - Dependency Inversion Principle (Inversão de Dependência)
    - Injeção de Dependência: O seu AccountsController não faz new AccountRepository(). Ele recebe IMediator pelo construtor. O AccountHandler recebe IAccountRepository.

Conceito chave: O seu código de alto nível (Aplicação) não depende de detalhes de baixo nível (MySQL). Ambos dependem de abstrações (Interfaces). Isso permitiu, por exemplo, trocar o Redis por um Cache em Memória no Program.cs sem mudar uma linha de código no Handler.

## 📐 Arquitetura e Padrões

A solução segue os princípios de:

- **Clean Architecture**
- **DDD (Domain-Driven Design)**
  -Entidades de Domínio: A classe Account não é apenas um "balde de dados" (Anêmico). Ela possui lógica própria, como o construtor que garante um ID e o estado inicial ativo.

  - Repositórios (Interfaces): Definimos IAccountRepository no Domínio. Isso diz ao sistema o que ele precisa fazer com os dados, sem se importar como (se é MySQL, MongoDB ou uma lista em memória).

  - Camada de Aplicação: Onde estão os Handlers e Commands. Eles orquestram a lógica: validam a entrada, chamam o repositório e disparam eventos. É aqui que o "processo de negócio" acontece.

  - Eventos de Domínio: O uso de AccountCreatedEvent reflete o conceito de Linguagem Ubíqua. Quando o pessoal de "Fraude" ou "Cartões" diz "precisamos saber quando uma conta é criada", o código reflete exatamente esse evento.
  
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

## 🗄️ Configurar o banco
- Rodar Migrations

```bash
dotnet ef database update
```

- Ou se preferir rode esse script no banco 
```
-- 1. Criação do Banco de Dados
CREATE DATABASE IF NOT EXISTS KrtBankDb;
USE KrtBankDb;

-- 2. Criação da Tabela Account
CREATE TABLE IF NOT EXISTS Accounts (
    Id CHAR(36) NOT NULL,
    OwnerName VARCHAR(150) NOT NULL,
    Document VARCHAR(14) NOT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    -- Índice Único: Evita CPFs duplicados e acelera buscas (Economia de CPU na AWS)
    UNIQUE INDEX IX_Accounts_Document (Document)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

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
