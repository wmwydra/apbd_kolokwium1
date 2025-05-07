namespace s30543_kolokwium1.Models;

public class VisitGetDTO
{
    public int visit_id { get; set; }
    public ClientGetDTO client_id { get; set; }
    public MechanicGetDTO mechanic_id { get; set; }
    public DateTime date { get; set; }
    
    public List<ServiceGetDTO> services { get; set; }
}