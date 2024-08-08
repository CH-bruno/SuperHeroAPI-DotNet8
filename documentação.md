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

# SuperHero.cs

Este código define uma classe SuperHero no namespace SuperHeroAPI_DotNet8.Entities. A classe representa um super-herói e serve como uma entidade que pode ser mapeada para uma tabela em um banco de dados usando o Entity Framework Core.

### 1. Namespace

```csharp
namespace SuperHeroAPI_DotNet8.Entities
```
O namespace agrupa logicamente as classes relacionadas, neste caso, as entidades do projeto SuperHeroAPI_DotNet8.

### 2. Classe SuperHero

```csharp
public class SuperHero
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
}
```
public class SuperHero: Define uma classe pública chamada SuperHero.

Id: Uma propriedade int que serve como chave primária da entidade.

Name: Uma propriedade string que armazena o nome do super-herói. O modificador required indica que essa propriedade é obrigatória e deve ser inicializada durante a criação do objeto (introduzido no C# 11).

FirstName: Uma propriedade string que armazena o primeiro nome do super-herói. Por padrão, é inicializada com uma string vazia (string.Empty).

LastName: Uma propriedade string que armazena o sobrenome do super-herói, também inicializada com uma string vazia.

Place: Uma propriedade string que armazena o local associado ao super-herói, como cidade de origem ou base de operações, igualmente inicializada com uma string vazia.

# ILogWriter.cs

Este código define uma interface ILogWriter no namespace SuperHeroAPI_DotNet8.Interfaces. A interface especifica um contrato para classes que implementam funcionalidades de escrita de logs. 

```csharp
public interface ILogWriter
{
    void WriteLog(string message);
}
```

public interface ILogWriter: Define uma interface pública chamada ILogWriter. Em C#, uma interface é um contrato que descreve um conjunto de métodos que uma classe deve implementar, sem fornecer a implementação desses métodos.

WriteLog(string message): Este é o único método definido pela interface ILogWriter. Qualquer classe que implemente esta interface deve fornecer uma implementação para este método.

#### Propósito da Interface:

Abstração: A interface ILogWriter permite que diferentes implementações de escrita de logs sejam utilizadas de forma intercambiável. Isso promove a flexibilidade e facilita a troca de implementações sem modificar o código que depende dessa funcionalidade.

Implementação: Classes que implementam esta interface, como ConsoleLogWriter ou FileLogWriter, fornecerão a lógica específica para como os logs serão escritos (por exemplo, no console ou em um arquivo).

void: O método não retorna um valor.

string message: O método aceita uma string como parâmetro, que representa a mensagem de log a ser escrita.

# ConsoleLogWriter.cs

Este código define a classe ConsoleLogWriter no namespace SuperHeroAPI_DotNet8.Services, que implementa a interface ILogWriter. Esta classe é responsável por registrar mensagens de log no console, incluindo a data e hora em que a mensagem foi escrita.

### 1.Namespace e Usings
```csharp
using SuperHeroAPI_DotNet8.Interfaces;

namespace SuperHeroAPI_DotNet8.Services
```
using SuperHeroAPI_DotNet8.Interfaces: Importa a interface ILogWriter para que ela possa ser implementada pela classe ConsoleLogWriter.

namespace SuperHeroAPI_DotNet8.Services: Agrupa a classe ConsoleLogWriter dentro de um namespace que provavelmente contém outras classes relacionadas a serviços na aplicação.

### 2.Classe ConsoleLogWriter

```csharp
public class ConsoleLogWriter : ILogWriter
{
    public void WriteLog(string message)
    {
        Console.WriteLine($"{DateTime.Now}: {message}");
    }
}
```

public class ConsoleLogWriter : ILogWriter: Define uma classe pública ConsoleLogWriter que implementa a interface ILogWriter. Isso significa que a classe deve fornecer uma implementação para o método WriteLog definido na interface.

WriteLog(string message): Este método, exigido pela interface ILogWriter, implementa a funcionalidade de registrar uma mensagem de log.

Console.WriteLine($"{DateTime.Now}: {message}");: Escreve a mensagem no console, precedida pela data e hora atuais.

DateTime.Now: Retorna a data e hora atuais.

$"{DateTime.Now}: {message}": Usando interpolação de strings, a mensagem de log é formatada para incluir a data e hora no formato "YYYY-MM-DD HH:MM:SS: Mensagem".

### 3.Funcionamento

Logging ao Console: Quando o método WriteLog é chamado, ele escreve a mensagem especificada no console, prefixada com a data e hora atuais. Isso facilita a análise dos logs, pois cada entrada é claramente associada a um momento específico.

# FileLogWriter.cs

Este código define a classe FileLogWriter no namespace SuperHeroAPI_DotNet8.Services, que implementa a interface ILogWriter. A função dessa classe é registrar mensagens de log em um arquivo de texto. 

```csharp
namespace SuperHeroAPI_DotNet8.Services
{
    public class FileLogWriter : ILogWriter
    {
        private readonly string _filePath;

        public FileLogWriter(string filePath)
        {
            _filePath = filePath;
        }

        public void WriteLog(string message)
        {
            using (var writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}
```
public class FileLogWriter : ILogWriter: Define uma classe pública FileLogWriter que implementa a interface ILogWriter. Isso exige que a classe implemente o método WriteLog.

private readonly string _filePath;: Define um campo privado e somente leitura (readonly) chamado _filePath. Esse campo armazena o caminho do arquivo onde os logs serão gravados. Ele é inicializado no construtor da classe.

public FileLogWriter(string filePath): Este é o construtor da classe, que recebe um parâmetro filePath. O caminho do arquivo é armazenado no campo _filePath.

public void WriteLog(string message): Implementa o método WriteLog da interface ILogWriter.

using (var writer = new StreamWriter(_filePath, true)): Cria uma instância de StreamWriter para escrever no arquivo especificado por _filePath. O segundo argumento true indica que o conteúdo será anexado ao arquivo, em vez de sobrescrevê-lo.

writer.WriteLine($"{DateTime.Now}: {message}");: Escreve a mensagem no arquivo, precedida pela data e hora atuais. Cada chamada a WriteLog resulta em uma nova linha no arquivo de log.

#### Funcionamento
Logging em Arquivo: Quando WriteLog é chamado, a mensagem de log é escrita no arquivo especificado. Se o arquivo não existir, ele será criado. Se já existir, a mensagem será anexada ao final do arquivo, preservando os logs anteriores.

# DataContext.cs
Este código define a classe DataContext no namespace SuperHeroAPI_DotNet8.Data, que herda de DbContext, a classe base do Entity Framework Core. DataContext gerencia a conexão com o banco de dados e o mapeamento das entidades para as tabelas do banco de dados. 

```csharp
namespace SuperHeroAPI_DotNet8.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)    
        { 
        
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }
    }
}
```
public class DataContext : DbContext: Define uma classe pública DataContext que herda de DbContext. O DbContext é a classe central do Entity Framework Core que coordena a funcionalidade de mapeamento e as operações de persistência de dados.

public DataContext(DbContextOptions<DataContext> options) : base(options): Este é o construtor da classe DataContext. Ele recebe uma instância de DbContextOptions<DataContext> como parâmetro, que contém as configurações para o contexto, como a string de conexão ao banco de dados. O construtor passa essa configuração para o construtor da classe base DbContext.

public DbSet<SuperHero> SuperHeroes { get; set; }: Define uma propriedade DbSet<SuperHero> chamada SuperHeroes.

DbSet<SuperHero>: Representa uma coleção de todas as entidades SuperHero no contexto, ou que podem ser consultadas do banco de dados. Isso mapeia a classe SuperHero para uma tabela no banco de dados.

SuperHeroes: Esta propriedade será utilizada para realizar operações CRUD (Create, Read, Update, Delete) na tabela correspondente no banco de dados.

#### Funcionamento
Contexto de Dados: A classe DataContext serve como um intermediário entre a aplicação e o banco de dados, gerenciando a consulta e a persistência dos dados.

Mapeamento de Entidades: Cada DbSet<T> dentro do DataContext mapeia uma entidade (como SuperHero) para uma tabela no banco de dados. No caso, SuperHero é mapeado para uma tabela que  será chamada SuperHeroes.

# 20240725225751_Initial.cs

Este código define uma classe de migração chamada Initial no namespace SuperHeroAPI_DotNet8.Migrations, que é responsável por criar a tabela SuperHeroes no banco de dados e também por revertê-la se necessário. As migrações no Entity Framework Core são usadas para atualizar o esquema do banco de dados conforme as classes de entidade do modelo mudam. Vamos analisar o código:

### 1. Namespace e Usings
```csharp
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperHeroAPI_DotNet8.Migrations
```
using Microsoft.EntityFrameworkCore.Migrations: Importa os componentes necessários para criar e aplicar migrações com o Entity Framework Core.

#nullable disable: Desabilita a checagem de nullabilidade no código a seguir. Isso significa que o compilador não emitirá avisos ou erros relacionados a referências anuláveis.
### 2. Classe Initial e Métodos Up e Down
```csharp
/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SuperHeroes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Place = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SuperHeroes", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SuperHeroes");
    }
}
```
public partial class Initial : Migration: Define uma classe pública Initial que herda de Migration. A classe Migration fornece os métodos Up e Down que são usados para aplicar e reverter a migração, respectivamente.

protected override void Up(MigrationBuilder migrationBuilder): Este método define as operações que serão realizadas ao aplicar a migração. No caso, ele cria a tabela SuperHeroes com as colunas especificadas.

migrationBuilder.CreateTable: Cria uma nova tabela no banco de dados.

name: "SuperHeroes": Nome da tabela a ser criada.

columns: table => new: Define as colunas da tabela.

Id: Coluna do tipo int que serve como chave primária. Ela é auto-incrementada usando a anotação "SqlServer:Identity", "1, 1".

Name, FirstName, LastName, Place: Colunas do tipo string (nvarchar(max)) para armazenar os atributos do super-herói. Todas são definidas como nullable: false, o que significa que não podem ser nulas.

constraints: table =>: Define as restrições da tabela.

table.PrimaryKey("PK_SuperHeroes", x => x.Id): Define a chave primária da tabela como sendo a coluna Id.

protected override void Down(MigrationBuilder migrationBuilder): Este método define as operações que serão realizadas para reverter a migração. Neste caso, ele remove a tabela SuperHeroes.

migrationBuilder.DropTable(name: "SuperHeroes"): Exclui a tabela SuperHeroes do banco de dados.

#### Funcionamento
Aplicação da Migração (Método Up): Quando você aplica essa migração, o método Up é executado, criando a tabela SuperHeroes no banco de dados com as colunas Id, Name, FirstName, LastName, e Place.

Reversão da Migração (Método Down): Se por algum motivo você precisar reverter essa migração, o método Down será executado, removendo a tabela SuperHeroes do banco de dados.

### 3. Comandos no terminal Migrations

#### 1. Adicionar uma Nova Migração
Este comando cria uma nova migração com base nas alterações feitas no modelo de dados. Você deve especificar um nome para a migração.

``` bash
dotnet ef migrations add NomeDaMigracao
```
#### 2. Atualizar o Banco de Dados com a Migração
Aplica todas as migrações pendentes ao banco de dados. Isso cria ou atualiza o esquema do banco de dados com base nas migrações.

``` bash
dotnet ef database update
```
#### 3. Remover a Última Migração
Se você adicionou uma migração que ainda não foi aplicada ao banco de dados, você pode removê-la com este comando.

``` bash
dotnet ef migrations remove
```

#### 4. Listar Migrações
Mostra uma lista de todas as migrações aplicadas e pendentes.

``` bash
dotnet ef migrations list
```
#### 5. Gerar Scripts SQL
Gera um script SQL para a migração, que pode ser útil para aplicar as alterações manualmente ou para fins de auditoria.

``` bash
dotnet ef migrations script
```
#### 6. Reverter a Última Migração
Reverte a última migração aplicada ao banco de dados. Esse comando pode ser útil se você precisar desfazer uma migração.

``` bash
dotnet ef database update LastGoodMigration
```
(Substitua LastGoodMigration pelo nome da última migração boa antes da que você quer reverter.)

# DataContextModelSnapshot.cs
O código fornecido é uma classe de snapshot de modelo gerada automaticamente pelo Entity Framework Core, que representa a estrutura atual do modelo de dados no banco de dados. Esta classe é usada internamente pelo EF Core para rastrear alterações e gerar migrações. 

### 1.Namespace e Usings
``` csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuperHeroAPI_DotNet8.Data;

#nullable disable
```
using Microsoft.EntityFrameworkCore: Importa os componentes principais do Entity Framework Core.

using Microsoft.EntityFrameworkCore.Infrastructure, Metadata, Storage.ValueConversion: Importa namespaces relacionados à infraestrutura do EF Core e configuração de modelo.

using SuperHeroAPI_DotNet8.Data: Importa o namespace que contém o DataContext.

#nullable disable: Desabilita a verificação de nullabilidade para o código gerado automaticamente.

### 2.Classe DataContextModelSnapshot

``` csharp
namespace SuperHeroAPI_DotNet8.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            #pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SuperHeroAPI_DotNet8.Entities.SuperHero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Place")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SuperHeroes");
                });
            #pragma warning restore 612, 618
        }
    }
}
```

[DbContext(typeof(DataContext))]: Indica que esta snapshot de modelo está associada ao DataContext. O atributo DbContext é usado para associar o ModelSnapshot à classe DbContext que representa o contexto de dados da aplicação.

partial class DataContextModelSnapshot : ModelSnapshot: Define uma classe parcial chamada DataContextModelSnapshot que herda de ModelSnapshot. Esta classe é gerada automaticamente e não deve ser modificada manualmente.

protected override void BuildModel(ModelBuilder modelBuilder): Método que constrói o modelo de dados para o contexto. Este método é usado pelo EF Core para definir a estrutura do modelo que será refletida no banco de dados.

modelBuilder.HasAnnotation("ProductVersion", "8.0.7"): Define a versão do EF Core usada.

modelBuilder.HasAnnotation("Relational
", 128): Define o comprimento máximo dos identificadores do banco de dados, como nomes de colunas e tabelas.

SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder): Configura o uso de colunas de identidade no SQL Server para gerar valores automaticamente para a coluna Id.

modelBuilder.Entity("SuperHeroAPI_DotNet8.Entities.SuperHero", b =>: Configura a entidade SuperHero:

b.Property<int>("Id"): Configura a propriedade Id como uma coluna do tipo int que é gerada automaticamente.

b.Property<string>("FirstName"): Configura a propriedade FirstName como uma coluna do tipo nvarchar(max) e define que é obrigatória (IsRequired).

b.Property<string>("LastName"): Configura a propriedade LastName da mesma forma que FirstName.

b.Property<string>("Name"): Configura a propriedade Name da mesma forma.

b.Property<string>("Place"): Configura a propriedade Place da mesma forma.

b.HasKey("Id"): Define a propriedade Id como a chave primária da tabela.

b.ToTable("SuperHeroes"): Define o nome da tabela no banco de dados como SuperHeroes.

# launchSettings.json
O arquivo launchSettings.json é usado no .NET para configurar diferentes perfis de execução para uma aplicação. Ele define como a aplicação deve ser iniciada e quais configurações de ambiente devem ser usadas para desenvolvimento.

### 1.Estrutura e Configurações
``` csharp
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:33815",
      "sslPort": 44385
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5140",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7127;http://localhost:5140",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

```
#### Schema
  "$schema": "http://json.schemastore.org/launchsettings.json"
    Define o esquema JSON para validação e sugestões no editor.
#### iisSettings
  "windowsAuthentication": false
    Desativa a autenticação do Windows para o IIS Express.
  "anonymousAuthentication": true    
    Habilita a autenticação anônima no IIS Express.
#### "iisExpress"
  Configurações específicas para o IIS Express:
    "applicationUrl": "http://localhost:33815": URL onde a aplicação será acessível ao executar no IIS Express.
    "sslPort": 44385: Porta SSL para HTTPS no IIS Express.
#### profiles
Define diferentes perfis de inicialização para a aplicação:

###### "http"

"commandName": "Project": Executa o projeto diretamente com dotnet run.

"dotnetRunMessages": true: Habilita mensagens de execução do dotnet run.

"launchBrowser": true: Abre o navegador automaticamente quando a aplicação iniciar.

"launchUrl": "swagger": URL inicial a ser aberta no navegador. Neste caso, é a página do Swagger.

"applicationUrl": "http://localhost:5140": URL base para a aplicação quando executada usando este perfil.

"environmentVariables": Define variáveis de ambiente.

"ASPNETCORE_ENVIRONMENT": "Development": Define o ambiente como Desenvolvimento.

###### "https"

"commandName": "Project": Semelhante ao perfil http, mas para execução em HTTPS.

"applicationUrl": "https://localhost:7127;http://localhost:5140": Define URLs para HTTPS e HTTP. A aplicação pode ser acessada em ambos os protocolos.

"launchUrl": "swagger": Abre a página do Swagger no navegador.

"environmentVariables": Define o ambiente como Desenvolvimento.

###### "IIS Express"

"commandName": "IISExpress": Configura a execução com o IIS Express.

"launchBrowser": true: Abre o navegador automaticamente.

"launchUrl": "swagger": URL inicial a ser aberta no navegador.

"environmentVariables": Define o ambiente como Desenvolvimento.
