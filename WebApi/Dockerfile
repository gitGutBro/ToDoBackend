services:
  postgres:
    image: postgres:latest
    container_name: ToDoItems
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: pass
      POSTGRES_DB: ToDoItems
    ports:
      - "5433:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U todo_user"]
      interval: 5s
      timeout: 5s
      retries: 5

volumes:
  pgdata: