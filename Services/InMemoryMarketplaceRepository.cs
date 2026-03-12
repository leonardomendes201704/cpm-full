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

    private readonly List<string> _supportCategoryOptions =
    [
        "Duvidas sobre cadastro",
        "Problemas tecnicos na plataforma",
        "Pagamentos e cobrancas",
        "Perfil profissional e reputacao",
        "Cancelamentos e contestacoes",
        "Sugestoes e melhorias",
        "Outros assuntos"
    ];

    private readonly List<FaqItem> _supportFaqItems =
    [
        new() { Question = "Como recebo novos pedidos de servico?", Answer = "Mantenha seu perfil atualizado, com profissao principal, servicos realizados e telefone valido. Quando houver demanda na sua regiao, os contatos podem ser enviados para o numero cadastrado." },
        new() { Question = "Preciso pagar para me cadastrar como profissional?", Answer = "O cadastro inicial e gratuito. A plataforma pode oferecer planos e recursos adicionais, que sao informados separadamente antes de qualquer contratacao." },
        new() { Question = "Quanto tempo leva para analisar meu cadastro?", Answer = "A validacao inicial normalmente ocorre em ate 48 horas uteis. Em periodos de alta demanda esse prazo pode variar." },
        new() { Question = "Posso alterar minha profissao e meus servicos depois?", Answer = "Sim. Voce pode ajustar suas informacoes de perfil sempre que necessario para refletir melhor sua area de atuacao." },
        new() { Question = "Como funciona a avaliacao dos clientes?", Answer = "Clientes podem avaliar atendimentos apos a conclusao do servico. As avaliacoes ajudam outros usuarios e impactam a visibilidade do seu perfil." },
        new() { Question = "O que acontece se um cliente cancelar o servico?", Answer = "Cancelamentos devem ser tratados com transparencia entre as partes. Em casos de conflito, registre detalhes no suporte para analise." },
        new() { Question = "Como atualizo meu telefone ou CEP?", Answer = "Acesse a area de cadastro/perfil e atualize os dados. Sempre mantenha telefone e CEP corretos para evitar perda de oportunidades." },
        new() { Question = "Como reportar comportamento inadequado de cliente ou profissional?", Answer = "Use o formulario de suporte e selecione a categoria adequada. Informe data, contexto e evidencias para agilizar a apuracao." },
        new() { Question = "Posso atender em mais de uma cidade?", Answer = "Sim, desde que voce consiga cumprir prazos e deslocamento. Recomenda-se detalhar no seu perfil sua area principal de atendimento." },
        new() { Question = "Como faco para encerrar meu cadastro?", Answer = "Envie uma solicitacao no suporte com o assunto de encerramento de conta. A equipe confirmara os passos e prazos." }
    ];

    private readonly Dictionary<string, string> _siteContents = new(StringComparer.OrdinalIgnoreCase)
    {
        ["home.hero.title"] = "Resolva os problemas da sua casa",
        ["home.hero.subtitle"] = "Encontre eletricistas, encanadores, pedreiros e outros especialistas em minutos, com atendimento proximo de voce.",
        ["home.topservices.title"] = "Servicos mais buscados",
        ["home.topservices.subtitle"] = "Escolha a categoria e receba orcamentos rapidamente",
        ["home.about.title"] = "Sobre a ConsertaPraMim",
        ["home.about.subtitle"] = "Somos uma plataforma digital que conecta clientes e prestadores locais para servicos domesticos, manutencao, instalacoes e reformas.",
        ["home.about.proposal.title"] = "Nossa proposta",
        ["home.about.proposal.paragraph1"] = "A ConsertaPraMim nasceu para simplificar a contratacao de servicos e fortalecer profissionais da propria regiao. A operacao inicial comeca em Praia Grande, com foco em qualidade, organizacao e comunicacao clara.",
        ["home.about.proposal.paragraph2"] = "Aqui, cliente e profissional conversam de forma direta, recebem e enviam orcamentos com agilidade e acompanham o atendimento com mais transparencia.",
        ["home.about.differentials.title"] = "Diferenciais da plataforma",
        ["home.howitworks.title"] = "Como funciona",
        ["home.howitworks.subtitle"] = "Fluxo completo da solicitacao ate a avaliacao final",
        ["home.procta.title"] = "Voce e profissional?",
        ["home.procta.subtitle"] = "Cadastre-se no ConsertaPraMim e receba novas oportunidades todos os dias na sua cidade.",
        ["home.finalcta.title"] = "Pronto para resolver seu problema?",
        ["home.finalcta.subtitle"] = "Solicite agora e receba contatos de profissionais proximos.",
        ["legal.professional.html"] = "<p><strong>Ultima atualizacao:</strong> 12 de marco de 2026.</p><h5>1. Aceitacao</h5><p>Ao se cadastrar, o profissional concorda com estes termos e com as politicas da plataforma.</p><h5>2. Cadastro</h5><p>As informacoes devem ser verdadeiras e atualizadas, incluindo nome, telefone, CEP e servicos prestados.</p><h5>3. Responsabilidades</h5><p>O profissional e responsavel pela execucao tecnica, qualidade e conduta no atendimento.</p><h5>4. Relacao com clientes</h5><p>Valores, prazos e condicoes sao negociados diretamente entre as partes.</p><h5>5. Conduta</h5><p>Fraudes, linguagem ofensiva ou praticas abusivas podem gerar suspensao da conta.</p><h5>6. Contato</h5><p>Em caso de duvidas, utilize a Central de Suporte.</p>",
        ["legal.privacy.html"] = "<p><strong>Ultima atualizacao:</strong> 12 de marco de 2026.</p><h5>1. Coleta de dados</h5><p>Coletamos dados como nome, telefone, e-mail e CEP para operar a plataforma.</p><h5>2. Uso dos dados</h5><p>Utilizamos os dados para conectar clientes e profissionais, suporte e melhoria continua.</p><h5>3. Compartilhamento</h5><p>Compartilhamos apenas o necessario para viabilizar a prestacao do servico.</p><h5>4. Seguranca</h5><p>Adotamos medidas tecnicas e administrativas para proteger as informacoes.</p><h5>5. Direitos do titular</h5><p>Voce pode solicitar acesso, correcao e exclusao de dados conforme a legislacao aplicavel.</p>"
    };

    public IReadOnlyList<ServiceCategory> GetCategories() => _categories;

    public ServiceCategory? GetCategoryById(string categoryId) =>
        _categories.FirstOrDefault(c => c.Id.Equals(categoryId, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<string> GetProfessionOptions() => _professionOptions;

    public IReadOnlyList<string> GetSupportCategoryOptions() => _supportCategoryOptions;

    public IReadOnlyList<FaqItem> GetSupportFaqItems() => _supportFaqItems;

    public IReadOnlyDictionary<string, string> GetSiteContents() => _siteContents;

    public string? GetSiteContent(string key) =>
        _siteContents.TryGetValue(key, out var value) ? value : null;

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
