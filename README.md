## E-commerce Microserviços

Projeto de e-commerce utilizando arquitetura de microserviços, mensageria com RabbitMQ, API Gateway e bancos de dados isolados.


### Visão Geral
- Microserviços: Estoque, Vendas, Gateway
- Comunicação REST (clientes/gateway) e RabbitMQ (eventos entre serviços)
- Bancos SQL Server separados por domínio
- API Gateway centralizando o acesso
- Segurança: será implantado JWT para autenticação e autorização dos endpoints

### Estrutura
- `estoque/Estoque.API`: Gerenciamento de produtos e estoque
- `vendas/Vendas.API`: Gerenciamento de pedidos
- `gateway/Gateway.API`: API Gateway (roteamento e segurança)
- `docs/`: Documentação de arquitetura, requisitos e mensageria
- `Tests/`: Arquivos .http para testar endpoints

### Execução Local
1. Suba os serviços com Docker Compose:
	```sh
	docker-compose up -d
	```
2. Rode cada microserviço (Estoque.API, Vendas.API, Gateway.API) via Visual Studio, VS Code ou CLI.
3. Acesse o RabbitMQ em [http://localhost:15672](http://localhost:15672) (guest/guest).
4. Teste os endpoints usando os arquivos em `Tests/`.

### Documentação
- [Arquitetura](docs/arquitetura.md)
- [Requisitos](docs/requisitos.md)
- [Mensageria (RabbitMQ)](docs/mensageria.md)

---
Em desenvolvimento contínuo. Sugestões e melhorias são bem-vindas!