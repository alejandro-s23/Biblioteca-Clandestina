# Biblioteca Clandestina

# Objetivo

Criar um Site WEB que gerencie empréstimos de livros de uma biblioteca clandestina no IFSUL

---

## Funções / Telas ( Usuário )

### Cadastro

O usuário deve ter um formulário para solicitar um cadastro no site, informando as seguintes informações: Nome, Sobrenome, Email, Telefone, Cidade, Bairro, Endereço, Matrícula e CPF, além da sua senha para Login.
Após ser feito o cadastro, será enviado uma solicitação para o administrador da biblioteca, que decidirá aprovar ou não o cadastro.

### Login

Terá um formulário para login, onde o usuário deve informar email e senha para logar, e após o login será redirecionado para tela de Reserva de Livros

### Reservar Livro

O usuário possui um botão que abre um menu de pesquisa e ordenação dos livros, para auxiliar na busca pelo livro ideal.
Após decidir qual livro ele vai ler, clicará no botão de realizar reserva, ação que vai criar um registro de empréstimo no nome dele, e tornará o livro indisponível à outros usuários.

### Minha Conta

Ao pressionar o botão “Minha Conta” no menu de navegação, o usuário será redirecionado à tela “Minha Conta”, onde poderá alterar algumas informações referentes ao seu cadastro e verificar qual livro está em sua posse, caso haja algum, o site mostrará à quantos dias esse livro está emprestado , caso o tempo de empréstimo seja superior à 14 dias, o site mostrará um aviso dizendo “Devolução em Atraso”, e um botão para realizar a devolução do livro.

## Funções / Telas (Administrador)

### Login

O adm deve informar seu email e sua senha definidas pelo desenvolvedor, após o login ele terá acesso ao Menu de Ações

### Menu Inicial

O menu inicial ou Dashboard, tem um contador de solicitações pendentes e empréstimos ativos, além de um que indica a % da disponibilidade de livros.
Contém também, mais dois menus que listam um resumo das solicitações pendentes e empréstimos ativos, mostrando no máximo 5 registros.

### Lista Solicitações de Cadastro

Onde será listado todas as solicitações de cadastro no site, juntamente com o nome, matricula e um botão de detalhes, que exibirá o restante das informações do candidato à cadastro, além de exibir nos detalhes um botão para aprovar o cadastro e outro para negar.

### Lista de Usuários

Onde será listado todos os usuários efetivamente cadastrados, junto com um botão para detalhes, nos detalhes desse usuário, deve haver um botão para resetar a senha do usuário e um botão de confirmação para essa ação.

### Relatório de Empréstimos

Uma página que deverá listar todos os empréstimos feito no site, com filtros para mostrar somente empréstimos ativos ou não, e empréstimos feito em um período específico, e ferramenta de pesquisa de acordo com um critério selecionado, podendo ser, Título do Livro, Autor ou Usuário. E ordenação por data, mais recente ou mais antigo.