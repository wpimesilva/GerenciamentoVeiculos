# GerenciamentoVeiculos.Api

API REST desenvolvida em **.NET 8** para gerenciamento de **Proprietários** e **Veículos**, com foco em boas práticas de arquitetura, organização em camadas e preparação para cenários reais de produção.



## Objetivo

A aplicação permite:

- CRUD completo de **Proprietários**
- CRUD completo de **Veículos**
- Relacionamento entre proprietário e veículo
- Validações de entrada e regras de negócio
- Busca de veículos por:
  - placa
  - modelo
  - nome do proprietário
- Exclusão lógica de registros
- Estrutura preparada para mensageria (Azure Service Bus)
- Base pronta para autentação via JWT



## Decisões Técnicas

### Arquitetura em Camadas

A aplicação foi organizada para separar responsabilidades:

- **Controllers** → entrada e saída HTTP  
- **Services** → regras de negócio  
- **Repositories** → acesso a dados  
- **DTOs** → contrato da API  
- **Models** → entidades do domínio  
- **Middleware** → tratamento global de erros  
- **Messaging** → comunicação assíncrona  
- **Workers** → processamento em background  
 Essa separação reduz acoplamento e facilita manutenção, testes e evolução.


##  Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (InMemory)
- Swagger / OpenAPI
- Injeção de Dependência nativa
- BackgroundService
- Azure Service Bus (estrutura preparada)
- JWT Bearer Authentication (base pronta)



##  Estrutura do Projeto


GerenciamentoVeiculos.Api
│
├── Controllers
├── Data
├── DTOs
├── Exceptions
├── Messaging
├── Middleware
├── Models
├── Repositories
│   └── Interfaces
├── Services
│   └── Interfaces
├── Workers
├── Program.cs
└── appsettings.json


## Regras de Negócio
	Proprietários

Nome obrigatório (3 a 100 caracteres)
CPF obrigatório e válido
Data de nascimento obrigatória

	Veículos
Placa obrigatória no formato ABC-1234
Modelo obrigatório
Ano obrigatório (> 1980)
Proprietário deve existir
Não pode haver duplicidade de placa

	Exclusão
Exclusão lógica (Ativo = false)
Proprietário não pode ser excluído se possuir veículos ativos

## Funcionalidades
	Proprietários
GET /api/proprietarios
GET /api/proprietarios/{id}
POST /api/proprietarios
PUT /api/proprietarios/{id}
DELETE /api/proprietarios/{id}

	Veículos
GET /api/veiculos
GET /api/veiculos/{id}
GET /api/veiculos/buscar?placa=&modelo=&nomeProprietario=
POST /api/veiculos
PUT /api/veiculos/{id}
DELETE /api/veiculos/{id}

## exemplos de Payload
Criar Proprietário
{
  "nome": "João Silva",
  "cpf": "12345678901",
  "dataNascimento": "1990-05-10"
}

Criar Veículo
{
  "placa": "ABC-1234",
  "modelo": "Civic",
  "ano": 2020,
  "proprietarioId": "GUID_DO_PROPRIETARIO"
}


## Como Executar
	1. Clonar o repositório
git clone https://github.com/seu-usuario/gerenciamento-veiculos-api.git
	2. Acessar o projeto
cd GerenciamentoVeiculos.Api
	3. Restaurar dependências
dotnet restore
	4. Executar a aplicação
dotnet run
	5. Acessar o Swagger
https://localhost:xxxx/swagger

## Configuração
appsettings.json
{
  "AzureServiceBus": {
    "ConnectionString": "SUA_CONNECTION_STRING_AQUI"
  },
  "Jwt": {
    "Authority": "https://seu-servidor-identidade",
    "Audience": "gerenciamento-veiculos-api"
  }
}

## Mensageria

A aplicação possui estrutura pronta para integração com Azure Service Bus:

Publisher de mensagens ao criar veículo
Worker consumidor em background
Estratégia de:
confirmação manual
retry
dead-letter queue

	Observação:

A mensageria foi mantida desabilitada no ambiente local por ausência de configuração do Azure Service Bus, mas a estrutura está pronta para uso.


## Exclusão Lógica

Foi adotado o padrão de exclusão lógica:

registros não são removidos fisicamente
campo Ativo controla visibilidade
consultas retornam apenas registros ativos

## Segurança

A API está preparada para autenticação via JWT:

base configurada com JwtBearer
pronta para integração com:
Keycloak
Auth0

## Boas Práticas Aplicadas
Separação de responsabilidades (SOLID)
Baixo acoplamento via interfaces
DTOs para controle de contrato
Middleware para tratamento global de erros
Uso de AsNoTracking para otimização de leitura
Identificadores com Guid (pensando em sistemas distribuídos)

## Melhorias Futuras
Persistência em SQL Server/PostgreSQL
Paginação de resultados
FluentValidation
Testes unitários e de integração
Observabilidade (Application Insights)
Autenticação e autorização completas
Implementação de CQRS
Idempotência no consumo de mensagens


## Modelo Relacional (Referência)
CREATE TABLE Proprietarios (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Cpf NVARCHAR(11) NOT NULL,
    DataNascimento DATETIME2 NOT NULL,
    Ativo BIT NOT NULL
);

CREATE TABLE Veiculos (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Placa NVARCHAR(8) NOT NULL,
    Modelo NVARCHAR(100) NOT NULL,
    Ano INT NOT NULL,
    ProprietarioId UNIQUEIDENTIFIER NOT NULL,
    Ativo BIT NOT NULL,
    CONSTRAINT FK_Veiculos_Proprietarios 
        FOREIGN KEY (ProprietarioId) REFERENCES Proprietarios(Id)
);

## Considerações Finais

Este projeto foi desenvolvido com foco em:

organização
clareza
escalabilidade
proximidade com cenários reais

Mesmo sendo um projeto de teste, a estrutura foi pensada para evoluir facilmente para um ambiente de produção.


## Autor

Desenvolvido por WELLINGTON PIMENTEL DA SILVA
