CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250809223007_InitialCreate') THEN
    CREATE TABLE accounts (
        "Id" uuid NOT NULL,
        owner_id uuid NOT NULL,
        type integer NOT NULL,
        currency_type integer NOT NULL,
        balance numeric(18,2) NOT NULL,
        interest_rate numeric(5,4),
        open_date timestamp with time zone NOT NULL,
        close_date timestamp with time zone,
        CONSTRAINT "PK_accounts" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250809223007_InitialCreate') THEN
    CREATE TABLE transactions (
        "Id" uuid NOT NULL,
        account_id uuid NOT NULL,
        counterparty_account_id uuid,
        amount numeric(18,2) NOT NULL,
        currency integer NOT NULL,
        type integer NOT NULL,
        description text NOT NULL,
        timestamp timestamp with time zone NOT NULL,
        CONSTRAINT "PK_transactions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_transactions_accounts_account_id" FOREIGN KEY (account_id) REFERENCES accounts ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_transactions_accounts_counterparty_account_id" FOREIGN KEY (counterparty_account_id) REFERENCES accounts ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250809223007_InitialCreate') THEN
    CREATE INDEX "IX_transactions_account_id" ON transactions (account_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250809223007_InitialCreate') THEN
    CREATE INDEX "IX_transactions_counterparty_account_id" ON transactions (counterparty_account_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250809223007_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250809223007_InitialCreate', '9.0.8');
    END IF;
END $EF$;
COMMIT;

