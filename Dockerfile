FROM microsoft/dotnet:1.0-sdk-msbuild

WORKDIR /app

COPY dotnetcore-web-docker.csproj .
RUN dotnet restore

COPY . .

WORKDIR /app/tls
RUN [ "/bin/bash", "/app/ssh/generateCert.sh", "sample", "7da3043" ]

WORKDIR /app
RUN dotnet publish -c Release -o bin

ENV CERT_PATH /app/tls/sample_7da3043.p12
ENV CERT_PASSWORD 7da3043
ENV HTTPS_PORT 9999

CMD [ "dotnet", "bin/dotnetcore-web-docker.dll" ]
