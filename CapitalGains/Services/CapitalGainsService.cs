using CapitalGains.Models;

namespace CapitalGains.Services
{
    /// <summary>
    /// Serviço para calcular impostos sobre ganhos de capital.
    /// </summary>
    public class CapitalGainsService
    {
        private decimal averageCost = 0; // Custo médio das ações
        private int totalQuantity = 0; // Quantidade total de ações
        private decimal lossCarryForward = 0; // Prejuízo acumulado para compensação futura

        /// <summary>
        /// Calcula os impostos com base nas operações de compra e venda.
        /// </summary>
        /// <param name="operations">Lista de operações de compra e venda.</param>
        /// <returns>Lista de resultados fiscais para cada operação.</returns>
        public List<TaxResult> CalculateTaxes(List<Operation> operations)
        {
            var results = new List<TaxResult>();

            // Processa cada operação (compra ou venda)
            foreach (var operation in operations)
            {
                if (operation.OperationType == OperationType.Buy)
                {
                    // Chama o método de compra e adiciona o resultado com imposto 0
                    Buy(operation.UnitCost, operation.Quantity);
                    results.Add(new TaxResult { Tax = 0 });
                }
                else if (operation.OperationType == OperationType.Sell)
                {
                    // Chama o método de venda e calcula o imposto
                    var tax = Sell(operation.UnitCost, operation.Quantity);
                    results.Add(new TaxResult { Tax = tax });
                }
            }

            return results;
        }

        /// <summary>
        /// Registra uma operação de compra e atualiza o custo médio e quantidade.
        /// </summary>
        /// <param name="unitCost">O custo por unidade da ação comprada.</param>
        /// <param name="quantity">A quantidade de ações compradas.</param>
        private void Buy(decimal unitCost, int quantity)
        {
            // Atualiza o custo médio ponderado
            averageCost = ((totalQuantity * averageCost) + (quantity * unitCost)) / (totalQuantity + quantity);
            totalQuantity += quantity; // Atualiza a quantidade total de ações
        }

        /// <summary>
        /// Registra uma operação de venda e calcula o imposto devido sobre o lucro.
        /// </summary>
        /// <param name="unitCost">O custo por unidade da ação vendida.</param>
        /// <param name="quantity">A quantidade de ações vendidas.</param>
        /// <returns>O imposto devido sobre a venda.</returns>
        /// <exception cref="InvalidOperationException">Lançada quando tenta vender mais ações do que o disponível.</exception>
        private decimal Sell(decimal unitCost, int quantity)
        {
            // Verifica se a quantidade de ações vendidas é maior do que a quantidade disponível
            if (quantity > totalQuantity)
                throw new InvalidOperationException("Não é possível vender mais ações do que o disponível.");

            var totalValue = unitCost * quantity; // Valor total da venda

            // Calcula o lucro ou prejuízo da operação
            var profit = (unitCost - averageCost) * quantity;

            if (profit < 0)
            {
                // Acumula o prejuízo para compensação futura
                lossCarryForward += Math.Abs(profit);
                totalQuantity -= quantity; // Atualiza a quantidade total de ações

                // Se o valor total da venda for menor ou igual a R$ 20.000, não há imposto devido
                if (totalValue <= 20000)
                {
                    return 0;
                }

                // Se a venda ultrapassar R$ 20.000, o lucro não será tributado, mas o prejuízo foi acumulado
                return 0;
            }

            // Após acumular o prejuízo, se a venda for abaixo de R$ 20.000, não há imposto
            if (totalValue <= 20000)
            {
                return 0;
            }

            // Caso a venda seja lucrativa, verifica o prejuízo acumulado
            decimal taxableProfit = profit - lossCarryForward;

            // Se o lucro tributável for negativo, significa que o prejuízo acumulado cobre o lucro
            if (taxableProfit < 0)
            {
                lossCarryForward = Math.Abs(taxableProfit); // O prejuízo restante é mantido
                taxableProfit = 0; // O lucro tributável é zerado
            }
            else
            {
                lossCarryForward = 0; // O prejuízo foi totalmente utilizado
            }

            totalQuantity -= quantity; // Atualiza a quantidade total de ações

            // Calcula o imposto sobre o lucro tributável (20% do lucro)
            return taxableProfit > 0 ? Math.Round(taxableProfit * 0.2m, 2) : 0;
        }
    }
}
