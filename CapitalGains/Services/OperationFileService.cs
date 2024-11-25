using System.Text.Json;
using CapitalGains.Models;

namespace CapitalGains.Services
{
    public class OperationFileService
    {
        /// <summary>
        /// Lê e desserializa as operações de um arquivo.
        /// </summary>
        /// <param name="filePath">O caminho do arquivo.</param>
        /// <returns>Uma lista de listas de operações.</returns>
        public async Task<List<List<Operation>>> LoadOperationsFromFileAsync(string filePath)
        {
            // Verifica se o arquivo existe
            if (!File.Exists(filePath))
            {
                // Lança uma exceção caso o arquivo não seja encontrado
                throw new FileNotFoundException($"O arquivo {filePath} não foi encontrado.");
            }

            // Lê o conteúdo do arquivo de forma assíncrona
            var fileContent = await File.ReadAllTextAsync(filePath);
            return ParseOperations(fileContent);
        }

        /// <summary>
        /// Desserializa o conteúdo do arquivo para uma estrutura de operações.
        /// </summary>
        /// <param name="fileContent">O conteúdo do arquivo como string.</param>
        /// <returns>Uma lista de listas de operações.</returns>
        private List<List<Operation>> ParseOperations(string fileContent)
        {
            // Sanitiza o conteúdo do arquivo para remover espaços e quebras de linha
            var sanitizedContent = fileContent.Replace(" ", "").Replace("\r", "").Replace("\n", "");
            var jsonArrays = sanitizedContent.Split("[", StringSplitOptions.RemoveEmptyEntries);

            var operations = new List<List<Operation>>();

            // Processa cada bloco de operações no conteúdo do arquivo
            foreach (var jsonArray in jsonArrays)
            {
                try
                {
                    // Adiciona o caractere "[" ao início para formar um array JSON válido
                    var operationList = JsonSerializer.Deserialize<List<Operation>>(jsonArray.Insert(0, "["));
                    if (operationList != null)
                    {
                        operations.Add(operationList);
                    }
                }
                catch (JsonException ex)
                {
                    // Exibe uma mensagem de erro caso ocorra um problema ao desserializar um bloco
                    Console.WriteLine($"Erro ao desserializar um bloco de operações: {ex.Message}");
                }
            }

            return operations;
        }
    }
}
