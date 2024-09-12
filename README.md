# CryptoInvestmentManagement

[![Build Status](https://github.com/Alansilvao/CryptoInvestmentManagement/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/Alansilvao/CryptoInvestmentManagement/actions)
[![SonarCloud Status](https://sonarcloud.io/api/project_badges/measure?project=Investiment&metric=alert_status)](http://localhost:9000/dashboard?id=Investiment)

## Visão Geral

**CryptoInvestmentManagement** é uma aplicação .NET que simula um sistema de gestão de investimentos, permitindo que usuários gerenciem seus portfólios de investimentos em ações, títulos e criptomoedas. O projeto segue os princípios de **Clean Architecture** para garantir a escalabilidade e a separação de responsabilidades.

### Funcionalidades

- Gerenciamento de portfólios de investimentos.
- Suporte a múltiplos tipos de ativos (ações, títulos e criptomoedas).
- Registro e acompanhamento de transações (compra e venda de ativos).
- Sistema de autenticação com área logada.

## Estrutura do Projeto

O projeto está estruturado seguindo **Clean Architecture**:

- `Core`: Contém as entidades principais (usuário, portfólio, ativo e transação) e as regras de negócio.
- `Infrastructure`: Implementa a camada de infraestrutura, como acesso ao banco de dados.
- `Application`: Contém a lógica da aplicação e as interfaces de caso de uso.
- `Web`: A camada de apresentação que expõe a API para os usuários.

## Tecnologias Utilizadas

- **.NET 7.x**
- **EF Core**
- **XUnit** para testes unitários
- **Coverlet** para cobertura de testes
- **SonarQube** para análise de qualidade de código

## Como Rodar o Projeto

### Pré-requisitos

Certifique-se de ter os seguintes itens instalados:

- [.NET SDK](https://dotnet.microsoft.com/download) (7.x ou superior)
- [SQL Server](https://www.microsoft.com/sql-server)
- [SonarQube](https://www.sonarqube.org/downloads/) (caso queira rodar a análise de código localmente)

### Passos

1. Clone o repositório:

    ```bash
    git clone https://github.com/Alansilvao/CryptoInvestmentManagement.git
    cd CryptoInvestmentManagement
    ```

2. Restaure as dependências:

    ```bash
    dotnet restore
    ```

3. Compile o projeto:

    ```bash
    dotnet build
    ```

4. Execute as migrações do banco de dados (se necessário):

    ```bash
    dotnet ef database update
    ```

5. Rode a aplicação:

    ```bash
    dotnet run --project Web
    ```

A aplicação estará disponível em `http://localhost:7176`.

## Rodando os Testes

Os testes unitários estão localizados no diretório `tests/UnitTests`. Para executar os testes:

```bash
dotnet test tests/UnitTests
