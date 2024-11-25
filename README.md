# Ganho de Capital

Este é um projeto de cálculo de imposto sobre o ganho de capital de operações de compra e venda de ações no mercado financeiro. O sistema calcula o imposto a ser pago com base nas operações de venda e compra de ações, levando em consideração a dedução de prejuízos e a isenção de impostos para operações abaixo de R$ 20.000.

## Funcionalidades

- **Compra (Buy)**: Ao comprar ações, não há imposto a ser pago.
- **Venda (Sell)**: O imposto é calculado com base no lucro da venda, seguindo as regras:
  - Se o valor total da operação for inferior ou igual a R$ 20.000, não há imposto.
  - Caso contrário, o imposto é de 20% sobre o lucro, considerando prejuízos acumulados das vendas anteriores.
  - O prejuízo de vendas anteriores pode ser deduzido de lucros futuros até que o prejuízo seja totalmente absorvido.

## Requisitos

- .NET Core 8 ou superior

## Instalação

1. **Abra a solução no Visual Studio ou no editor de sua escolha e instale as dependências**:
   ```bash

   dotnet restore


2. **Compilar o projeto:**:
   ```bash

   dotnet build

3. **Para executar o aplicativo, execute o seguinte comnando no terminal:**
   ```bash

   dotnet run

Já existe um arquivo input.txt na pasta bin com algumas entradas para o programa gere a saída esperada. Caso queira editar a entrada do programa, basta acessar o diretório bin e alterar o arquivo input.txt.

# Como Testar

O projeto inclui testes unitários usando o xUnit. Os testes unitários foram produzidos de acordo com as especificações propostas no desafio. Para executar os testes, use o seguinte comando:
   ```bash

   dotnet test
```
# Testes e Estrutura do Projeto

## Testes

Os testes cobrem uma variedade de cenários, incluindo:

- Cálculo de imposto em operações de venda com lucro.
- Dedução de prejuízo de vendas anteriores.
- Isenção de imposto para vendas abaixo de R$ 20.000.

## Estrutura do Projeto

- **CapitalGains.Models**: Contém as classes e enums que representam as operações de compra e venda.
- **CapitalGains.Services**: Contém a lógica de cálculo do imposto, incluindo o cálculo do lucro, prejuízo e dedução.
- **CapitalGains.Tests**: Contém os testes unitários que validam a funcionalidade do sistema, todos produzidos com base nas especificações do desafio.
- **CapitalGains.Helper**: Contém utilitários auxiliares, como a classe `DecimalConverter`, que garante a precisão na serialização de valores decimais.

## Pasta Helper

A pasta **Helper** contém a classe `DecimalConverter`, que é usada para personalizar a serialização e deserialização de valores do tipo decimal. Esta classe garante que os valores decimais sejam representados com duas casas decimais ao serem convertidos para JSON, importante para as operações financeiras.
