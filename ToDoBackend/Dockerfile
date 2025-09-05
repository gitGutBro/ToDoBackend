services:
  postgres:
    image: postgres:latest
    container_name: todoitems
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: pass
      POSTGRES_DB: todoitems
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U todo_user"]
      interval: 5s
      timeout: 5s
      retries: 5

volumes:
  pgdata: