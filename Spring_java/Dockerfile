# ===== STAGE 1: Build =====
FROM maven:3.9-eclipse-temurin-17 AS build
WORKDIR /src
COPY . .
RUN mvn clean package -DskipTests

# ===== STAGE 2: Run =====
FROM eclipse-temurin:17-jre
WORKDIR /app
COPY --from=build /src/target/*.jar app.jar
EXPOSE 8080
ENTRYPOINT echo "🚀 Starting PhyGen..." && java -jar app.jar