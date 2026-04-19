# 📚 Biblioteca Clandestina
<p>A Biblioteca Clandestina é uma aplicação web robusta voltada para a gestão de acervos literários e controle de empréstimos. Desenvolvida como parte dos estudos de Análise e Desenvolvimento de Sistemas (UNOPAR), o projeto foca em escalabilidade, manutenção facilitada e boas práticas de arquitetura de software.</p>

## 🏛️ Arquitetura e Padrões de Projeto
<p>Para garantir um código limpo (Clean Code) e desacoplado, a aplicação foi estruturada utilizando:</p>

<p><strong>Repository Pattern (Generic):</strong> Implementação de um BaseRepository<T> abstrato que centraliza as operações de CRUD, permitindo que novos repositórios sejam criados com o mínimo de código redundante.</p>

<p><strong>Service Layer:</strong> Toda a lógica de negócio (como validação de empréstimos e processamento de solicitações) está isolada em serviços, garantindo que os Controllers fiquem "magros" e focados apenas no fluxo da requisição.</p>

<p><strong>Dependency Injection (DI):</strong> Uso do container nativo do .NET para gerenciar o ciclo de vida dos objetos, facilitando a testabilidade e a inversão de controle.</p>

<p><strong>Polymorphic Request Handling:</strong> Técnica avançada para processar corpos de requisição dinâmicos (Dictionary<string, object>) e convertê-los em objetos fortemente tipados através de lógica de resolução por Enum.</p>

## 🚀 Tecnologias Utilizadas
### Back-end

Linguagem: C# (.NET 10.0)

Framework: ASP.NET Core MVC

ORM: Entity Framework Core (Code First)

Banco de Dados: SQL Server (Configurado via Variáveis de Ambiente)

### Front-end

Design: Bootstrap 5 & CSS Personalizado (Tema Dark/Clandestino)

Interatividade: JavaScript (ES6+), jQuery e AJAX para carregamento dinâmico de relatórios

Feedback Visual: Modais dinâmicos para detalhes de usuários e TempData para alertas globais

## 📊 Funcionalidades em Destaque

<p><strong>Dashboard Administrativo:</strong> Visão consolidada de solicitações pendentes e disponibilidade do acervo.</p>

<p><strong>Central de Relatórios:</strong> Sistema de abas dinâmicas que utiliza Partial Views para carregar dados de usuários e livros sob demanda.</p>

<p><strong>Controle de Posse:</strong> Regra de negócio que limita o usuário a possuir apenas um exemplar ativo por vez.</p>

<p><strong>Sistema de Aprovação:</strong> Fluxo de entrevista e validação de novos membros pela equipe administrativa.</p>

## 👨‍💻 Autor
<p>Alejandro Souza dos Santos Graduando em Análise e Desenvolvimento de Sistemas - Jaguarão/RS.</p>

<p>Focado em Arquitetura de Software e Desenvolvimento Back-end com ecossistema .NET.</p>
