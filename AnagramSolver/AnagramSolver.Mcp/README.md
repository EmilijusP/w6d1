# AnagramSolver.Mcp

MCP (Model Context Protocol) server that exposes anagram-finding functionality via the `find_anagrams` tool over `stdio` transport.

## Prerequisites

- .NET 8.0 SDK
- Running instance of `AnagramSolver.Api` (default: `https://localhost:7267/`)

## Configuration

The API base URL can be configured via environment variable or `appsettings.json`:

```json
{
  "AnagramApi": {
    "BaseUrl": "https://localhost:7079/"
  }
}
```

## Running

1. Start the API first:
   ```bash
   dotnet run --project AnagramSolver.Api
   ```

2. Start the MCP server:
   ```bash
   dotnet run --project AnagramSolver.Mcp
   ```

The server communicates over `stdio` and is configured in `.vscode/mcp.json` for VS Code Copilot integration.

## MCP Tool

### `find_anagrams`

| Parameter | Type   | Description                      |
|-----------|--------|----------------------------------|
| `word`    | string | The word to find anagrams for    |

Returns a text response with the list of found anagrams or an error message.
