# Program.cs

### 1. Adicionando Serviços ao Contêiner

A aplicação adiciona vários serviços ao contêiner de injeção de dependência:

```csharp
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```
  
AddControllers(): Adiciona suporte para controladores, permitindo a criação de APIs RESTful.

AddEndpointsApiExplorer(): Gera descrições de endpoints para suporte a ferramentas como o Swagger.

AddSwaggerGen(): Adiciona suporte ao Swagger, que é usado para gerar documentação interativa da API.
  
### 2. Configuração do Banco de Dados
  
O banco de dados é configurado utilizando o Entity Framework Core, com o SQL Server como provedor:

```csharp
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```
AddDbContext<DataContext>(): Registra o DataContext no contêiner de serviços usando o SQL Server como banco de dados, com a string de conexão definida no arquivo de configuração (por exemplo, appsettings.json).

### 3. Registrando Serviços de Log
  
O projeto inclui dois serviços de logging personalizados:

```csharp
builder.Services.AddSingleton<ILogWriter, ConsoleLogWriter>();
builder.Services.AddSingleton<ILogWriter>(provider => new FileLogWriter("log.txt"));
```
AddSingleton<ILogWriter, ConsoleLogWriter>(): Registra o ConsoleLogWriter como uma implementação da interface ILogWriter, para logs no console.

AddSingleton<ILogWriter>(provider => new FileLogWriter("log.txt")): Registra o FileLogWriter, que grava logs em um arquivo de texto.
  
### 4.  Construindo e Configurando o Pipeline de Solicitação HTTP

A configuração do pipeline de solicitação HTTP inclui:

```csharp
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```
  
app.Environment.IsDevelopment(): Verifica se a aplicação está em ambiente de desenvolvimento. Se estiver, ativa o Swagger e a interface Swagger UI.

UseSwagger()/UseSwaggerUI(): Configura a aplicação para usar o Swagger para geração de documentação da API.

UseHttpsRedirection(): Redireciona automaticamente solicitações HTTP para HTTPS.

UseAuthorization(): Configura a autorização de requisições.

MapControllers(): Mapeia os controladores para os endpoints.

app.Run(): Inicia a aplicação.
