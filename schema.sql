CREATE DATABASE CryptoTradingApp;
USE CryptoTradingApp;

CREATE TABLE Users (
    UserId CHAR(36) PRIMARY KEY,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    DateRegistered DATETIME DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE Wallets (
    WalletId CHAR(36) PRIMARY KEY,
    UserId CHAR(36) NOT NULL,
    Currency VARCHAR(10) NOT NULL,
    Balance DECIMAL(18, 8) NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
CREATE TABLE Transactions (
    TransactionId CHAR(36) PRIMARY KEY,
    UserId CHAR(36) NOT NULL,
    Amount DECIMAL(18, 8) NOT NULL,
    Currency VARCHAR(10),
    Type VARCHAR(10) NOT NULL CHECK (Type IN ('Deposit', 'Withdraw')),
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Orders (
    OrderId CHAR(36) PRIMARY KEY,
    UserId CHAR(36) NOT NULL,
    CryptoCurrency VARCHAR(10) NOT NULL,
    Amount DECIMAL(18, 8) NOT NULL,
    Price DECIMAL(18, 8) NOT NULL,
    IsBuyOrder BOOLEAN NOT NULL,
    Status ENUM('Pending', 'Matched', 'Cancelled') DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
CREATE TABLE Trades (
    TradeId CHAR(36) PRIMARY KEY,
    BuyOrderId CHAR(36) NOT NULL,
    SellOrderId CHAR(36) NOT NULL,
    MatchedAmount DECIMAL(18, 8) NOT NULL,
    MatchedPrice DECIMAL(18, 8) NOT NULL,
    TradeTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (BuyOrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (SellOrderId) REFERENCES Orders(OrderId)
);
CREATE TABLE MarketData (
    DataId CHAR(36) PRIMARY KEY,
    CurrencyPair VARCHAR(20) NOT NULL,
    Price DECIMAL(18, 8) NOT NULL,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
);
