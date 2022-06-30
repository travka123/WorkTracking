namespace WebAPI.Data.Forms;

public record TaskAddForm(string? name, int? unitId, int? quantity, 
    string? description, DateTime? reportingDate, int? firmId);
