#!/bin/bash

gnome-terminal -- bash -c "cd Gateway.API && dotnet watch run"
gnome-terminal -- bash -c "cd Auth.API && dotnet watch run"
gnome-terminal -- bash -c "cd Estoque.API && dotnet watch run"
gnome-terminal -- bash -c "cd Vendas.API && dotnet watch run"