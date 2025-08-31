
# E-commerce Microserviços

Projeto de e-commerce utilizando arquitetura de microserviços, mensageria com RabbitMQ, API Gateway (Ocelot), autenticação JWT e bancos de dados isolados.

## Visão Geral

- **Microserviços:** Auth, Estoque, Vendas
- **API Gateway:** Centraliza o acesso e roteamento (Ocelot)
- **Mensageria:** RabbitMQ para eventos assíncronos entre serviços
- **Bancos:** SQL Server separados por domínio
- **Segurança:** JWT para autenticação e autorização
- **Logs:** Logs estruturados e tratamento global de exceções em todos os serviços
- **Testes:** Arquivos .http para simular requisições

## Arquitetura

- Comunicação RESTful entre clientes e API Gateway.
- Comunicação assíncrona entre microserviços via RabbitMQ.
- Cada microserviço possui seu próprio banco de dados.
- Toda chamada passa pelo API Gateway.

```
[Cliente] -> [API Gateway] -> [Auth.API | Estoque.API | Vendas.API]
                               |                |
                        [Banco Estoque]   [Banco Vendas]
                               \\_________/
                             RabbitMQ
```

## Estrutura de Pastas

- `Auth.API/` - Serviço de autenticação e registro de usuários
- `Estoque.API/` - Gerenciamento de produtos e estoque
- `Vendas.API/` - Gerenciamento de pedidos e integração com estoque
- `Gateway.API/` - API Gateway (Ocelot)
- `Docs/` - Documentação de arquitetura, requisitos e mensageria
- `Tests/` - Arquivos .http para testar endpoints das APIs


## Execução Local

1. **Suba bancos e RabbitMQ com Docker Compose:**
  ```sh
  docker-compose up -d
  ```
2. **Crie as migrations e os bancos de dados dentro dos containers Docker:**
  - Gere as migrations do Entity Framework para cada microserviço:
  
  ```sh
  # Auth.API
  dotnet ef migrations add InitialCreate --project Auth.API/Auth.API.csproj --startup-project Auth.API/Auth.API.csproj

  # Estoque.API
  dotnet ef migrations add InitialCreate --project Estoque.API/Estoque.API.csproj --startup-project Estoque.API/Estoque.API.csproj

  # Vendas.API
  dotnet ef migrations add InitialCreate --project Vendas.API/Vendas.API.csproj --startup-project Vendas.API/Vendas.API.csproj
  ```
  - Em seguida, aplique as migrations para criar os bancos e tabelas:

  ```sh
  # Auth.API
  dotnet ef database update --project Auth.API/Auth.API.csproj --startup-project Auth.API/Auth.API.csproj

  # Estoque.API
  dotnet ef database update --project Estoque.API/Estoque.API.csproj --startup-project Estoque.API/Estoque.API.csproj

  # Vendas.API
  dotnet ef database update --project Vendas.API/Vendas.API.csproj --startup-project Vendas.API/Vendas.API.csproj
  ```
  - Isso garante que os bancos (UsuariosDb, EstoqueDb, VendasDb) e as tabelas sejam criados nos containers SQL Server do Docker.

3. **Rode cada microserviço individualmente:**
  - `dotnet watch run` em cada pasta de API (ou use o script executarAPIs.ps1)

4. **Acesse o RabbitMQ:** [http://localhost:15672](http://localhost:15672) (guest/guest)

5. **Testes:** Use os arquivos em `Tests/` para simular login, registro, pedidos, etc.

## Principais Endpoints (via Gateway)

- **Auth**
  - `POST /api/auth/login` - Login e obtenção de token JWT
  - `POST /api/auth` - Registro de usuário (Administrador)
  - `GET /api/auth/?pagina=1` - Listar usuários (Administrador)
  - `DELETE /api/auth/{email}` - Excluir usuário (Administrador)
- **Produtos**
  - `GET /api/produtos` - Listar produtos (Estoque)
  - `GET /api/produtos/{id}` - Detalhe do produto (Estoque e Vendas)
  - `POST /api/produtos` - Incluir produto (Estoque)
  - `PUT /api/produtos/{id}` - Atualizar produto (Estoque)
  - `DELETE /api/produtos/{id}` - Remover produto (Estoque)
- **Pedidos**
  - `GET /api/pedidos` - Listar pedidos (Vendas)
  - `GET /api/pedidos/{id}` - Detalhe do pedido (Vendas)
  - `POST /api/pedidos` - Criar pedido (Vendas)
  - `DELETE /api/pedidos/{id}` - Cancelar pedido (Vendas)

## Mensageria (RabbitMQ)

- **Filas:**
  - `pedidos-criados`: Vendas.API publica ao criar pedido, Estoque.API consome e debita estoque.
  - `pedidos-cancelados`: Vendas.API publica ao cancelar pedido, Estoque.API consome e devolve estoque.
- **Configuração local:** Veja exemplo de serviço RabbitMQ no `docker-compose.yml`.
- **Painel:** [http://localhost:15672](http://localhost:15672) (guest/guest)


## Segurança e Autorização

- **Usuário e senha padrão do admin:**
  - **Usuário:** admin@ecommerce.com
  - **Senha:** admin
- **JWT:** Todos os endpoints sensíveis exigem autenticação JWT.
- **Roles:** Controle de acesso por roles (`Administrador`, `Estoque`, `Vendas`).
- **Obtenção do token:** Via endpoint `/api/auth/login`.

## Logs e Monitoramento

- Logs estruturados com `ILogger` em todos os serviços.
- Middleware global de tratamento de exceções.
- Apenas exceções reais são logadas (sem poluição de logs de fluxo normal).

## Tecnologias

- .NET Core (C#)
- Entity Framework Core
- SQL Server
- RabbitMQ
- JWT
- Ocelot (API Gateway)
- Docker

## Documentação

- [Arquitetura](docs/arquitetura.md)
- [Requisitos](docs/requisitos.md)
- [Mensageria (RabbitMQ)](docs/mensageria.md)

## Observações

- O Gateway (Ocelot) só expõe rotas configuradas no arquivo `Gateway.API/ocelot.json`.
- Para acessar endpoints protegidos, obtenha um token JWT via login e use nos headers das requisições.