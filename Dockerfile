FROM microsoft/dotnet:onbuild

WORKDIR /dotnetapp/tls
RUN [ "/bin/bash", "/dotnetapp/ssh/generateCert.sh", "sample", "1f16b18" ]

WORKDIR /dotnetapp
RUN dotnet build

ENV CERT_PATH /dotnetapp/tls/sample_1f16b18.p12
ENV CERT_PASSWORD 1f16b18

CMD dotnet run
