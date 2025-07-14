namespace Microondas_API.Models
{
    public class ProgamaAquecimento
    {
        public class ProgramaAquecimento
        {
            public string Nome { get; set; }
            public string Alimento { get; set; }
            public int TempoEmSegundos { get; set; }
            public int Potencia { get; set; }
            public char Caractere { get; set; }
            public string Instrucoes { get; set; }
            public bool PreDefinido { get; set; } = false;
        }

    }
}
