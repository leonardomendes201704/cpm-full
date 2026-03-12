using AppMobileCPM.Models;

namespace AppMobileCPM.Services;

public sealed class InMemoryMarketplaceRepository : IMarketplaceRepository
{
    private readonly object _writeLock = new();
    private readonly List<ServiceRequest> _serviceRequests = [];
    private readonly List<ProfessionalRegistration> _professionalRegistrations = [];
    private readonly List<SupportRequest> _supportRequests = [];

    private readonly List<ServiceCategory> _categories =
    [
        new() { Id = "eletricista", Name = "Eletricista", IconClass = "bi-lightning-charge-fill" },
        new() { Id = "encanador", Name = "Encanador", IconClass = "bi-droplet-fill" },
        new() { Id = "pedreiro", Name = "Pedreiro", IconClass = "bi-bricks" },
        new() { Id = "pintor", Name = "Pintor", IconClass = "bi-brush-fill" },
        new() { Id = "chaveiro", Name = "Chaveiro", IconClass = "bi-key-fill" },
        new() { Id = "ar-condicionado", Name = "Ar Condicionado", IconClass = "bi-snow" },
        new() { Id = "montador", Name = "Montador de Moveis", IconClass = "bi-tools" },
        new() { Id = "eletrodomesticos", Name = "Tecnico de Eletrodomesticos", IconClass = "bi-wrench-adjustable-circle-fill" },
        new() { Id = "telhado", Name = "Telhado", IconClass = "bi-house-fill" },
        new() { Id = "reformas", Name = "Reformas", IconClass = "bi-hammer" }
    ];

    private readonly List<string> _professionOptions =
    [
        "Ajudante Geral de Obras",
        "Aplicador de Massa Corrida",
        "Aplicador de Resina em Piso",
        "Arquiteto",
        "Assentador de Piso Frio",
        "Assentador de Porcelanato",
        "Azulejista",
        "Bombeiro Hidraulico",
        "Calheiro",
        "Carpinteiro",
        "Chaveiro Residencial",
        "Consultor de Reformas",
        "Controlador de Pragas",
        "Dedetizador",
        "Desentupidor",
        "Designer de Interiores",
        "Eletricista",
        "Eletricista de Manutencao",
        "Eletricista Predial",
        "Eletricista Residencial",
        "Encanador",
        "Engenheiro Civil",
        "Gesseiro",
        "Funileiro de Telhado",
        "Higienizador de Ar Condicionado",
        "Impermeabilizador",
        "Instalador de Alarmes",
        "Instalador de Ar Condicionado",
        "Instalador de Box",
        "Instalador de Cameras",
        "Instalador de Cerca Eletrica",
        "Instalador de Drywall",
        "Instalador de Energia Solar",
        "Instalador de Espelhos",
        "Instalador de Forro PVC",
        "Instalador de Gas",
        "Instalador de Interfone",
        "Instalador de Janela",
        "Instalador de Papel de Parede",
        "Instalador de Piso Laminado",
        "Instalador de Piso Vinilico",
        "Instalador de Porta e Fechadura",
        "Instalador de Portao Eletronico",
        "Instalador de Porteiro Eletronico",
        "Instalador de Rede de Protecao",
        "Instalador de Rodape",
        "Jardinagem e Poda",
        "Jardineiro",
        "Limpador de Caixa D Agua",
        "Limpeza Pos-Obra",
        "Lustrador de Piso",
        "Marceneiro",
        "Marido de Aluguel",
        "Mecanico de Bomba Hidraulica",
        "Mestre de Obras",
        "Montador de Moveis",
        "Paisagista",
        "Pedreiro",
        "Pintor Predial",
        "Pintor Residencial",
        "Pintor de Fachada",
        "Piscineiro",
        "Reparador de Persianas",
        "Reparador de Portao Eletronico",
        "Revestidor de Parede",
        "Rufeiro",
        "Serralheiro",
        "Sintequeiro",
        "Soldador",
        "Tecnico de Ar Condicionado",
        "Tecnico de Aquecedor a Gas",
        "Tecnico de Aquecedor Solar",
        "Tecnico de Bomba de Piscina",
        "Tecnico de CFTV",
        "Tecnico de Eletrodomesticos",
        "Tecnico de Fogao",
        "Tecnico de Forno Eletrico",
        "Tecnico de Freezer",
        "Tecnico de Geladeira",
        "Tecnico de Lava e Seca",
        "Tecnico de Lava-Loucas",
        "Tecnico de Maquina de Lavar",
        "Tecnico de Maquina de Secar",
        "Tecnico de Microondas",
        "Tecnico de Porta de Vidro",
        "Tecnico de Portao Eletronico",
        "Tecnico de Pressurizador",
        "Tecnico de Purificador de Agua",
        "Tecnico de Refrigerador Expositor",
        "Tecnico de Refrigeracao",
        "Tecnico de Ventilador de Teto",
        "Tecnico de TV",
        "Telhadista",
        "Texturizador",
        "Vidraceiro"
    ];

    private readonly List<Professional> _professionals =
    [
        new()
        {
            Id = 1,
            Name = "Joao Silva",
            Profession = "Eletricista",
            Description = "Eletricista residencial para instalacoes, reparos e manutencao preventiva.",
            Rating = 4.9,
            Reviews = 128,
            Distance = "2.5 km",
            Services = ["Instalacao eletrica", "Reparos", "Projetos"],
            ServicePhotoUrls =
            [
                "https://picsum.photos/seed/joao-servico-1/420/280",
                "https://picsum.photos/seed/joao-servico-2/420/280",
                "https://picsum.photos/seed/joao-servico-3/420/280",
                "https://picsum.photos/seed/joao-servico-4/420/280"
            ],
            Verified = true,
            ImageUrl = "https://picsum.photos/seed/joao/200/200",
            WhatsappUrl = "https://wa.me/5511990000001"
        },
        new()
        {
            Id = 2,
            Name = "Maria Oliveira",
            Profession = "Encanadora",
            Description = "Especialista em vazamentos, desentupimento e instalacoes hidraulicas.",
            Rating = 4.8,
            Reviews = 95,
            Distance = "3.1 km",
            Services = ["Vazamentos", "Desentupimento", "Instalacoes"],
            ServicePhotoUrls =
            [
                "https://picsum.photos/seed/maria-servico-1/420/280",
                "https://picsum.photos/seed/maria-servico-2/420/280",
                "https://picsum.photos/seed/maria-servico-3/420/280",
                "https://picsum.photos/seed/maria-servico-4/420/280"
            ],
            Verified = true,
            ImageUrl = "https://picsum.photos/seed/maria/200/200",
            WhatsappUrl = "https://wa.me/5511990000002"
        },
        new()
        {
            Id = 3,
            Name = "Carlos Santos",
            Profession = "Pedreiro",
            Description = "Pedreiro para reformas, alvenaria e acabamento com foco em qualidade.",
            Rating = 4.7,
            Reviews = 210,
            Distance = "5.0 km",
            Services = ["Reformas", "Alvenaria", "Acabamento"],
            ServicePhotoUrls =
            [
                "https://picsum.photos/seed/carlos-servico-1/420/280",
                "https://picsum.photos/seed/carlos-servico-2/420/280",
                "https://picsum.photos/seed/carlos-servico-3/420/280",
                "https://picsum.photos/seed/carlos-servico-4/420/280"
            ],
            Verified = false,
            ImageUrl = "https://picsum.photos/seed/carlos/200/200",
            WhatsappUrl = "https://wa.me/5511990000003"
        },
        new()
        {
            Id = 4,
            Name = "Ana Costa",
            Profession = "Pintora",
            Description = "Pintora residencial para pintura interna, texturas e massa corrida.",
            Rating = 5.0,
            Reviews = 42,
            Distance = "1.2 km",
            Services = ["Pintura interna", "Texturas", "Massa corrida"],
            ServicePhotoUrls =
            [
                "https://picsum.photos/seed/ana-servico-1/420/280",
                "https://picsum.photos/seed/ana-servico-2/420/280",
                "https://picsum.photos/seed/ana-servico-3/420/280",
                "https://picsum.photos/seed/ana-servico-4/420/280"
            ],
            Verified = true,
            ImageUrl = "https://picsum.photos/seed/ana/200/200",
            WhatsappUrl = "https://wa.me/5511990000004"
        },
        new()
        {
            Id = 5,
            Name = "Alan Araujo",
            Profession = "Eletricista e Tecnico de Ar Condicionado",
            Description = "Eletricista, instalacao e manutencao de ar-condicionado, alguns reparos em hidraulica.",
            Rating = 4.9,
            Reviews = 67,
            Distance = "4.3 km",
            Services = ["Eletrica residencial", "Ar-condicionado", "Reparos hidraulicos"],
            ServicePhotoUrls =
            [
                "https://picsum.photos/seed/alan-servico-1/420/280",
                "https://picsum.photos/seed/alan-servico-2/420/280",
                "https://picsum.photos/seed/alan-servico-3/420/280",
                "https://picsum.photos/seed/alan-servico-4/420/280"
            ],
            Verified = true,
            ImageUrl = "https://picsum.photos/seed/alan/200/200",
            WhatsappUrl = "https://wa.me/5511990000005"
        }
    ];

    public IReadOnlyList<ServiceCategory> GetCategories() => _categories;

    public ServiceCategory? GetCategoryById(string categoryId) =>
        _categories.FirstOrDefault(c => c.Id.Equals(categoryId, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<string> GetProfessionOptions() => _professionOptions;

    public IReadOnlyList<Professional> GetProfessionals(string? searchTerm = null)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return _professionals;
        }

        return _professionals
            .Where(p =>
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Profession.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Services.Any(s => s.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    public void AddServiceRequest(ServiceRequest request)
    {
        lock (_writeLock)
        {
            var nextId = _serviceRequests.Count + 1;
            _serviceRequests.Add(new ServiceRequest
            {
                Id = nextId,
                CategoryId = request.CategoryId,
                CategoryName = request.CategoryName,
                Description = request.Description,
                Location = request.Location,
                Name = request.Name,
                Phone = request.Phone,
                IsWhatsapp = request.IsWhatsapp,
                SubmittedAt = request.SubmittedAt
            });
        }
    }

    public void AddProfessionalRegistration(ProfessionalRegistration registration)
    {
        lock (_writeLock)
        {
            var nextId = _professionalRegistrations.Count + 1;
            _professionalRegistrations.Add(new ProfessionalRegistration
            {
                Id = nextId,
                Name = registration.Name,
                Profession = registration.Profession,
                Services = registration.Services,
                PostalCode = registration.PostalCode,
                Phone = registration.Phone,
                IsWhatsapp = registration.IsWhatsapp,
                Experience = registration.Experience,
                SubmittedAt = registration.SubmittedAt
            });
        }
    }

    public void AddSupportRequest(SupportRequest request)
    {
        lock (_writeLock)
        {
            var nextId = _supportRequests.Count + 1;
            _supportRequests.Add(new SupportRequest
            {
                Id = nextId,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Category = request.Category,
                Subject = request.Subject,
                Message = request.Message,
                SubmittedAt = request.SubmittedAt
            });
        }
    }
}
