using s30543_kolokwium1.Models;

namespace s30543_kolokwium1.Services;

public interface iVisitService
{
    Task<bool> DoesVisitExist(int visitId);
    Task<VisitGetDTO> GetVisitByIdAsync(int visitId);
    Task<ClientGetDTO> GetClientByIdAsync(int clientId);
    
    Task<MechanicGetDTO> GetMechanicByIdAsync(int mechanicId);
    
    Task<List<ServiceGetDTO>> GetVisitServicesAsync(int visitId);
    Task<MechanicGetDTO> GetMechanicByLicenseAsync(string lNumber);

    Task<VisitGetDTO> AddVisitAsync(VisitCreateDTO visit); 
}