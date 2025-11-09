# API Catálogo - Lojas Henner

Esta é uma API REST para gerenciamento de catálogo de produtos, construída com .NET 9 e Clean Architecture.

## 🛠️ Stack Tecnológica

- .NET 9 (SDK)
- ASP.NET Core (para Web API)
- Entity Framework Core 9 (ORM)
- PostgreSQL (Banco de Dados Relacional)
- MinIO (Serviço de Storage compatível com S3)
- Docker (Gerenciamento de contêineres da infraestrutura)
- MediatR (Padrão Mediator para os Use Cases)
- FluentResults (Tratamento de erros e resultados)
- FluentValidation (Validação de DTOs e Comandos)
- Swashbuckle (Geração de documentação Swagger)
- xUnit / NSubstitute (Testes Unitários)

## 🚀 Como Configurar e Rodar o Projeto (Setup de Nova Máquina)

Siga estes passos para configurar todo o ambiente de desenvolvimento.

Pré-requisitos

Antes de começar, garanta que você tem os seguintes softwares instalados:

.NET 9 SDK

Docker Desktop

EF Core CLI Tool (ferramenta de linha de comando):
```
dotnet tool install --global dotnet-ef
```

(Se já tiver instalado, rode dotnet tool update --global dotnet-ef para garantir a versão mais recente).

Passo 1: Configurar Variáveis de Ambiente (Para Migrations Locais)

O Docker usará as variáveis definidas no docker-compose.yml, mas as suas ferramentas locais (como o dotnet ef) precisam ler as variáveis do seu sistema.

Abra as "Variáveis de Ambiente" do seu Windows.

Crie as seguintes Variáveis de Usuário:

Banco de Dados:

Nome: 
```
ConnectionStrings__LojasHennerDb
```
Valor:
```
Host=localhost;Port=5432;Database=lojas_henner_db;Username=postgres;Password=admin
```

IMPORTANTE: Após criar as variáveis, feche e reabra seu terminal e sua IDE (VS Code/Visual Studio) para que eles carreguem os novos valores.

Passo 2: Levantar o Banco de Dados (DB)

Vamos usar o Docker para iniciar o banco de dados.

Inicie o Docker Desktop na sua máquina (espere o ícone da baleia parar de animar e ficar estável).

Abra um terminal na pasta raiz da solução (onde está o docker-compose.yml).

Execute o seguinte comando para iniciar apenas o banco:
```
docker-compose up -d db
```

Passo 3: Preparar o Banco de Dados (Migrations)

Com a infraestrutura rodando (Passo 2), vamos criar as tabelas no banco de dados.

Abra um novo terminal na raiz do projeto (onde está o .slnx).

Crie a Migration (instrução C# para o EF Core):

Se a pasta Infrastructure/Migrations ainda não existir:
```
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project WebAPI
```

Aplique a Migration (executa o SQL no banco):
```
dotnet ef database update --project Infrastructure --startup-project WebAPI
```

(Use WebAPI (maiúsculo) se esse for o nome da sua pasta/projeto).

Agora você pode abrir o lojas_henner_db (via DBeaver, etc.) e a tabela Produtos estará lá.

Passo 4: Executar a Aplicação (Via Docker)

Com o banco de dados migrado, agora podemos levantar a API.

Limpeza (Obrigatório se houver erros antigos):

Execute docker-compose down e docker volume prune -f para limpar quaisquer volumes ou contêineres "fantasmas" de tentativas anteriores.

Inicie a Stack (API + DB):
No seu terminal (na raiz do projeto), execute:
```
docker-compose up -d --build
```

(O --build força o Docker a reconstruir sua imagem da API. O -d roda em background).

A API estará rodando e ouvindo na porta 5015.

A documentação do Swagger estará disponível em:
```
http://localhost:5015/swagger
```
Passo 5: Como Testar (Importante!)

Você pode usar a UI do Swagger (/swagger) para testar todos os endpoints (GET, POST, PUT, DELETE).

⚠️ AVISO: Testando o Upload de Imagem

O Upload de Imagem requer setup manual do MinIO.

O serviço minio no docker-compose.yml foi desabilitado temporariamente pois estava falhando ao iniciar (exited (1)).

Para testar o upload, você precisará instalar e rodar o MinIO manualmente na sua máquina local (localhost:9000) com as credenciais (usuario/admin).

A API (rodando no Docker) está configurada para procurar o MinIO em http://host.docker.internal:9000.

Teste via Postman (Após MinIO manual):

Método: POST

URL: http://localhost:5015/api/Produtos/SEU_GUID_DE_PRODUTO_AQUI/foto

Selecione a aba Body.

Selecione a opção form-data.

Adicione uma chave (key) chamada ArquivoFoto.

No lado direito da chave, mude o tipo de Text para File.

Clique em "Select Files" e anexe sua imagem (.jpg, .png, etc.).

Clique em Send.
