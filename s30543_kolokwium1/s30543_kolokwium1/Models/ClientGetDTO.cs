namespace s30543_kolokwium1.Models;

public class ClientGetDTO
{
    public int client_id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public DateTime date_of_birth { get; set; }
}