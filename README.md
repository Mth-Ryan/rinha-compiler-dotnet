# rinha-compiler-dotnet
Um compilador .Net simples para a Rinha de Compiler.  Isso foi feito em menos de 5 dias (praticamente), não espere muito. Foi apenas uma aventura divertida.

Este compilador compila um arquivo AST em json da [Rinha de compiler](https://github.com/aripiprazole/rinha-de-compiler/tree/main) para uma dll .Net que
pode ser utilizada por virtualmente qualquer .Net runtime 5+.

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
