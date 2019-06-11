### Exercício de API Gateway

Nesse exercício você irá usar os conhecimentos aprendidos no tutorial anterior ([APIGateway.md](APIGateway.md)). Você irá construir os seguintes serviços autônomos:

. Cadastro de produtos (CRUD)

. Gestão de um carrinho de compra (CRUD)

. Gestão de pedidos (Pagamento de pedidos, Cancelamento de pedidos; Listagem de pedidos)

Cada serviço deve operar como um projeto independente, rodando em um IP/Porto distinto. Por exemplo, em .NET você terá três projetos distintos do tipo Web API.

Implemente e teste cada projeto de forma independente. Garanta que eles estejam operacionais antes de avançar.

---

Após a criação dos projetos, você irá criar com o apoio do Ocelot a exposição de duas APIs. Uma API pública para operações para um usuário da sua loja virtual e uma API privativa de administração com operações sensíveis (Exemplo: Cadastro de novos produtos ou listagem de todos os pedidos realizados).

Essa forma de arquitetura é uma variação do padrão API Gatewat chamado Backend for Frontends (BFF), comforme mostrado na figura abaixo.

![img](http://devblogs.microsoft.com/cesardelatorre/wp-content/uploads/sites/32/2018/05/051518_1821_Designingan2.png)

Dica: Para fazer isso, crie dois projetos Ocelot. O primeiro será usado para expor a API pública. O segundo será usado expor a API privativa. 



