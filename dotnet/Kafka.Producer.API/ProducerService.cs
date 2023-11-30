using System.Text.Json;
using Confluent.Kafka;

namespace Kafka.Producer.API
{
    public class ProducerService
    {
        /// <summary>
        /// Obtém as configurações do appsettings
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Configuração do cliente kafka
        /// </summary>
        private readonly ProducerConfig _producerConfig;
        /// <summary>
        /// Cria logs de execução
        /// </summary>
        private readonly ILogger<ProducerService> _logger;
        
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public ProducerService(IConfiguration configuration, ILogger<ProducerService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            //obtem o endereço do servidor
            var bootstrap = _configuration.GetSection("KafkaConfig").GetSection("BootstrapServer").Value;
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = bootstrap
            };
        }
        
        /// <summary>
        /// Envia mensagems para o servidor Kafka
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> SendMessage(PessoaModel model)
        {
            //obtem as configurações
            var topic = _configuration.GetSection("KafkaConfig").GetSection("TopicName").Value;
            //obj que sera persistido na fila
            string objSerializado = JsonSerializer.Serialize(model);

            try
            {
                using (var producer = new ProducerBuilder<Null, string> (_producerConfig).Build())
                {   
                    //persiste os dados
                    DeliveryResult<Null, string> result = await producer.ProduceAsync(topic: topic, new (){ Value = objSerializado });
                    
                    //gera log de execução
                    _logger.LogInformation(string.Concat(result.Status.ToString(), " - ", objSerializado));
                    
                    return string.Concat(result.Status.ToString(), " - ", objSerializado);
                }
            }
            catch
            {
                _logger.LogError("Erro ao enviar mensagem");
                return "Erro ao enviar mensagem";
            }
            
        }
    }
    
}