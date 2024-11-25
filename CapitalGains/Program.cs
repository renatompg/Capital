using System.Text.Json;
using CapitalGains.Helper;
using CapitalGains.Services;

// Inicializa os serviços necessários para carregar e processar as operações
var operationFileService = new OperationFileService();
var capitalGainsService = new CapitalGainsService();

// Configura as opções para a serialização JSON, incluindo a conversão de valores decimais
var options = new JsonSerializerOptions
{
    WriteIndented = false,  // Configura a saída do JSON para não ser indentada
    Converters = { new DecimalConverter() }  // Adiciona um conversor customizado para valores decimais
};

// Obtém o diretório raiz da aplicação, subindo três níveis de diretórios para alcançar o diretório correto
string rootDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

// Define o caminho do arquivo de entrada onde as operações estão armazenadas
string filePath = Path.Combine(rootDirectory, "input.txt");

// Carrega as operações do arquivo de entrada de forma assíncrona
var operations = await operationFileService.LoadOperationsFromFileAsync(filePath);

// Processa cada lote de operações carregadas
foreach (var operationBatch in operations)
{
    try
    {
        // Calcula os impostos para o lote de operações usando o serviço de CapitalGains
        var results = capitalGainsService.CalculateTaxes(operationBatch);
        
        // Serializa o resultado para JSON e imprime na tela
        Console.WriteLine(JsonSerializer.Serialize(results, options));
    }
    catch (Exception ex)
    {
        // Caso ocorra um erro durante o processamento, exibe uma mensagem de erro
        Console.WriteLine($"Erro ao processar as operações: {ex.Message}");
    }
}
