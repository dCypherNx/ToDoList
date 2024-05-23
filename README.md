# ToDoList

Este README fornece uma visão geral do teste solicitado, incluindo os requisitos do teste, os itens relevantes abordados no código e uma breve descrição de como cada um deles foi implementado.

## Descrição do Teste:
O teste consiste no desenvolvimento de uma aplicação composta por um frontend Angular, uma WebAPI e um Service Worker em C#, utilizando .NET Core 6. Os métodos de leitura de dados da WebAPI se comunicam diretamente com um banco de dados SQL, enquanto os métodos de criação, atualização e exclusão se comunicam com o Service Worker via RabbitMQ.

## Itens Relevantes

1) **Log para Suporte da Aplicação**<br>
Para o suporte da aplicação, foi utilizado o ILogger do .NET, fornecendo uma abordagem robusta e flexível de logs. A implementação permite a captura de informações importantes para análise e monitoramento da aplicação. Para uma análise mais aprofundada dos logs, poderiamos considerar uma ferramenta como Datadog que afim de centralizar e otimizar o monitoramento e análise dos mesmos.<br><br>
2) **Aplicação de Design Patterns**<br>
Foram aplicados os princípios SOLID para melhorar a manutenção e extensibilidade do código. O padrão de DDD (Domain Driven Design) foi utilizado para alinhar o design do software com as regras de negócio, facilitando a comunicação e compreensão do código. Foram seguidas as práticas de Clean Code para garantir um código claro e de alta qualidade, simplificando a sustentação e o trabalho diário dos desenvolvedores.<br><br>
3) **Utilização de Framework de Persistência**<br>
O Entity Framework foi escolhido como o framework de persistência devido à sua integração nativa com o .NET Core e sua capacidade de abstrair a complexidade do acesso ao banco de dados. Dessa forma o foco do desenvolvimento é direcionado principalmente para as regras de negócio, simplificando a implementação e manutenção do código.<br><br>
4) **Banco de Dados Livre**<br>
O banco de dados escolhido para o teste foi o MSSQL, devido à sua robustez, segurança, desempenho e integração total com o .NET. Embora seja uma escolha sólida, a aplicação foi desenvolvida de forma a permitir a substituição do banco de dados por outras opções, caso necessário.  
