FROM microsoft/dotnet:1.0.1-sdk-projectjson
ENTRYPOINT ["dotnet", "RecommendationAPI.dll"]
WORKDIR /app
EXPOSE 80
COPY $source .
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
RUN dotnet restore
ENTRYPOINT ["dotnet", "run"]