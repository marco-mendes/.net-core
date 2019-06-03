#### Exercícios Práticos sobre Orientação por Objeto - Parte 3

Utilizando o código desenvolvido nas partes 1 e 2 dos exercícios práticos sobre Orientação por Objeto e conectando aos aprendizados anteriores sobre Testes, vamos implementar algumas verificações basesadas em casos de testes.

Para isso, estando na pasta da solução criada, basta executar o comando:

```
dotnet new xunit -o testes
```

> Mais informações podem ser encontradas no tutorial do xUnit, [aqui](../../NET Core 2.2/Laboratório/xUnit.md).
>
> Neste laboratório, serão especificadas uma série de casos de testes que devem ser implementadas utilizando os conceitos anteriormente vistos.



##### Ações e Exceções esperadas

> Para mais informações sobre exceções personalizadas, consulte [Criar e Lançar Exceções](https://docs.microsoft.com/pt-br/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions)

| Ação                                                  | Exceções Esperadas                                           | Razão                           |
| ----------------------------------------------------- | ------------------------------------------------------------ | ------------------------------- |
| Salário negativo para Funcionário.                    | Geração de erro demonstrando que funcionário não pode ter salário negativo. | Questões trabalhistas           |
| Funcionário sem Identificador                         | Geração de erro demonstrando que funcionário exige um Identificador, seja CPF ou RG. | Questões trabalhistas           |
| Adicionar funcionários a uma Filial sem Identificador | Geração de erro demonstrando que filial sem identificador não pode ter funcionários | Questões trabalhistas           |
| Produtos Vendidos negativo para Funcionários          | Geração de erro demonstrando que funcionário não pode ter produtos vendidos negativos. | Evitar erro em cálculo de Bônus |



##### Ações e resultados esperados

| Ação                                                      | Resultado Esperado                                           |
| --------------------------------------------------------- | ------------------------------------------------------------ |
| Criar um funcionário                                      | Deve ser retornado um objeto Funcionário com salário maior ou igual a 0, bônus maior ou igual a 0 e produtos vendidos maior ou igual a 0. |
| Criar uma filial - funcionários                           | Por padrão, as filiais devem ser criadas sem funcionários, inicialmente retornando uma coleção vazia quando funcionários é acessado. |
| Criar um cliente                                          | Deve ser retornado um objeto Cliente, contendo obrigatoriamente um nome, uma coleção que pode ser vazia de endereços e um identificador que pode ser nulo. |
| Endereços de Cliente                                      | Os endereços do cliente devem retornar uma coleção, mesmo que haja somente um. |
| Propriedade de custo de filial                            | Retorno de um valor representando todos os salários e bônus de todos os funcionários relacionados àquela filial. |
| Pesquisar funcionário por Nome ou Identificador em Filial | O retorno deve ser uma coleção com todos os funcionários encontrados, tendo um elemento quanto encontrado um e vazia se nenhum funcionário for encontrado. |

