# E-commerce Microserviços

Projeto de e-commerce utilizando arquitetura de microserviços, mensageria com RabbitMQ, API Gateway (Ocelot), autenticação JWT e bancos de dados isolados.

## Visão Geral

- **Microserviços:** Auth, Estoque, Vendas
- **API Gateway:** Centraliza o acesso e roteamento (Ocelot)
- **Mensageria:** RabbitMQ para eventos assíncronos entre serviços
- **Bancos:** SQL Server separados por domínio
- **Segurança:** JWT para autenticação e autorização
- **Testes:** Arquivos .http para simular requisições

## Estrutura de Pastas

- `Auth.API/` - Serviço de autenticação e registro de usuários
- `Estoque.API/` - Gerenciamento de produtos e estoque
- `Vendas.API/` - Gerenciamento de pedidos e integração com estoque
- `Gateway.API/` - API Gateway (Ocelot)
- `Docs/` - Documentação de arquitetura, requisitos e mensageria
- `Tests/` - Arquivos .http para testar endpoints das APIs

## Execução Local

1. **Suba os bancos e RabbitMQ com Docker Compose:**
   ```sh
   docker-compose up -d
   ```
2. **Rode cada microserviço individualmente:**
   - `dotnet watch run` em cada pasta de API (ou use o script executarAPIs.ps1)
3. **Acesse o RabbitMQ:** [http://localhost:15672](http://localhost:15672) (guest/guest)
4. **Testes:** Use os arquivos em `Tests/` para simular login, registro, pedidos, etc.

## Principais Endpoints (via Gateway)

- **Auth**
  - `POST /api/auth/login` - Login e obtenção de token JWT
  - `POST /api/auth` - Registro de usuário (requer token de admin)
  - `GET /api/auth/?pagina=1` - Listar usuários (admin)
  - `DELETE /api/auth/{email}` - Excluir usuário (admin)
- **Produtos**
  - `GET /api/produtos` - Listar produtos (estoquista)
  - `GET /api/produtos/{id}` - Detalhe do produto
  - `POST /api/produtos` - Incluir produto
  - `PUT /api/produtos/{id}` - Atualizar produto
  - `DELETE /api/produtos/{id}` - Remover produto
- **Pedidos**
  - `GET /api/pedidos` - Listar pedidos (vendedor)
  - `GET /api/pedidos/{id}` - Detalhe do pedido
  - `POST /api/pedidos` - Criar pedido
  - `DELETE /api/pedidos/{id}` - Cancelar pedido

## Documentação

- [Arquitetura](Docs/arquitetura.md)
- [Requisitos](Docs/requisitos.md)
- [Mensageria (RabbitMQ)](Docs/mensageria.md)

## Observações

- O Gateway (Ocelot) só expõe rotas configuradas no arquivo `Gateway.API/ocelot.json`.
- Para acessar endpoints protegidos, obtenha um token JWT via login e use nos headers das requisições.
- O projeto está em desenvolvimento contínuo. Sugestões e melhorias são bem-vindas!