#### Exercícios Práticos sobre Orientação a Objeto - Parte 2



3. Um belo dia, Cláudio foi disfarçado em uma de suas filiais e foi muito bem atendido por João da Silva, um funcionário. Ele gostou tanto do atendimento que resolveu enviar um brinde para casa de João. Só tem um problema, ele não sabe o endereço dele. Como Cláudio pretende visitar com frequência suas lojas e verificar o atendimento, gostaria que seu programa implementasse uma função de pesquisa de funcionário por filial, seja por Nome ou Identificador. 



4. Aproximando do final do mês, Cláudio percebeu que precisava saber quanto deveria pagar de salários/bônus em cada filial para realizar as movimentações financeiras. Embora não esteja nas especificações anteriores, para que todos os funcionários recebam salários e bônus adequados, é necessário implementar um método em cada filial que retorne o quanto de dinheiro Cláudio precisa para pagar todos os funcionários.

   > Seria interessante se cada funcionário tivesse um método que calculasse o total de custos, somando o salário e bônus para facilitar os cálculos. Por exemplo, `funcionario.Custo` retornaria já a soma.
   >
   > É possível até utilizar propriedade `get`, como:
   >
   > ```c#
   > public double Custo {
   > 	get {
   > 		return this.salario + this.bonus;
   > 	}
   > }
   > ```

