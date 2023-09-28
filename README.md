# rinha-compiler-dotnet
Um compilador .Net simples para a Rinha de Compiler.  Isso foi feito em menos de 5 dias (praticamente) e foi testado apenas com os exemplos fornecidos,
não espere muito. Foi apenas uma aventura divertida.

Este projeto compila um arquivo AST em json da [Rinha de compiler](https://github.com/aripiprazole/rinha-de-compiler/tree/main) para uma dll .Net que
pode ser utilizada por virtualmente qualquer runtime .Net 5+.

## Como rodar

### Docker

Build:
```
docker build -t rinha .
```

Run:
```
docker run -v "$(pwd)/files/fib.json:/var/rinha/source.rinha.json" rinha
```

### Nativo

Build do Compiler:
```
dotnet publish -c Release -o out
```
Compilando um arquivo:
```
./out/rinhac files/fib.json -o targets
```

Rodando a dll gerada:
```
dotnet targets/fib/fib.dll
```

## Limitações

Atualmente o compilador roda todos os exemplos básicos da pasta files. Programas parecidos
provavelmente vão funcionar. Programas onde closures são criadas dinamicamentes e chamadas
de forma recursiva ou com dependências de variáveis externas não vão funcionar.

## Sobre Correções

Provavelmente não irá ter nenhuma por minha parte, no entanto fique a vontade para mandar um pull request
se desejar. Contudo, pretendo trabalhar novamente em compiladores Dotnet mais completos, se quiser ver algum
outro, fique de olho nos meus repositórios.
