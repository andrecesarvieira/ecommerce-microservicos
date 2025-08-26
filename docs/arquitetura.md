\# Documento de Arquitetura



\## 1. Visão Geral

O sistema será desenvolvido com \*\*arquitetura de microserviços\*\*, utilizando comunicação \*\*RESTful\*\* entre clientes e API Gateway, e \*\*RabbitMQ\*\* para mensagens assíncronas entre os microserviços.



---



\## 2. Diagrama de Arquitetura



```plaintext

&nbsp;          \[Cliente / Frontend]

&nbsp;                   |

&nbsp;              \[API Gateway]

&nbsp;            /               \\

&nbsp;  \[MS Estoque]         \[MS Vendas]

&nbsp;       |                     |

&nbsp; \[Banco Estoque]        \[Banco Vendas]

&nbsp;       \\\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_/

&nbsp;          Comunicação via RabbitMQ



