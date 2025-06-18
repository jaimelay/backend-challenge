# Instruções para executar o projeto

- Dentro da pasta `/src` possui um `docker-compose.yml` para rodar basta executar o seguinte comando: `docker-compose up --build -d``
- Ele irá rodar a API e o PostgreSQL
- A API poderá ser acessada pelo swagger: `http://localhost:5080/swagger`
- Ao inicializar o projeto irá ser criado as tabelas pelas migrations e que também serão populadas pelo seed encontrado na pasta `/src/Infrastructure/Seed/`