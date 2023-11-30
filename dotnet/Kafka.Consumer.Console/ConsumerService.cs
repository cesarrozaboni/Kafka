using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kafka.Consumer.Console
{
    public class ConsumerService : BackgroundService
    {
        /// <summary>
        /// Consumidor do servico
        /// </summary>
        private readonly IConsumer<Ignore, string> _consumer;
        /// <summary>
        /// Configurações do consumidor de mensagens
        /// </summary>
        private readonly ConsumerConfig _consumerConfig;
        /// <summary>
        /// Cria logs de execução
        /// </summary>
        private readonly ILogger<ConsumerService> _logger;
        /// <summary>
        /// Parametros do consumidor
        /// </summary>
        private readonly ParametersModel _parameters;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="logger"></param>
        public ConsumerService(ILogger<ConsumerService> logger)
        {
            _parameters = new ParametersModel();
            _logger     = logger;
            
            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = _parameters.BootstrapServer,
                GroupId          = _parameters.GroupId,
                AutoOffsetReset  = AutoOffsetReset.Earliest //Marca a mensagem do broker como lida
            };

            _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        }

        /// <summary>
        /// Interface para consumir o serviço do kafka
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aguardando mensagens");
            
            //Informa o tópico que sera consumido por essa instancia
            //Increve o consumidor para ouvir este tópico no broker
            _consumer.Subscribe(_parameters.TopicName);

            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => 
                {
                    //Obtem uma nova mensagem, se não possuir fica aguardando uma nova, e assim que possuir o evento é disparado
                    ConsumeResult<Ignore, string> result = _consumer.Consume(stoppingToken);

                    //descerializa a mensagem
                    PessoaModel? pessoa = JsonSerializer.Deserialize<PessoaModel>(result.Message.Value);

                    _logger.LogInformation($"GroupId: {_parameters.GroupId} Mensagem: {result.Message.Value}");
                    _logger.LogInformation(pessoa?.ToString());
                });
            }
        }

        /// <summary>
        /// sobreescreve o metodo stopasync da interface para gravar log da aplicação
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _consumer.Close();
            _logger.LogInformation("Aplicação parou, conexão fechada");

            return Task.CompletedTask;
        }
    }
}