using System.Collections.Generic;
using MultiSeguroViagem.Domain.Entities;
using MultiSeguroViagem.Domain.Interfaces.Repositories;
using MultiSeguroViagem.Domain.Interfaces.Services.Application;

namespace MultiSeguroViagem.Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepo;

        public BannerService(IBannerRepository bannerRepo)
        {
            _bannerRepo = bannerRepo;
        }

        public List<Banner> Busca()
        {
            return _bannerRepo.Busca();
        }
    }
}
