using Kafka.Producer.API;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ProducerService>();
var app = builder.Build();

//Cria a instancia de producerService
app.MapPost("/", async ([FromServices]ProducerService service, [FromBody]PessoaModel model) =>
{
    //Envia a mensagem
    return await service.SendMessage(model);
});

app.Run();