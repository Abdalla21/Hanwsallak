# SQL Server Replication Setup Guide for Hanwsallak

This guide explains how to set up SQL Server replication for the Hanwsallak application to enable read-only queries from a replica database.

## Overview

The application uses two database connections:
- **WriteConnection**: Primary database for all write operations (Commands)
- **ReadOnlyConnection**: Read replica database for all read operations (Queries)

## Prerequisites

- SQL Server 2016 or later (Standard Edition or higher for replication features)
- SQL Server Management Studio (SSMS)
- Appropriate permissions (sysadmin or replication administrator)

## Option 1: Transactional Replication (Recommended)

Transactional replication provides near real-time synchronization with low latency.

### Step 1: Configure the Publisher (Primary Database)

1. Open SQL Server Management Studio and connect to your primary SQL Server instance
2. Right-click on **Replication** → **New Publication**
3. Select your primary database (`HanwsallakDB`)
4. Choose **Transactional publication**
5. Select the following tables to replicate:
   - `Trips`
   - `Shipments`
   - `Orders`
   - `AspNetUsers` (if needed for queries)
   - `AspNetRoles` (if needed)
6. Configure snapshot agent (immediate or scheduled)
7. Set security settings for the snapshot agent
8. Complete the wizard

### Step 2: Configure the Subscriber (Replica Database)

1. Create a new database named `HanwsallakDB_ReadOnly` on the same or different SQL Server instance
2. Right-click on **Replication** → **New Subscription**
3. Select your publication
4. Choose **Push subscription** (recommended) or **Pull subscription**
5. Select the subscriber database (`HanwsallakDB_ReadOnly`)
6. Configure the distribution agent
7. Set synchronization schedule (continuous for real-time, or scheduled)
8. Complete the wizard

### Step 3: Configure Read-Only Access

1. Connect to the replica database server
2. Create a new login for read-only access:
   ```sql
   CREATE LOGIN [HanwsallakReadOnly] WITH PASSWORD = 'YourStrongPassword123!';
   ```
3. Grant read-only access to the database:
   ```sql
   USE [HanwsallakDB_ReadOnly];
   CREATE USER [HanwsallakReadOnly] FOR LOGIN [HanwsallakReadOnly];
   ALTER ROLE [db_datareader] ADD MEMBER [HanwsallakReadOnly];
   ```

### Step 4: Update Connection Strings

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "WriteConnection": "Server=YourPrimaryServer;Database=HanwsallakDB;User Id=YourWriteUser;Password=YourPassword;TrustServerCertificate=true",
    "ReadOnlyConnection": "Server=YourReplicaServer;Database=HanwsallakDB_ReadOnly;User Id=HanwsallakReadOnly;Password=YourStrongPassword123!;TrustServerCertificate=true"
  }
}
```

## Option 2: Always On Availability Groups (Enterprise Edition)

If you have SQL Server Enterprise Edition, Always On Availability Groups provide high availability and read-only replicas.

### Step 1: Enable Always On

1. Open SQL Server Configuration Manager
2. Enable Always On Availability Groups feature
3. Restart SQL Server service

### Step 2: Create Availability Group

1. In SSMS, right-click **Always On High Availability** → **New Availability Group Wizard**
2. Configure primary replica
3. Add secondary replica(s)
4. Configure read-only routing (for automatic routing of read queries)
5. Complete the wizard

### Step 3: Configure Read-Only Routing

Update connection strings to use the availability group listener:

```json
{
  "ConnectionStrings": {
    "WriteConnection": "Server=AGListener;Database=HanwsallakDB;User Id=YourWriteUser;Password=YourPassword;ApplicationIntent=ReadWrite;TrustServerCertificate=true",
    "ReadOnlyConnection": "Server=AGListener;Database=HanwsallakDB;User Id=HanwsallakReadOnly;Password=YourPassword;ApplicationIntent=ReadOnly;TrustServerCertificate=true"
  }
}
```

## Option 3: Log Shipping (Simpler Alternative)

Log Shipping is simpler but has higher latency than transactional replication.

### Step 1: Configure Log Shipping

1. Right-click on your database → **Properties** → **Transaction Log Shipping**
2. Enable log shipping
3. Configure backup settings
4. Add secondary database
5. Configure restore settings
6. Set up monitoring

### Step 2: Configure Read-Only Access

Same as Step 3 in Transactional Replication section.

## Testing the Replication

1. Insert test data into the primary database
2. Wait for replication to sync (depends on your configuration)
3. Query the replica database to verify data is synchronized
4. Test that write operations fail on the read-only connection (they should throw an exception)

## Monitoring

- Use SQL Server Replication Monitor to monitor replication status
- Check replication latency
- Monitor replication agent status
- Set up alerts for replication failures

## Troubleshooting

### Common Issues

1. **Replication not syncing**: Check agent status, network connectivity, and permissions
2. **High latency**: Consider adjusting sync schedule or using continuous sync
3. **Connection errors**: Verify connection strings and firewall rules
4. **Read-only errors**: Ensure the read-only user has proper permissions

### Useful Queries

```sql
-- Check replication status
SELECT * FROM sys.dm_repl_articles;

-- Check subscription status
SELECT * FROM distribution.dbo.MSsubscriptions;

-- Check latency
SELECT * FROM sys.dm_repl_traninfo;
```

## Notes

- For development, you can use the same database for both connections (not recommended for production)
- Ensure proper backup strategies for both primary and replica databases
- Monitor disk space on both servers
- Consider using separate servers for better performance isolation

## Security Considerations

- Use strong passwords for database users
- Encrypt connections (TrustServerCertificate=true in connection strings)
- Limit network access to database servers
- Regularly update SQL Server and apply security patches
- Use Windows Authentication where possible

