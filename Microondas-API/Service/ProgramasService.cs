using System.Text.Json;
using static Microondas_API.Models.ProgamaAquecimento;

namespace Microondas_API.Service
{
    public class ProgramasService
    {
        private const string ArquivoCustomizados = @"Data/programas_customizados.json";

     
        public static List<ProgramaAquecimento> ObterPreDefinidos()
        {
            return new List<ProgramaAquecimento>
            {
                new ProgramaAquecimento
                {
                    Nome = "Pipoca",
                    Alimento = "Pipoca de micro-ondas",
                    TempoEmSegundos = 180,
                    Potencia = 7,
                    Caractere = '*',
                    Instrucoes = "Observar o barulho de estouros do milho. Caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento.",
                    PreDefinido = true
                },
                new ProgramaAquecimento
                {
                    Nome = "Leite",
                    Alimento = "Leite",
                    TempoEmSegundos = 300,
                    Potencia = 5,
                    Caractere = '~',
                    Instrucoes = "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras.",
                    PreDefinido = true
                },
                new ProgramaAquecimento
                {
                    Nome = "Carnes de boi",
                    Alimento = "Carne em pedaço ou fatias",
                    TempoEmSegundos = 840,
                    Potencia = 4,
                    Caractere = '#',
                    Instrucoes = "Interrompa o processo na metade e vire o conteúdo para descongelamento uniforme.",
                    PreDefinido = true
                },
                new ProgramaAquecimento
                {
                    Nome = "Frango",
                    Alimento = "Frango (qualquer corte)",
                    TempoEmSegundos = 480,
                    Potencia = 7,
                    Caractere = '@',
                    Instrucoes = "Interrompa o processo na metade e vire o conteúdo para descongelamento uniforme.",
                    PreDefinido = true
                },
                new ProgramaAquecimento
                {
                    Nome = "Feijão",
                    Alimento = "Feijão congelado",
                    TempoEmSegundos = 480,
                    Potencia = 9,
                    Caractere = '$',
                    Instrucoes = "Deixe o recipiente destampado e em casos de plástico, cuidado ao retirar pois pode perder resistência.",
                    PreDefinido = true
                }
            };
        }

     
        public static List<ProgramaAquecimento> CarregarCustomizados()
        {
            if (!File.Exists(ArquivoCustomizados))
                return new List<ProgramaAquecimento>();

            var json = File.ReadAllText(ArquivoCustomizados);
            return JsonSerializer.Deserialize<List<ProgramaAquecimento>>(json)
                   ?? new List<ProgramaAquecimento>();
        }

        
        public static void SalvarCustomizados(List<ProgramaAquecimento> customizados)
        {
            var pasta = Path.GetDirectoryName(ArquivoCustomizados);
            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            var json = JsonSerializer.Serialize(customizados, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ArquivoCustomizados, json);
        }

        
        public static bool CaractereValido(char caractere, List<ProgramaAquecimento> predefinidos, List<ProgramaAquecimento> customizados)
        {
            if (caractere == '.')
                return false;

            if (predefinidos.Any(p => p.Caractere == caractere))
                return false;

            if (customizados.Any(p => p.Caractere == caractere))
                return false;

            return true;
        }

        
        public static bool AdicionarCustomizado(ProgramaAquecimento novo, List<ProgramaAquecimento> predefinidos, List<ProgramaAquecimento> customizados)
        {
            if (!CaractereValido(novo.Caractere, predefinidos, customizados))
                return false;

            novo.PreDefinido = false;
            customizados.Add(novo);
            SalvarCustomizados(customizados);
            return true;
        }

       
        public static List<ProgramaAquecimento> TodosProgramas()
        {
            var predef = ObterPreDefinidos();
            var custom = CarregarCustomizados();
            return predef.Concat(custom).ToList();
        }
    }
}
