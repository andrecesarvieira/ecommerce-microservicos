# Documentação de Mensageria (RabbitMQ)

## Visão Geral
Este projeto utiliza o RabbitMQ como broker de mensagens para comunicação assíncrona entre os microserviços (ex: Vendas.API e Estoque.API).

## Acesso ao RabbitMQ
- **Painel de administração:** [http://localhost:15672](http://localhost:15672)
- **Usuário padrão:** guest
- **Senha padrão:** guest

## Filas Utilizadas
- **pedidos-criados**: Publicada pela Vendas.API ao criar um pedido. Consumida pela Estoque.API para debitar o estoque.
- **pedidos-cancelados**: Publicada pela Vendas.API ao cancelar um pedido. Consumida pela Estoque.API para devolver o estoque.

## Configuração Local
O RabbitMQ pode ser executado via Docker Compose. Exemplo de serviço no `docker-compose.yml`:

```yaml
rabbitmq:
  image: rabbitmq:3-management
  ports:
    - "5672:5672"   # Porta para comunicação das aplicações
    - "15672:15672" # Porta do painel web
```

## Fluxo de Mensagens
1. **Pedido criado:**
   - Vendas.API publica mensagem na fila `pedidos-criados`.
   - Estoque.API consome e debita o estoque.
2. **Pedido cancelado:**
   - Vendas.API publica mensagem na fila `pedidos-cancelados`.
   - Estoque.API consome e devolve o estoque.

## Troubleshooting
- Se não conseguir acessar o painel, verifique se o container está rodando e se as portas estão corretas.
- Usuário/senha padrão só funcionam localmente. Em produção, altere as credenciais.
- Verifique logs das APIs para mensagens de erro de conexão.

---

Dúvidas ou problemas? Consulte a documentação oficial do RabbitMQ: https://www.rabbitmq.com/documentation.html
