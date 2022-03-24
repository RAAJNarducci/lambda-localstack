# lambda-localstack - Lambda que criar um indice e insere um documento no ES


## Passos para criar o lambda no Localstack:

1. dotnet publish -c Release -o publish
2. zipar o conteúdo da pasta
3. na pasta localstack, crie uma role com o seguinte comando:
- aws --endpoint http://localhost:4566 iam create-role --role-name lambda-dotnet-webapi-ex --assume-role-policy-document file://trust-policy.json
4. em seguida utilize o comando abaixo para realizar o attach da role criada:
- aws --endpoint http://localhost:4566 iam attach-role-policy --role-name lambda-dotnet-webapi-ex --policy-arn arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole
5. na pasta onde está seu arquivo .zip, execute o seguinte comando que criará a função no localstack:
- aws --endpoint http://localhost:4566 lambda create-function --function-name lambda-dotnet --zip-file fileb://publish.zip --handler Example.Lambda::Example.Lambda.Function::FunctionHandler --runtime dotnetcore3.1 --role arn:aws:iam::000000000000:role/lambda-dotnet-webapi-ex
6. em seguida execute o comando que dará o invoke na função:
- aws --endpoint http://localhost:4566 lambda invoke --function-name lambda-dotnet --payload "\"test\"" response.json --log-type Tail --cli-binary-format raw-in-base64-out
7. o retorno se dará no arquivo response.json no diretório onde você estiver com prompt aberto.