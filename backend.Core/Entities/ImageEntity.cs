namespace backend.Core.Entities;

public class ImageEntity
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    public byte[] Data { get; set; } = null!;
}