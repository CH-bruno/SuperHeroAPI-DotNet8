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

# appsettings.json

É usado em projetos ASP.NET Core para configurar a aplicação, incluindo as strings de conexão, o nível de logging e outras configurações.

### 1. ConnectionStrings

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLExpress;Database=SuperHeroDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```
ConnectionStrings: Contém as strings de conexão para os bancos de dados que a aplicação utilizará.

DefaultConnection: Nome da string de conexão usada no projeto. Neste caso, ela se conecta a um servidor SQL Server local (.\\SQLExpress) e ao banco de dados chamado SuperHeroDb.

Trusted_Connection=true: Utiliza a autenticação integrada do Windows.

TrustServerCertificate=true: Ignora problemas relacionados ao certificado SSL ao conectar-se ao banco de dados (útil em ambientes de desenvolvimento).

### 2. Logging

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```
Logging: Configura as opções de logging para a aplicação.

LogLevel: Define o nível mínimo de log que será registrado.

Default: Define o nível de log padrão como Information, o que significa que logs de Information, Warning, Error, e Critical serão registrados.

Microsoft.AspNetCore: Define o nível de log para os logs gerados por componentes Microsoft.AspNetCore como Warning, então somente logs de Warning, Error, e Critical serão registrados para essa categoria.

### 3. AllowedHosts

```json
"AllowedHosts": "*"
```
AllowedHosts: Define quais hosts são permitidos para acessar a aplicação. O valor "*" significa que todos os hosts são permitidos, o que é comum em ambientes de desenvolvimento.
