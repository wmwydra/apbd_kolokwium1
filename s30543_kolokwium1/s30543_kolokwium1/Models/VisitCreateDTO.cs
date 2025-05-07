namespace s30543_kolokwium1.Models;

public class VisitCreateDTO
{
    public int visit_id { get; set; }
    public int client_id { get; set; }
    public string mechanic_license_number { get; set; }
    
    public List<ServiceGetDTO> services { get; set; }
}