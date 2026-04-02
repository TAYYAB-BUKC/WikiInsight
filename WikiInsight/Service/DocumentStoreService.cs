using System;
using Microsoft.Data.Sqlite;

namespace WikiInsight.Service;

public class DocumentStoreService
{
    private const string DbFile = "WikiInsight_ContentStore.db";
    static DocumentStoreService()
    {
        using var conn = new SqliteConnection($"Data Source={DbFile}");
        conn.Open();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS TBL_Documents(
                Id TEXT PRIMARY KEY,
                Title TEXT,
                Content TEXT,
                PageUrl TEXT
            }";
    }
}