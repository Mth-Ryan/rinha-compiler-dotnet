#! /bin/bash

set -e

./rinhac "/var/rinha/source.rinha.json" -o targets

TARGET="source.rinha"
dotnet ./targets/$TARGET/"$TARGET.dll"
