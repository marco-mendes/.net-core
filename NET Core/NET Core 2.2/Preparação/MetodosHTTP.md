# Métodos HTTP

O protocolo HTTP define um conjunto de **métodos de requisição** responsáveis por indicar a ação a ser executada para um dado recurso. Embora esses métodos possam ser descritos como substantivos, eles também são comumente referenciados como **HTTP Verbs (Verbos HTTP)**. Cada um deles implementa uma semântica diferente, mas alguns recursos são compartilhados por um grupo deles, como por exemplo, qualquer método de requisição pode ser do tipo [safe](https://developer.mozilla.org/pt-BR/docs/Glossary/safe), [idempotent](https://developer.mozilla.org/pt-BR/docs/Glossary/idempotent) ou [cacheable](https://developer.mozilla.org/pt-BR/docs/Glossary/cacheable).

- `GET`

  O método `GET` solicita a representação de um recurso específico. Requisições utilizando o método `GET` devem retornar apenas dados.

- `HEAD`

   O método `HEAD` solicita uma resposta de forma idêntica ao método `GET`, porém sem conter o corpo da resposta.

- `POST`

  O método `POST` é utilizado para submeter uma entidade a um recurso específico, frequentemente causando uma mudança no estado do recurso ou efeitos colaterais no servidor.

- `PUT`

  O método `PUT` substitui todas as atuais representações do recurso de destino pela carga de dados da requisição.

- `DELETE`

  O método `DELETE` remove um recurso específico.

- `CONNECT`

  O método `CONNECT` estabelece um túnel para o servidor identificado pelo recurso de destino.

- `OPTIONS`

  O método `OPTIONS` é usado para descrever as opções de comunicação com o recurso de destino.

- `TRACE`

  O método `TRACE` executa um teste de chamada *loop-back* junto com o caminho para o recurso de destino.

- `PATCH`

  O método `PATCH` é utilizado para aplicar modificações parciais em um recurso.



[Mozilla](https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Methods)

Especificações formais:

- https://tools.ietf.org/html/rfc7231#section-4
- https://tools.ietf.org/html/rfc5789#section-2

