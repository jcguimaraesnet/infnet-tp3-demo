using System.ComponentModel;

namespace profjulioguimaraes.Models
{
    public class Amigo
    {
        public int Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
        public int QuantidadeFilhos { get; set; }
        public bool PossuiParentesco { get; set; }
        [DisplayName("Foto")]
        public string ImagemUrl { get; set; }
        public DateTime? UltimaVisualizacao { get; set; }
    }
}
