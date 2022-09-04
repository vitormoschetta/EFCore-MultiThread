## EF Core e Multi-Threading

O DbContext do EF Core NÃO deve ser compartilhado entre threads. Se você precisar acessar o mesmo DbContext de várias threads, você deve criar uma instância do Contexto para cada thread, conforme `Sample2` no projeto de exemplo.

Quando precisar trabalhar com várias threads tente limitar o escopo das threads em atualiar os objetos apenas em memória, e depois salvar as alterações em um único contexto (na thread principal), conforme `Sample1` no projeto de exemplo.


## Subir banco de dados em container
```
docker-compose up -d
```
