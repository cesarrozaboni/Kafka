namespace Kafka.Producer.API
{
    /// <summary>
    /// Model para receber os dados que serão persistidos
    /// </summary>
    public class PessoaModel
    {
        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string? Nome {get; set;}
        /// <summary>
        /// Idade da pessoa
        /// </summary>
        public int Idade {get; set;}
    }
}