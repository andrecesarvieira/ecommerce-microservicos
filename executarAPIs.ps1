Start-Process powershell -ArgumentList 'cd Gateway.API; dotnet watch run'
Start-Process powershell -ArgumentList 'cd Auth.API; dotnet watch run'
Start-Process powershell -ArgumentList 'cd Estoque.API; dotnet watch run'
Start-Process powershell -ArgumentList 'cd Vendas.API; dotnet watch run'
