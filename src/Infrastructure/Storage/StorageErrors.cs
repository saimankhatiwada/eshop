using Domain.Abstractions;

namespace Infrastructure.Storage;

public static class StorageErrors
{
    public static Error DoesNotExists = new(
        "Storage.DoesNotExists",
        "The storgae with the specified name does not exists");
}
