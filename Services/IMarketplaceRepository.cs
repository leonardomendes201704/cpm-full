using AppMobileCPM.Models;

namespace AppMobileCPM.Services;

public interface IMarketplaceRepository
{
    IReadOnlyList<ServiceCategory> GetCategories();
    ServiceCategory? GetCategoryById(string categoryId);
    IReadOnlyList<string> GetProfessionOptions();
    IReadOnlyList<Professional> GetProfessionals(string? searchTerm = null);
    void AddServiceRequest(ServiceRequest request);
    void AddProfessionalRegistration(ProfessionalRegistration registration);
    void AddSupportRequest(SupportRequest request);
}
