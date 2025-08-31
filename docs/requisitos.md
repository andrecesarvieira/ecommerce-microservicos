
# Documento de Requisitos

## 1. Objetivo do Sistema

Desenvolver uma aplicação baseada em **arquitetura de microserviços** para gerenciar **estoque de produtos** e **vendas** em uma plataforma de e-commerce.
O sistema deve ser **escalável, seguro e robusto**, simulando um ambiente real de alta demanda.

---

## 2. Escopo

### 2.1 Funcionalidades Obrigatórias

**Microserviço de Estoque**
- Cadastro de produtos: nome, descrição, preço e quantidade.
- Consulta do catálogo de produtos.
- Atualização automática do estoque após venda.

**Microserviço de Vendas**
- Criação de pedidos com validação de estoque.
- Consulta de status de pedidos.
- Comunicação com serviço de estoque via **RabbitMQ**.

**API Gateway**
- Ponto único de entrada para clientes.
- Roteamento para os microserviços corretos.

**Segurança**
- Autenticação e autorização via **JWT**.

---

### 2.2 Funcionalidades Extras
- Testes unitários e integração.
- Logs centralizados.
- Monitoramento básico.
- Preparação para escalabilidade horizontal.

---

## 3. Requisitos Funcionais

| **ID** | **Requisito**                 | **Descrição**                                   |
|--------|-------------------------------|-------------------------------------------------|
| RF01   | Cadastro de produtos          | Permitir cadastrar produtos no estoque.         |
| RF02   | Consulta de catálogo          | Listar produtos e suas quantidades.             |
| RF03   | Criação de pedidos            | Criar pedidos validando estoque disponível.     |
| RF04   | Atualização de estoque        | Reduzir estoque automaticamente após vendas.    |
| RF05   | Autenticação                  | Somente usuários autenticados acessam endpoints.|

---

## 4. Requisitos Não Funcionais

| **ID** | **Requisito**      | **Descrição**                                    |
|--------|--------------------|--------------------------------------------------|
| RNF01  | Performance        | Resposta máxima por requisição: **300ms**.       |
| RNF02  | Escalabilidade     | Suporte a aumento de tráfego com microserviços.  |
| RNF03  | Segurança          | Uso de JWT para autenticação e autorização.       |
| RNF04  | Confiabilidade     | Garantir consistência de dados com RabbitMQ.      |
| RNF05  | Manutenibilidade   | Código modular com separação clara de camadas.    |

---

## 5. Premissas
- Cada microserviço terá **seu próprio banco de dados**.
- Comunicação entre serviços via **RabbitMQ**.
- Toda chamada passará pelo **API Gateway**.
- Autenticação obrigatória para endpoints sensíveis.

---

## 6. Critérios de Aceitação
- O sistema deve permitir cadastrar produtos com todos os campos obrigatórios.
- Não deve ser possível criar pedidos com estoque indisponível.
- O estoque deve ser atualizado automaticamente.
- A autenticação JWT deve ser validada em todos os endpoints privados.

---

## 7. Tecnologias
- **.NET Core (C#)** para os microserviços.
- **Entity Framework Core** como ORM.
- **SQL Server** como banco de dados.
- **RabbitMQ** para mensageria.
- **JWT** para autenticação.
- **API Gateway**: Ocelot (ou YARP).
- **Docker** para orquestração.