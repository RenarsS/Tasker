using Carter;
using MassTransit;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.Chroma;
using Microsoft.SemanticKernel.Connectors.Weaviate;
using Microsoft.SemanticKernel.Memory;
using OpenAI;
using Quartz;
using StackExchange.Redis;
using Tasker.API.Consumers;
using Tasker.API.Services;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Events;
using Tasker.Infrastructure.Builders;
using Tasker.Infrastructure.Client;
using Tasker.Infrastructure.Client.Interfaces;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Database.Interface;
using Tasker.Infrastructure.Mapping;
using Tasker.Infrastructure.Processor;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories;
using Tasker.Infrastructure.Repositories.Interfaces;


var builder = WebApplication.CreateSlimBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddScoped<OracleDbService>(provider =>
    new OracleDbService(builder.Configuration.GetConnectionString("TaskerDb")));

builder.Services.AddScoped<IChromaClient, ChromaClient>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["Redis:Host"]!));
builder.Services.AddHttpClient();
builder.Services.AddSingleton(new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]));

builder.Services.AddChatClient(services 
    => services.GetRequiredService<OpenAIClient>().AsChatClient(builder.Configuration["OpenAI:ChatModel"]!));

builder.Services.AddEmbeddingGenerator(services
    => services.GetRequiredService<OpenAIClient>().AsEmbeddingGenerator(builder.Configuration["OpenAI:EmbeddingModel"]!));

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddAutoMapper(typeof(TaskerProfile).Assembly);

builder.Services.AddScoped<IMemoryStore, WeaviateMemoryStore>((services) => new WeaviateMemoryStore(builder.Configuration["WeaviateDb:Endpoint"]));
builder.Services.AddScoped<IEmbeddingClient, WeaviateEmbeddingClient>();
builder.Services.AddScoped<IEmbeddingProcessor, EmbeddingProcessor>();
builder.Services.AddScoped<OrderBuilder>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<IAssignmentRepository,AssignmentRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IStatusRepository,StatusRepository>();
builder.Services.AddScoped<ITaskTypeRepository,TaskTypeRepository>();
builder.Services.AddScoped<IQueryRepository,QueryRepository>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<ITaskTypeService, TaskTypeService>();
builder.Services.AddScoped<IQueryService, QueryService>();
builder.Services.AddScoped<IResponseService, ResponseService>();
builder.Services.AddScoped<IDataImportService, DataImportService>();
builder.Services.AddScoped<IRetrievalService, RetrievalService>();
builder.Services.AddScoped<IPromptService, PromptService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

builder.Services.AddScoped<IVectorSeedTask, VectorSeedTask>();

builder.Services.Configure<RouteOptions>(options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument();
builder.Services.AddCarter();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<NewTaskCreatedConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMqSettings:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMqSettings:Username"]!);
            h.Password(builder.Configuration["RabbitMqSettings:Password"]!);
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var vectorSeedTask = scope.ServiceProvider.GetRequiredService<IVectorSeedTask>();
    await vectorSeedTask.Execute();
}

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();
app.Run();