namespace Application.Dtos;

public class AlarmResponse
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MqttTopic { get; set; }
    public required int SecondsBeforeOldReading { get; set; }
    public bool? Status { get; set; } = null;
    public bool Inverted { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public static implicit operator AlarmResponse(Alarm? alarm)
    {
        if(alarm == null)
            throw new ArgumentNullException($"'{nameof(Alarm)}': Alarm cannot be null");

        return new AlarmResponse
        {
            Id = alarm.Id,
            LocationId = alarm.Id,
            Name = alarm.Name,
            Description = alarm.Description,
            MqttTopic = alarm.MqttTopic,
            SecondsBeforeOldReading = alarm.SecondsBeforeOldReading,
            Status = alarm.Status,
            Created = alarm.Created,
            Updated = alarm.Updated,
            Inverted = alarm.Inverted
        };
    }
}
