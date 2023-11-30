namespace Kafka.Consumer.Console
{
    public class ParametersModel
    {
        /// <summary>
        /// Construtor com as configurações
        /// </summary>
        public ParametersModel()
        {
            BootstrapServer = "localhost:9092";
            TopicName       = "topic1";
            GroupId         = "Group 1";
        }

        /// <summary>
        /// Servidor bootstrap
        /// </summary>
        public string BootstrapServer { get; set; }
        /// <summary>
        /// Nome do topico
        /// </summary>
        public string TopicName { get; set; }
        /// <summary>
        /// Id do grupo
        /// </summary>
        public string GroupId { get; set; }
    }
}