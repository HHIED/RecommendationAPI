# Use the standard Microsoft .NET Core container
FROM microsoft/dotnet:1.0.1-sdk-projectjson
 
# Copy our code from the "/src/MyWebApi" folder to the "/app" folder in our container
WORKDIR /app
COPY ./src/RecommendationAPI .
 
# Expose port 80 for the Web API traffic
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80
 
# Restore the necessary packages
RUN dotnet restore
 
# Build and run the dotnet application from within the container
ENTRYPOINT ["dotnet", "run"]