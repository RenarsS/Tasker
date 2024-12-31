using Carter;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.Chroma;
using OpenAI;
using Tasker.API.Services;
using Tasker.API.Services.Interfaces;
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

#pragma warning disable SKEXP0020

var builder = WebApplication.CreateSlimBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddScoped<OracleDbService>(provider =>
    new OracleDbService(builder.Configuration.GetConnectionString("TaskerDb")));

builder.Services.AddScoped<IChromaClient, ChromaClient>();

builder.Services.AddHttpClient();
builder.Services.AddSingleton(new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]));

builder.Services.AddChatClient(services 
    => services.GetRequiredService<OpenAIClient>().AsChatClient(builder.Configuration["OpenAI:ChatModel"]!));

builder.Services.AddEmbeddingGenerator(services
    => services.GetRequiredService<OpenAIClient>().AsEmbeddingGenerator(builder.Configuration["OpenAI:EmbeddingModel"]!));

builder.Services.AddAutoMapper(typeof(TaskerProfile).Assembly);

builder.Services.AddScoped<IEmbeddingClient, MilvusEmbeddingClient>();
builder.Services.AddScoped<IEmbeddingProcessor, EmbeddingProcessor>();
builder.Services.AddScoped<OrderBuilder>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<IAssignmentRepository,AssignmentRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IStatusRepository,StatusRepository>();
builder.Services.AddScoped<ITaskTypeRepository,TaskTypeRepository>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<ITaskTypeService, TaskTypeService>();
builder.Services.AddScoped<IDataImportService, DataImportService>();

builder.Services.AddScoped<IVectorSeedTask, VectorSeedTask>();

builder.Services.Configure<RouteOptions>(options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument();
builder.Services.AddCarter();

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