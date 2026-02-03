namespace EventHub.Api.Dtos;

public class CreateEventDto
{
        public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime Date { get; set; }
    public Guid OrganizerId { get; set; }
}