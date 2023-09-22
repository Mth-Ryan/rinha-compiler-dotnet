#! /bin/bash

set -e

./rinha-compiler-dotnet "$1" -o targets

TARGET=$(basename "$1" ".json")
dotnet ./targets/$TARGET/"$TARGET.dll"
