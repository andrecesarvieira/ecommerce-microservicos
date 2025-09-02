# Documento de Arquitetura

## 1. Visão Geral

O sistema será desenvolvido com **arquitetura de microserviços**, utilizando comunicação **RESTful** entre clientes e API Gateway, e **RabbitMQ** para mensagens assíncronas entre os microserviços.

---

## 2. Diagrama de Arquitetura

```plaintext
        [Cliente / Frontend]
                  |
             [API Gateway]
           /               \
   [MS Estoque]         [MS Vendas]
        |                     |
 [Banco Estoque]        [Banco Vendas]
        \_______________________/
           Comunicação via RabbitMQ