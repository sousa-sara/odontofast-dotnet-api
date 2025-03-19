using Microsoft.EntityFrameworkCore;
using OdontofastAPI.Data;
using OdontofastAPI.Repository.Interfaces;
using OdontofastAPI.Repository;
using OdontofastAPI.Service.Implementations;
using OdontofastAPI.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do banco de dados Oracle.
// O AddDbContext adiciona o contexto de dados ao cont�iner de DI, configurando o acesso ao banco de dados.
// UseOracle � o m�todo que configura o Entity Framework para se conectar ao Oracle, usando a string de conex�o "OracleConnection" do arquivo de configura��o.
builder.Services.AddDbContext<OdontofastDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyMethod()  // Permite qualquer m�todo HTTP
              .AllowAnyHeader();  // Permite qualquer cabe�alho
    });
});

// Registro de depend�ncias para o reposit�rio e servi�o de Usu�rio no container de inje��o de depend�ncia (DI).
// AddScoped garante que uma nova inst�ncia ser� criada para cada requisi��o HTTP.
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Registro do ConfiguracaoService como um Singleton, o que significa que a mesma inst�ncia ser� utilizada em toda a aplica��o.
// Usado, pois o Singleton � adequado para servi�os que n�o precisam ser criados v�rias vezes e s�o reutilizados ao longo do ciclo de vida da aplica��o.
builder.Services.AddSingleton<IConfiguracaoService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return ConfiguracaoService.GetInstance(configuration);
});

// Configura��o dos controllers, que s�o usados para mapear os endpoints da API.
builder.Services.AddControllers();

// Habilita��o do Swagger para gerar e exibir documenta��o da API. O Swagger � escolhido, pois facilita a intera��o com a API durante o desenvolvimento e a gera��o de documenta��o.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cria��o da inst�ncia da aplica��o (app) a partir das configura��es do builder.
var app = builder.Build();

// Condicional que verifica se o ambiente da aplica��o � de desenvolvimento.
// Se for, o Swagger ser� habilitado para gerar a documenta��o interativa da API.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o Swagger
    app.UseSwaggerUI(); // Habilita a interface do Swagger UI
}

// Use CORS
app.UseCors("AllowAll");

// Configura��o da autoriza��o na aplica��o. Isso � necess�rio para controlar o acesso aos recursos protegidos da API.
app.UseAuthorization();

// Mapeamento dos controllers para que o ASP.NET Core saiba como lidar com as rotas da API.
app.MapControllers();

// Inicia o servidor web e executa a aplica��o.
app.Run();
