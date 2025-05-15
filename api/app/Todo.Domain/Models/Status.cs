using System.Text.Json.Serialization;

namespace Todo.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Todo,
    InProgress,
    Done
}